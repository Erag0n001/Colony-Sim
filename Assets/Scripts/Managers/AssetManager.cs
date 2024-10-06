using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Client 
{
    public static class AssetManager
    {
        public static List<Tile> allTiles = new List<Tile>();

        public static Texture2D textureAtlas;
        public static void LoadAllAssets ()
        {
            GridManager.tileObject = Resources.Load<GameObject>("Tile");
            CreatureManager.creatureObject = Resources.Load<GameObject>("Creature");
            TerrainBase.terrainList = LoadAllTerrain();
            CreatureBase.creatureList = LoadAllCreatures();
        }

        private static TerrainBase[] LoadAllTerrain()
        {
            TextAsset[] data = Resources.LoadAll<TextAsset>(PathManager.jsonTerrain);
            List<TerrainBase> terrainData = new List<TerrainBase>();
            foreach (TextAsset rawJson in data)
            {
                TerrainBase terrain = Serializer.SerializeFromString<TerrainBase>(rawJson.text);
                Texture2D texture = Resources.Load<Texture2D>(terrain.TexturePath);
                if (texture != null)
                    terrain.texture = CreateSpriteFromTexture(texture);
                else
                    Printer.LogError($"Could not find texture for {terrain.IdName} at path {terrain.TexturePath}");

                terrainData.Add(terrain);
                AddTerrainToList(terrain);
            }
            return terrainData.ToArray();
        }

        private static CreatureBase[] LoadAllCreatures()
        {
            TextAsset[] data = Resources.LoadAll<TextAsset>(PathManager.jsonCreatures);
            List<CreatureBase> terrainData = new List<CreatureBase>();
            foreach (TextAsset rawJson in data)
            {
                CreatureBase creature = Serializer.SerializeFromString<CreatureBase>(rawJson.text);
                Texture2D texture = Resources.Load<Texture2D>(creature.TexturePath);
                if (texture != null)
                    creature.texture = CreateSpriteFromTexture(texture);
                else
                    Printer.LogError($"Could not find texture for {creature.IdName} at path {creature.TexturePath}");

                terrainData.Add(creature);
            }
            return terrainData.ToArray();
        }
        public static Sprite CreateSpriteFromTexture(Texture2D texture)
        {
            Sprite result = Sprite.Create(texture,
                                 new Rect(0, 0, texture.width, texture.height),
                                 new Vector2(0.5f, 0.5f),32);
            return result;
        }
        public static void AddTerrainToList(TerrainBase terrain) 
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = terrain.texture;
            tile.name = terrain.IdName;
            allTiles.Add(tile);
        }
    }
}
