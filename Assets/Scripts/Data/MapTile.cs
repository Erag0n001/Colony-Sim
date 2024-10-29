using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Client
{
    [Serializable]
    public class MapTile
    {
        [NonSerialized] private static readonly List<Position> Directions = new List<Position>() {
            new Position(0, 1, 0), new Position(-1, 0, 0), new Position(0, -1, 0), new Position(1, 0, 0),
            new Position(-1, 1, 0), new Position(-1, 1,0), new Position(1, -1, 0), new Position(1, 1, 0)
        };
        private static readonly List<Position> ZDirections = new List<Position>() {new Position(0,0,1), new Position(0,0,-1)};
        public Position position = new Position();
        public int id;
        
        public List<int> zLevelAccessCache = new List<int>(); //We cache z levels this tile can reach, if any. Also cache if it's part of a chain of staircases
        public bool hasZLevelAccess;
        public float WalkSpeed => baseType.WalkModifier * walkspeedmultipier;
        private float walkspeedmultipier = 1;
        public TerrainBase baseType;
        public readonly TerrainBase MainType;
        public List<MapTile> neighbor = new List<MapTile>();

        public MapTile(TerrainBase type) 
        {
            this.baseType = type;
            this.MainType = type;
        }

        public void CacheNeighbors(MapLayer layer = null) 
        {
            if (layer != null)
            {
                foreach (MapTile tile in Directions.Select(dir => MainManager.currentMap.GetTileFromVector(position + dir, layer)).Where(tile => tile != null))
                {
                    neighbor.Add(tile);
                }
            } else 
            {
                foreach (MapTile tile in Directions.Select(dir => MainManager.currentMap.GetTileFromVector(position + dir)).Where(tile => tile != null))
                {
                    neighbor.Add(tile);
                }
            }
            if(position.x == 0 && position.y == 0) 
            {
                foreach (MapTile tile in ZDirections.Select(dir => MainManager.currentMap.GetTileFromVector(position + dir)).Where(tile => tile != null))
                {
                    zLevelAccessCache.Add(tile.position.z);
                    hasZLevelAccess = true;
                    neighbor.Add(tile);
                    if (tile.hasZLevelAccess)
                    {
                        for (int i = 0; i < tile.zLevelAccessCache.Count; i++)
                        {
                            if (!zLevelAccessCache.Any(T => T == tile.zLevelAccessCache[i])) zLevelAccessCache.Add(tile.zLevelAccessCache[i]);
                        }
                        for (int i = 0; i < zLevelAccessCache.Count; i++)
                        {
                            if (!tile.zLevelAccessCache.Any(T => T == zLevelAccessCache[i])) tile.zLevelAccessCache.Add(zLevelAccessCache[i]);
                        }
                    }
                }
                zLevelAccessCache.Sort();
            }
        }
        public float GetRawDistance(Position other) 
        {
            Position distance = new Position(Mathf.Abs((int)(position.x - other.x)),Mathf.Abs((int)(position.y - other.y)));
            return distance.x + distance.y;
        }

        public float GetDistance(Position other)
        {
            float cost = 0f;
            Position simulatedTile = new Position(position.x, position.y, position.z);
            float diagonalCostMultiplier = (float)Math.Sqrt(2);

            while (!simulatedTile.Equals(other))
            {
                int deltaX = other.x - simulatedTile.x;
                int deltaY = other.y - simulatedTile.y;
                int deltaZ = other.z - simulatedTile.z;

                bool diagonal = false;

                if (deltaX != 0 && deltaY != 0)
                {
                    simulatedTile.x += Math.Sign(deltaX);
                    simulatedTile.y += Math.Sign(deltaY);
                    diagonal = true;
                }
                else
                {

                    if (deltaX != 0)
                        simulatedTile.x += Math.Sign(deltaX);
                    if (deltaY != 0)
                        simulatedTile.y += Math.Sign(deltaY);
                }


                if (deltaZ != 0)
                {
                    simulatedTile.z += Math.Sign(deltaZ);
                }


                MapTile currentTile = MainManager.currentMap.GetTileFromVector(simulatedTile);


                if (diagonal)
                {
                    cost += diagonalCostMultiplier * (2 - currentTile.walkspeedmultipier);
                }
                else
                {
                    cost += 2 - currentTile.walkspeedmultipier;
                }
            }
            return cost;
        }

    }
}