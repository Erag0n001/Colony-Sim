using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Client
{
    public class Map
    {
        public Vector2 size;
        public Dictionary<Vector3Int, TileData> tiles = new Dictionary<Vector3Int, TileData>();
        public Dictionary<Vector3Int, Creature> creatures = new Dictionary<Vector3Int, Creature>();
        public readonly int seed;
        public List<TerrainBase> terrains;
        public Tilemap tilemap;
        public Map(string seed, Vector2 size)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(seed);
            this.seed = BitConverter.ToInt32(bytes, 0);
            this.size = size;
        }
        public void UpdateTerrain(TileData[] tiles) 
        {
            Action todo = () =>
            {
                foreach (TileData tile in tiles)
                {
                    Tile newTile = AssetManager.allTiles.Where(T => tile.type.IdName == T.name).First();

                    tilemap.SetTile(tile.position, newTile);
                }
            };
            UnityMainThreadDispatcher.instance.Enqueue(todo);
        }
        public TileData GetTileFromVector(Vector3Int vector) => tiles.TryGetValue(vector, out TileData tile) ? tile : null;
    }
}
