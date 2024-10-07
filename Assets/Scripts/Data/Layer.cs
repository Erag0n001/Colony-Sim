using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Client 
{
    public class MapLayer 
    {
        public Map map;
        public readonly int ZLevel;

        public Dictionary<Position, TileData> tiles = new Dictionary<Position, TileData>();
        public Dictionary<Position, Creature> creatures = new Dictionary<Position, Creature>();

        public Tilemap tilemap;

        public TileData GetTileFromVector(Position vector) => tiles.TryGetValue(vector, out TileData tile) ? tile : null;

        public void UpdateTerrain(TileData[] tiles)
        {
            Action todo = () =>
            {
                foreach (TileData tile in tiles)
                {
                    Tile newTile = AssetManager.allTiles.Where(T => tile.type.IdName == T.name).First();

                    tilemap.SetTile(tile.position.ToVector3Int(), newTile);
                }
            };
            UnityMainThreadDispatcher.instance.Enqueue(todo);
        }

        public MapLayer(int zLevel)
        {
            this.ZLevel = zLevel;
        }
        }
    }