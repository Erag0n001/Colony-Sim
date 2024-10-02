using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;
namespace Client
{
    public partial class GridManager : MonoBehaviour
    {
        public static GridManager instance;

        public static GameObject tileObject;

        public Tile[] tileSetTiles;

        public void Awake()
        {
            AssetManager.LoadAllAssets();
        }
        public void Start()
        {
            GridManager.instance = this;

            GenerateGrid();
        }

        public void GenerateGrid(int width = 250, int height = 250, string seed = "")
        {
            Random rand = new Random();
            if (seed == "") seed = rand.Next(0,9999999).ToString();
            Printer.Log(seed);
            Map map = new Map(seed, new Vector2(width, height));
            map.terrains = new List<TerrainBase>() { TerrainBase.FindTerrainByID("Grass"), TerrainBase.FindTerrainByID("Stone") };
            Dictionary<Vector3Int, TileData> tiles = new Dictionary<Vector3Int, TileData>();

            int idCount = 0;
            Vector2 offset = new Vector2(rand.Next(-999999, 999999), rand.Next(-999999, 999999));

            Tilemap tileMap = CreateTileMap();
            tileSetTiles = GetTiles(map);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);

                    TileData tile = CalculateTile(x, y, map, offset);
                    tile.id = idCount++;
                    tile.position = pos;
                    Tile newTile = AssetManager.allTiles.Where(T =>tile.type.IdName == T.name).First();
                    idCount++;
                    tiles.Add(pos, tile);
                    tileMap.SetTile(pos,newTile);
                }
            }
            MainManager.currentMap = map;
            map.tiles = tiles;
            map.tilemap = tileMap;
            foreach (TileData tile in tiles.Values)
            {
                tile.CacheNeighbors();
            }
            CreatureManager.instance.SpawnNewCreature();
        }

        private static TileData CalculateTile(int x, int y, Map map, Vector2 offset = new Vector2())
        {
            int scale = 5;
            float xCoord = (float)x / map.size.x * scale + offset.x;
            float yCoord = (float)y / map.size.y * scale + offset.y;

            TileData tile = new TileData(TerrainBase.terrainList[0]);
            tile.wetness = CalculateWetness();
            tile.elevation = CalculateElevation();
            if (tile.wetness < 0.30) tile.type = TerrainBase.FindTerrainByID("Water");
            else tile.type = map.terrains[Mathf.FloorToInt(tile.elevation)];
            return tile;

            float CalculateWetness()
            {
                float wetness = Mathf.Clamp(Mathf.PerlinNoise(xCoord, yCoord),0, 0.999999f);
                return wetness;
            }
            float CalculateElevation()
            {
                float elevation = Mathf.Clamp(Mathf.PerlinNoise(xCoord, yCoord),0, 0.999999f) * map.terrains.Count();
                return elevation;
            }
        }

        public Tilemap CreateTileMap() 
        {
            GameObject tileMapObject = new GameObject();
            tileMapObject.transform.SetParent(this.transform.GetChild(0));
            tileMapObject.transform.position = new Vector3(0, 0, 0);
            Tilemap tilemap = tileMapObject.AddComponent<Tilemap>();
            tileMapObject.AddComponent<TilemapRenderer>();
            return tilemap;
        }

        public Tile[] GetTiles(Map map) 
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
