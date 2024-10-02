using Client;
using UnityEditor;
using UnityEngine;
namespace Editor
{
    public class CreateTools
    {
        class UnityEditorAssetCreator
        {
            //Unity editor Asset Creator
            [MenuItem("Assets/Create/JsonData/Creature")]
            private static void CreateNewItem()
            {
                ProjectWindowUtil.CreateAssetWithContent(
                    "Item.json",
                    JsonUtility.ToJson(new CreatureBase(), true));
            }
            [MenuItem("Assets/Create/JsonData/Terrain")]
            private static void CreateNewRangedWeapon()
            {
                ProjectWindowUtil.CreateAssetWithContent(
                    "RangedWeapon.json",
                    JsonUtility.ToJson(new TerrainData(), true));
            }
        }
    }
}