using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using Random = System.Random;
namespace Client
{
    public static class GridManager
    {

        public static GameObject tileObject;

        public static Random rand;
        public static async void GenerateMap(int width = 250, int height = 250, int zLevel = 20, string seed = "") 
        {
            rand = new Random();
            if (seed == "") seed = rand.Next(0, 9999999).ToString();
            Map map = new Map(seed, new Vector2(width, height), zLevel);
            map.terrains = new List<TerrainBase>() { TerrainBase.FindTerrainByID("Grass")};

            map.tileSetTiles = GetTiles(map);
            MainManager.currentMap = map;
            map.zLevels = zLevel;

            Vector2 offset = new Vector2(rand.Next(-999999, 999999), rand.Next(-999999, 999999));

            List<Task> tasks = new List<Task>();
            for (int z = 0; z < zLevel; z++)
            {
                int currentZLevel = z;
                tasks.Add(Task.Run(() => GenerateGrid(map, currentZLevel, offset)));
            }
            await Task.WhenAll(tasks);
            map.layers = map.layers.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            CreatureManager.SpawnNewCreature();
            }
        public static void GenerateGrid(Map map, int zLevel, Vector2 offset)
        {
            MapLayer mapLayer = new MapLayer(zLevel);
            Dictionary<Position, TileData> tiles = new Dictionary<Position, TileData>();

            int idCount = 0;
            for (int x = 0; x < map.size.x; x++)
            {
                //yield here
                for (int y = 0; y < map.size.y; y++)
                {
                    Position pos = new Position(x, y, zLevel);

                    TileData tile = CalculateTile(x, y, zLevel, map, offset);
                    tile.id = idCount++;
                    tile.position = pos;
                    idCount++;
                    tiles.Add(pos, tile);
                }
            }
            mapLayer.tiles = tiles;
            map.layers.Add(zLevel, mapLayer);
            foreach (TileData tile in mapLayer.tiles.Values)
            {
                tile.CacheNeighbors(mapLayer);
            }
            UnityMainThreadDispatcher.instance.Enqueue(() => CreateTileMap(mapLayer));
        }

        private static TileData CalculateTile(int x, int y, int z, Map map, Vector2 offset = new Vector2())
        {
            int scale = 2;
            float xCoord = (float)x / map.size.x * scale + offset.x;
            float yCoord = (float)y / map.size.y * scale + offset.y;

            TileData tile = new TileData(TerrainBase.terrainList[0]);
            tile.wetness = CalculateWetness();
            tile.elevation = CalculateElevation();
            if (tile.wetness * (z + 2) * 0.5 < 0.30 && z <= map.zLevels * 0.4) tile.type = TerrainBase.FindTerrainByID("Water");
            else if (tile.wetness * (z - map.zLevels -1) * -1 * 0.25 < 0.30 && z > map.zLevels * 0.8) tile.type = TerrainBase.FindTerrainByID("Magma");

            else if (tile.elevation < z * 0.2) tile.type = TerrainBase.FindTerrainByID("Stone");
            //else tile.type = map.terrains[Mathf.FloorToInt(tile.elevation)];
            else if (z > 0) tile.type = TerrainBase.FindTerrainByID("Dirt");
            else tile.type= TerrainBase.FindTerrainByID("Grass");
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

        public static void CreateTileMap(MapLayer layer) 
        {
            GameObject tileMapObject = new GameObject();
            tileMapObject.transform.SetParent(GameObject.Find("GridManager/Grid").transform);
            tileMapObject.name = layer.ZLevel.ToString();
            tileMapObject.transform.position = new Vector3(0, 0, 0);

            Tilemap tileMap = tileMapObject.AddComponent<Tilemap>();
            foreach (TileData tile in layer.tiles.Values) 
            {
                Tile newTile = AssetManager.allTiles.Where(T => tile.type.IdName == T.name).First();
                tileMap.SetTile(tile.position.ToVector3Int(), newTile);
            }

            layer.tilemap = tileMap;

            tileMapObject.AddComponent<TilemapRenderer>();
            if (layer.ZLevel != 0) tileMapObject.SetActive(false);
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
