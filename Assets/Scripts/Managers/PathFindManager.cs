using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace Client
{
    public static class PathFindManager
    {
        public static List<MapTile> PathFindTo(Position start, Position end)
        {
            return PathFindTo(MainManager.currentMap.GetTileFromVector(start), MainManager.currentMap.GetTileFromVector(end));
        }
        public static List<MapTile> PathFindTo(MapTile start, MapTile end, List<MapTile> existingPath = null) 
        {
            bool directAccess = true;
            Queue<MapTile> zAccess = null;
            Printer.LogWarning(start.ToString());
            Printer.LogWarning(end .ToString());
            List<MapTile>? zAccessTemp = FindDirectClosestZLevelAccess(start, end.position.z);// Check if there's a direct way up
            if (zAccessTemp == null)
            {
                zAccessTemp = FindFirstZLevelAccessTowardTarget(start, end.position.z); // We didn't find a direct access
                directAccess = false;
            }
            if (start.position.z != end.position.z) 
                if(zAccessTemp == null) return null; //We did not find a path to that z level
                else zAccess = new Queue<MapTile>(zAccessTemp);
            List<MapTile> result = new List<MapTile>();
            bool foundPath = false;
            if (start.position.z != end.position.z)
            {
                if (zAccess != null)
                {
                    while (foundPath == false)
                    {
                        if (zAccess.Count <= 0) return null;
                        result = FindPathToDestination(start, zAccess.Dequeue()); //Perform A* to the beginning of the stairs
                        if(result != null && directAccess) 
                        {
                            List<MapTile> result2 = FindPathToDestination(result.Last(), end); //Perform A* toward the end tile from the bottom of the stairs
                            if(result2 != null) 
                            {
                                result.AddRange(result2); //We found a complete path
                                return result;
                            }
                        }
                        else if (result != null)
                        {
                            Position newStart = new Position();
                            if (start.position.z > end.position.z)
                            {
                                newStart.x = result.Last().position.x;
                                newStart.y = result.Last().position.y;
                                newStart.z = result.Last().zLevelAccessCache.First();
                                return PathFindTo(newStart, end.position); //We try to find the next stairway up to that floor
                            }
                            else
                            {
                                newStart.x = result.Last().position.x;
                                newStart.y = result.Last().position.y;
                                newStart.z = result.Last().zLevelAccessCache.Last();
                                return PathFindTo(newStart, end.position); //We try to find the next stairway down to that floor
                            }
                        }
                    }
                }
            } 
            else // On the same zlevel
            {
                while (foundPath == false)
                {
                    result = FindPathToDestination(start, end); //Perform A* to destination
                    Printer.Log(foundPath.ToString());
                    if (result != null)
                    {
                        return result;
                    }
                    else 
                    {
                        return null;
                    }
                }
            }
            return null;
        }
        private static List<MapTile> FindPathToDestination(MapTile start, MapTile end)
        {
            if (end.WalkSpeed < 0.05 || start.WalkSpeed < 0.05)
            {
                return null;
            }
            List<MapTile> result = new List<MapTile>();
            PathfindingInfo info = new PathfindingInfo();
            info.toSearch.Add(start);
            info.gCost[start] = 0;
            info.hCost[start] = start.GetDistance(end.position);
            info.fCost[start] = info.hCost[start];

            while (info.toSearch.Count > 0)
            {
                MapTile current = info.toSearch.OrderBy(t => info.fCost[t]).ThenBy(t => info.hCost[t]).First();

                if (current.position == end.position)
                {
                    return BuildPath(start, end, info);
                }

                info.toSearch.Remove(current);
                info.searched.Add(current);

                foreach (MapTile neighbor in current.neighbor.Where(t => t.WalkSpeed > 0.05 && !info.searched.Contains(t)))
                {
                    float costToNeighbor = info.gCost[current] + current.GetDistance(neighbor.position);

                    if (!info.toSearch.Contains(neighbor) || costToNeighbor < info.gCost[neighbor])
                    {
                        info.gCost[neighbor] = costToNeighbor;
                        info.hCost[neighbor] = neighbor.GetDistance(end.position);
                        info.fCost[neighbor] = info.gCost[neighbor] + info.hCost[neighbor];
                        info.connection[neighbor] = current;

                        if (!info.toSearch.Contains(neighbor))
                        {
                            info.toSearch.Add(neighbor);
                        }
                    }
                }
            }
            return null;
        }

        private static List<MapTile> BuildPath(MapTile start, MapTile end, PathfindingInfo info)
        {
            List<MapTile> path = new List<MapTile>();
            MapTile current = end;
            string toPrint = "Pathfinding results = \n";
            while (current != start)
            {
                path.Add(current);
                current.baseType = TerrainBase.FindTerrainByID("Enemy");
                toPrint += current.position + "\n";
                current = info.connection[current];
            }
            Printer.LogError(toPrint);
            path.Add(start);
            path.Reverse();
            return path;
        }

        private static List<MapTile> FindDirectClosestZLevelAccess(MapTile start, int targetZLevel) // We try to find a direct way up, ex stairs on top of stairs
        {
            Queue<MapTile> toSearch = new Queue<MapTile>();
            HashSet<MapTile> visited = new HashSet<MapTile>();
            List<MapTile> zLevelTiles = new List<MapTile>();
            List<MapTile> results = new List<MapTile> ();

            toSearch.Enqueue(start);
            visited.Add(start);

            while (toSearch.Count > 0)
            {
                MapTile current = toSearch.Dequeue();
                if(current.WalkSpeed <= 0.05) continue;
                if (current.hasZLevelAccess) zLevelTiles.Add(current);
                else continue;
                if (current.zLevelAccessCache.Contains(targetZLevel))
                {
                    results.Add(current);
                }

                foreach (MapTile neighbor in current.neighbor.Where(t => !visited.Contains(t)))
                {
                    toSearch.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
            if(results.Count != 0) return results;
            else return null;
        }

        private static List<MapTile> FindFirstZLevelAccessTowardTarget(MapTile start, int targetZLevel, List<MapTile> tilesToCheck = null, bool preCalculatedTiles = false)
        {
            Queue<MapTile> toSearch = null;
            if (preCalculatedTiles) toSearch = new Queue<MapTile>(tilesToCheck);
            else toSearch = new Queue<MapTile>();

            HashSet<MapTile> visited = new HashSet<MapTile>();
            List<MapTile> results = new List<MapTile>();

            toSearch.Enqueue(start);
            visited.Add(start);

            while (toSearch.Count > 0)
            {
                MapTile current = toSearch.Dequeue();

                if (targetZLevel > start.position.z)
                {
                    if (current.zLevelAccessCache.Any(T => T > start.position.z))
                    {
                        results.Add(current);
                    }
                }
                else if (targetZLevel < start.position.z)
                {
                    if (current.zLevelAccessCache.Any(T => T < start.position.z))
                    {
                        results.Add(current);
                    }
                }
                if (preCalculatedTiles == false)
                {
                    foreach (MapTile neighbor in current.neighbor.Where(t => !visited.Contains(t)))
                    {
                        toSearch.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
            if (results.Count != 0) return results;
            else return null; // No tile with z-level access found, layer is unreachable
        }
    }
}