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
            map.biome = Biome.biomeList[0];
            map.terrains = new List<TerrainBase>() { TerrainBase.FindTerrainByID("Grass")};

            map.tileSetTiles = GetTiles(map);
            MainManager.currentMap = map;
            map.zLevels = zLevel;

            Vector2 offset = new Vector2(rand.Next(-999999, 999999), rand.Next(-999999, 999999));
            map.offset = offset;
            map.scale = 2;
            for (int z = 0; z < zLevel; z++)
            {
                int currentZLevel = z;
                GenerateGrid(map, currentZLevel);
            }
            List<Task> tasks = new List<Task>();
            foreach (GenStep step in map.biome.gensteps)
            {
                step.Generate(map);
            }

            await Task.WhenAll(tasks);

            map.layers = map.layers.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            CreatureManager.SpawnNewCreature();
            }
        public static void GenerateGrid(Map map, int zLevel)
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

                    TileData tile = new TileData(TerrainBase.terrainList[0]);
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
