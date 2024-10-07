using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class Creature
    {
        public Position position;
        public Vector3 objectPosition;
        public Position targetPos;
        public readonly CreatureBase type;
        public float speed;

        public string displayName;
        public Creature(CreatureBase data) 
        {
            this.type = data;
            speed = data.BaseSpeed;
        }

        public void Tick(float delta) 
        {

        }

        public List<TileData> FindPathToDestination(Position start, Position end) 
        {
            return FindPathToDestination(MainManager.currentMap.GetTileFromVector(start), MainManager.currentMap.GetTileFromVector(end));
        }
        public List<TileData> FindPathToDestination(TileData start, TileData end)
        {
            List<TileData> toSearch = new List<TileData>() { start };
            List<TileData> searched = new List<TileData>();
            if (end.WalkSpeed < 0.05) 
            {
                return null;
            }
            while (toSearch.Any())
            {
                TileData current = toSearch.First();
                foreach (TileData t in toSearch)
                {
                    if (t.pathfindingF < current.pathfindingF || t.pathfindingF == current.pathfindingF && t.pathfindingH < current.pathfindingH) current = t;
                }
                searched.Add(current);
                toSearch.Remove(current);
                if (current.position == end.position)
                {
                    TileData currentPathTile = end;
                    List<TileData> path = new List<TileData>();
                    while (currentPathTile != start)
                    {
                        path.Add(currentPathTile);
                        currentPathTile.type = TerrainBase.FindTerrainByID("Enemy");
                        currentPathTile = currentPathTile.connection;
                    }
                    path.Add(start);
                    path.Reverse();
                    MainManager.currentMap.UpdateTerrain(path.ToArray());
                    return path;
                }
                foreach(TileData neightbor in current.neightbors.Where(T => T.WalkSpeed > 0.05 && !searched.Contains(T))) 
                {
                    bool inSearch = toSearch.Contains(neightbor);

                    float costToNeighbor = current.pathfindingG + current.GetDistance(neightbor.position);
                    if(!inSearch || costToNeighbor < neightbor.pathfindingG) 
                    {
                        neightbor.pathfindingG = costToNeighbor;
                        neightbor.connection = current;
                        if (!inSearch)
                        {
                            neightbor.pathfindingH = neightbor.GetDistance(end.position);
                            toSearch.Add(neightbor);
                        }
                    }
                }
            }
            return null;
        }
    }
}
