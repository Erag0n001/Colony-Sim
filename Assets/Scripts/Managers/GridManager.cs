using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;
namespace Client
{
    public static class GridManager
    {

        public static GameObject tileObject;

        public static Random rand;
        public static void GenerateMap(int width = 25, int height = 25, int zLevel = 10, string seed = "") 
        {
            rand = new Random();
            if (seed == "") seed = rand.Next(0, 9999999).ToString();
            Map map = new Map(seed, new Vector2(width, height), zLevel);
            map.terrains = new List<TerrainBase>() { TerrainBase.FindTerrainByID("Grass")};

            map.tileSetTiles = GetTiles(map);
            MainManager.currentMap = map;

            for (int z = 0; z < zLevel; z++)
            {
                GenerateGrid(map, z);

                foreach (TileData tile in map.layers.Last().Value.tiles.Values)
                {
                    tile.CacheNeighbors();
                }
            }

            Printer.LogWarning(map.layers.Count().ToString());
            CreatureManager.SpawnNewCreature();
        }
        public static void GenerateGrid(Map map, int zLevel)
        {
            MapLayer mapLayer = new MapLayer(zLevel);
            Dictionary<Position, TileData> tiles = new Dictionary<Position, TileData>();

            int idCount = 0;
            Vector2 offset = new Vector2(rand.Next(-999999, 999999), rand.Next(-999999, 999999));

            Tilemap tileMap = CreateTileMap(zLevel);

            for (int x = 0; x < map.size.x; x++)
            {
                //yield here
                for (int y = 0; y < map.size.y; y++)
                {
                    Position pos = new Position(x, y, zLevel);

                    TileData tile = CalculateTile(x, y, zLevel, map, offset);
                    tile.id = idCount++;
                    tile.position = pos;
                    Tile newTile = AssetManager.allTiles.Where(T =>tile.type.IdName == T.name).First();
                    idCount++;
                    tiles.Add(pos, tile);
                    tileMap.SetTile(pos.ToVector3Int(),newTile);
                }
            }
            mapLayer.tilemap = tileMap;
            mapLayer.tiles = tiles;
            map.layers.Add(zLevel,mapLayer);
        }

        private static TileData CalculateTile(int x, int y, int z, Map map, Vector2 offset = new Vector2())
        {
            int scale = 1;
            float xCoord = (float)x / map.size.x * scale + offset.x;
            float yCoord = (float)y / map.size.y * scale + offset.y;

            TileData tile = new TileData(TerrainBase.terrainList[0]);
            tile.wetness = CalculateWetness();
            tile.elevation = CalculateElevation();
            if (tile.wetness < 0.30 && z <= 5) tile.type = TerrainBase.FindTerrainByID("Water");
            else if (tile.elevation < z * 0.2) tile.type = TerrainBase.FindTerrainByID("Stone");
            //else tile.type = map.terrains[Mathf.FloorToInt(tile.elevation)];
            else tile.type = TerrainBase.FindTerrainByID("Grass");
            return tile;

            float CalculateWetness()
            {
                float wetness = Mathf.Clamp(Mathf.PerlinNoise(xCoord * 2, yCoord * 2),0, 0.999999f);
                return wetness;
            }
            float CalculateElevation()
            {
                float elevation = Mathf.Clamp(Mathf.PerlinNoise(xCoord * 3, yCoord * 3),0, 0.999999f);
                return elevation;
            }
            float CalculateTerrainType() 
            {
                float type = Mathf.Clamp(Mathf.PerlinNoise(xCoord, yCoord), 0, 0.999999f) * map.terrains.Count();
                return type;
            }
        }

        public static Tilemap CreateTileMap(int zLevel) 
        {
            GameObject tileMapObject = new GameObject();
            tileMapObject.transform.SetParent(GameObject.Find("GridManager/Grid").transform);
            tileMapObject.name = zLevel.ToString();
            tileMapObject.transform.position = new Vector3(0, 0, 0);
            Tilemap tilemap = tileMapObject.AddComponent<Tilemap>();
            tileMapObject.AddComponent<TilemapRenderer>();
            if (zLevel != 0) tileMapObject.SetActive(false);
            return tilemap;
        }

        public static Tile[] GetTiles(Map map) 
        {
            List<Tile> result = new List<Tile>();
            foreach (TerrainBase data in map.terrains)
            {
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = data.texture;
                tile.name = data.IdName;
                result.Add(tile);
            }
            return result.ToArray();
        }
    }
}
