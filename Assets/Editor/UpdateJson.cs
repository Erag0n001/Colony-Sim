using Client;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace Editor {
    public class CustomTools : EditorWindow
    {
        public static readonly string pathResources = "Assets/Resources";
        [MenuItem("File/UpdateJson")]
        public static void BuildStuff()
        {
            UpdateTerrain();
            UpdateCreature();
            UpdateBuildings();
        }
        public static void UpdateTerrain()
        {
            TextAsset[] jsonRaw = Resources.LoadAll<TextAsset>(PathManager.jsonTerrain);
            foreach (TextAsset json in jsonRaw)
            {
                TerrainBase terrain = Serializer.SerializeFromString<TerrainBase>(json.text);
                Serializer.SerializeToFile(Path.Combine(pathResources, PathManager.jsonTerrain, terrain.IdName + ".json"), terrain);
            }
        }
        public static void UpdateCreature()
        {
            TextAsset[] jsonRaw = Resources.LoadAll<TextAsset>(PathManager.jsonCreatures);
            foreach (TextAsset json in jsonRaw)
            {
                CreatureBase creature = Serializer.SerializeFromString<CreatureBase>(json.text);
                Serializer.SerializeToFile(Path.Combine(pathResources, PathManager.jsonCreatures, creature.IdName + ".json"), creature);
            }
        }
        public static void UpdateBuildings()
        {
            TextAsset[] jsonRaw = Resources.LoadAll<TextAsset>(PathManager.jsonBuilding);
            foreach (TextAsset json in jsonRaw)
            {
                BuildingBase building = Serializer.SerializeFromString<BuildingBase>(json.text);
                Serializer.SerializeToFile(Path.Combine(pathResources, PathManager.jsonBuilding, building.IdName + ".json"), building);
            }
        }
    }
}