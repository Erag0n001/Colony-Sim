using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client
{
    public class Map
    {
        public Vector2 size;
        public int zLevels;
        public Dictionary<int,MapLayer> layers = new Dictionary<int,MapLayer>();
        public readonly int seed;

        public Vector2 offset;
        public float scale;

        public BiomeBase biome;

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

        public MapTile GetTileFromVector(Position vector)
        {
            MapLayer layer;
            layers.TryGetValue(vector.z, out layer);
            if (layer != null)
            {
                return layer.GetTileFromVector(vector);
            } else 
            {
                Printer.LogError($"Layer {vector.z} did not exist");
            }
            return null;
        }

        public MapTile GetTileFromVector(Position vector, MapLayer layer)
        {
            return layer.tiles.TryGetValue(vector, out MapTile tile) ? tile : null;
        }

        public void UpdateTerrain(MapTile[] tiles)
        {
            foreach (MapTile tile in tiles)
            {
                layers.TryGetValue(tile.position.z, out MapLayer mapLayer);
                mapLayer.UpdateTerrain(new MapTile[] {tile});
            }
        }

        public void UpdateAllTerrain() 
        {
            foreach(MapLayer layer in layers.Values) 
            {
                layer.UpdateTerrain(layer.tiles.Values.ToArray());
            }
        }
    }
}
