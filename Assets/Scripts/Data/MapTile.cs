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

        public Position position = new Position();
        public int id;

        public float WalkSpeed => baseType.WalkModifier * walkspeedmultipier;
        public float walkspeedmultipier = 1;
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
        }
        public float GetRawDistance(Position other) 
        {
            Position distance = new Position(Mathf.Abs((int)(position.x - other.x)),Mathf.Abs((int)(position.y - other.y)));
            return distance.x + distance.y;
        }

        public float GetDistance(Position other) 
        {
            float cost = 0f;
            Position simulatedTile = new Position(position.x, position.y, 0);
            float diagonalCostMultiplier = (float)Math.Sqrt(2);
            while (!simulatedTile.Equals(other)) 
            {
                bool diagonal = false;
                if(simulatedTile.x < other.x && simulatedTile.y < other.y) 
                {
                    simulatedTile.x += 1;
                    simulatedTile.y += 1;
                    diagonal = true;
                }
                if (simulatedTile.x > other.x && simulatedTile.y < other.y)
                {
                    simulatedTile.x -= 1;
                    simulatedTile.y += 1;
                    diagonal = true;
                }
                if (simulatedTile.x < other.x && simulatedTile.y > other.y)
                {
                    simulatedTile.x += 1;
                    simulatedTile.y -= 1;
                    diagonal = true;
                }
                if (simulatedTile.x > other.x && simulatedTile.y > other.y)
                {
                    simulatedTile.x -= 1;
                    simulatedTile.y -= 1;
                    diagonal = true;
                }
                if (!diagonal)
                {
                    if (simulatedTile.x < other.x)
                    {
                        simulatedTile.x += 1;
                    }
                    if (simulatedTile.x > other.x)
                    {
                        simulatedTile.x -= 1;
                    }
                    if (simulatedTile.y < other.y)
                    {
                        simulatedTile.y += 1;
                    }
                    if (simulatedTile.y > other.y)
                    {
                        simulatedTile.y -= 1;
                    }
                }

                if (diagonal)
                {
                    cost += diagonalCostMultiplier * (2 - MainManager.currentMap.GetTileFromVector(simulatedTile).walkspeedmultipier);
                }
                else
                {
                    cost += 2 - MainManager.currentMap.GetTileFromVector(simulatedTile).walkspeedmultipier;
                }
            }
            return cost;
        }
    }
}