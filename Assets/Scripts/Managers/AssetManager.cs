using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using Unity.VisualScripting;
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
            BiomeBase.biomeList = LoadAllBiomes();
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

        private static BiomeBase[] LoadAllBiomes()
        {
            TextAsset[] data = Resources.LoadAll<TextAsset>(PathManager.jsonBiome);
            List<BiomeBase> biomeData = new List<BiomeBase>();
            foreach (TextAsset rawJson in data)
            {
                BiomeBase biome = Serializer.SerializeFromString<BiomeBase>(rawJson.text);
                foreach (string stepRaw in biome.GenSteps) {
                    Type type = TypeMapper.GetTypeByName(stepRaw);
                    if (type != null)
                    {
                        object creatureInstance = Activator.CreateInstance(type);
                        GenStep step = creatureInstance as GenStep;
                        biome.gensteps.Add(step);
                    } 
                    else 
                    {
                        Printer.LogError($"Could not find class {stepRaw} while loading {biome.IdName}");
                    }
                }

                biomeData.Add(biome);
            }
            return biomeData.ToArray();
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

    public static class TypeMapper 
    {
        private static Dictionary<string, Type> allTypes = new Dictionary<string, Type>();

        static TypeMapper()
        {
            allTypes = new Dictionary<string, Type>();
            var Types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in Types)
            {
                allTypes[type.Name] = type;
            }
        }

        public static Type GetTypeByName(string typeName)
        {
            if (allTypes.TryGetValue(typeName, out var type))
                return type;
            throw new InvalidOperationException($"Unknown creature type: {typeName}");
        }
    }
}
