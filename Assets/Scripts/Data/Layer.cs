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

        public Dictionary<Position, MapTile> tiles = new Dictionary<Position, MapTile>();
        public Dictionary<Position, Creature> creatures = new Dictionary<Position, Creature>();
        public Tilemap tilemap;

        public MapTile GetTileFromVector(Position vector) => tiles.TryGetValue(vector, out MapTile tile) ? tile : null;

        public void UpdateTerrain(MapTile[] tiles)
        {
            Action todo = () =>
            {
                foreach (MapTile tile in tiles)
                {
                    Tile newTile = AssetManager.allTiles.Where(T => tile.baseType.IdName == T.name).First();

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