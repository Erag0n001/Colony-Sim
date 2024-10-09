using Client;
using UnityEditor;
using UnityEngine;
namespace Editor
{
    public class UnityEditorAssetCreator
    {
        //Unity editor Asset Creator
        [MenuItem("Assets/Create/JsonData/Creature")]
        private static void CreateNewCreatureData()
        {
            ProjectWindowUtil.CreateAssetWithContent(
                "CreatureData.json",
                JsonUtility.ToJson(new CreatureBase(), true));
        }
        [MenuItem("Assets/Create/JsonData/Terrain")]
        private static void CreateNewTerrainData()
        {
            ProjectWindowUtil.CreateAssetWithContent(
                "TerrainData.json",
                JsonUtility.ToJson(new TerrainBase(), true));
        }
        [MenuItem("Assets/Create/JsonData/Building")]
        private static void CreateNewBuildingData()
        {
            ProjectWindowUtil.CreateAssetWithContent(
                "TerrainData.json",
                JsonUtility.ToJson(new BuildingBase(), true));
        }
        [MenuItem("Assets/Create/JsonData/Biome")]
        private static void CreateNewBiomeData()
        {
            ProjectWindowUtil.CreateAssetWithContent(
                "BiomeData.json",
                JsonUtility.ToJson(new BiomeBase(), true));
        }
    }
}