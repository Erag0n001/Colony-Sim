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
        public int zLevels;
        public Dictionary<int,MapLayer> layers = new Dictionary<int,MapLayer>();
        public readonly int seed;
        public List<TerrainBase> terrains;

        public Tile[] tileSetTiles;

        private int activeLayer;
        public int ActiveLayer 
        {
            get 
            {
                return activeLayer;
            }
            set 
            {if(value >= 0 && value < layers.Count)
                {
                    Printer.LogWarning(value.ToString());
                    Printer.Log(activeLayer.ToString());
                    layers[activeLayer].tilemap.gameObject.SetActive(false);
                    activeLayer = value;
                    layers[value].tilemap.gameObject.SetActive(true);
                }
            } 
        }
        public Map(string seed, Vector2 size, int zLevels)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(seed);
            this.seed = BitConverter.ToInt32(bytes, 0);
            this.size = size;
            this.zLevels = zLevels;
        }

        public TileData GetTileFromVector(Position vector)
        {
            MapLayer layer;
            layers.TryGetValue(vector.z, out layer);
            if (layer != null)
            {
                return layer.tiles.TryGetValue(vector, out TileData tile) ? tile : null;
            } else 
            {
                Printer.LogError($"Layer {vector.z} did not exist");
            }
            return null;
        }

        public void UpdateTerrain(TileData[] tiles)
        {
            foreach (TileData tile in tiles)
            {
                layers.TryGetValue(tile.position.z, out MapLayer mapLayer);
            }
        }
    }
}
