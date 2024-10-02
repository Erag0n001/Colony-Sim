using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Client
{
    [Serializable]
    public class TileData
    {
        [NonSerialized] private static readonly List<Vector3Int> Directions = new List<Vector3Int>() {
            new Vector3Int(0, 1, 0), new Vector3Int(-1, 0, 0), new Vector3Int(0, -1, 0), new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 1, 0), new Vector3Int(-1, 1,0), new Vector3Int(1, -1, 0), new Vector3Int(1, 1, 0)
        };

        public Vector3Int position = new Vector3Int();
        public int id;
        public float wetness;
        public float elevation;
        public float WalkSpeed => type.WalkModifier * walkspeedmultipier;
        public float walkspeedmultipier = 1;
        public TerrainBase type;
        public readonly TerrainBase MainType;
        public List<TileData> neightbors = new List<TileData>();

        public float pathfindingF = 0f;
        public float pathfindingH = 0f;
        public float pathfindingG = 0f;
        public TileData connection;

        public TileData(TerrainBase type) 
        {
            this.type = type;
            this.MainType = type;
        }
        public void CacheNeighbors() 
        {
            foreach (TileData tile in Directions.Select(dir => MainManager.currentMap.GetTileFromVector(position + dir)).Where(tile => tile != null))
            {
                neightbors.Add(tile);
            }
        }
        public float GetRawDistance(Vector3Int other) 
        {
            Vector3Int distance = new Vector3Int(Mathf.Abs((int)(position.x - other.x)),Mathf.Abs((int)(position.y - other.y)));
            return distance.x + distance.y;
        }

        public float GetDistance(Vector3Int other) 
        {
            float cost = 0f;
            Vector3Int simulatedTile = new Vector3Int(position.x, position.y, 0);
            float diagonalCostMultiplier = (float)Math.Sqrt(2);
            while (simulatedTile != other) 
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