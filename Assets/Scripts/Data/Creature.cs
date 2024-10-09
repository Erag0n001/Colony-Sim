using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class Creature
    {
        public Position position;
        public Vector3 ObjectPosition => position.ToVector3();
        public Position targetPos;
        public readonly CreatureBase baseType;
        public float speed;

        public string displayName;
        public Creature(CreatureBase data) 
        {
            this.baseType = data;
            speed = data.BaseSpeed;
        }

        public void Tick(float delta) 
        {

        }

        public List<MapTile> FindPathToDestination(Position start, Position end) 
        {
            return FindPathToDestination(MainManager.currentMap.GetTileFromVector(start), MainManager.currentMap.GetTileFromVector(end));
        }
        public List<MapTile> FindPathToDestination(MapTile start, MapTile end)
        {
            if (end.WalkSpeed < 0.05 || start.WalkSpeed < 0.05)
            {
                return null; // Early exit if start or end is not walkable
            }

            PathfindingInfo info = new PathfindingInfo();
            info.toSearch.Add(start);
            info.gCost[start] = 0;
            info.hCost[start] = start.GetDistance(end.position);
            info.fCost[start] = info.hCost[start]; // Set fCost for start

            while (info.toSearch.Count > 0)
            {
                // Get the tile with the lowest fCost
                MapTile current = info.toSearch.OrderBy(t => info.fCost[t]).First();

                if (current.position == end.position)
                {
                    // Build the path back to start
                    return BuildPath(start, end, info);
                }

                info.toSearch.Remove(current);
                info.searched.Add(current);

                foreach (MapTile neighbor in current.neighbor.Where(t => t.WalkSpeed > 0.05 && !info.searched.Contains(t)))
                {
                    float costToNeighbor = info.gCost[current] + current.GetDistance(neighbor.position);

                    // Check if neighbor is not in toSearch or found a better path
                    if (!info.toSearch.Contains(neighbor) || costToNeighbor < info.gCost[neighbor])
                    {
                        info.gCost[neighbor] = costToNeighbor;
                        info.hCost[neighbor] = neighbor.GetDistance(end.position);
                        info.fCost[neighbor] = info.gCost[neighbor] + info.hCost[neighbor];
                        info.connection[neighbor] = current; // Set connection

                        if (!info.toSearch.Contains(neighbor))
                        {
                            info.toSearch.Add(neighbor);
                        }
                    }
                }
            }
            return null; // No path found
        }

        private List<MapTile> BuildPath(MapTile start, MapTile end, PathfindingInfo info)
        {
            List<MapTile> path = new List<MapTile>();
            MapTile current = end;

            while (current != start)
            {
                path.Add(current);
                current.baseType = TerrainBase.FindTerrainByID("Enemy");
                current = info.connection[current]; // Move to the connected tile
            }
            path.Add(start);
            path.Reverse();
            MainManager.currentMap.UpdateTerrain(path.ToArray());
            return path;
        }
    }
}
