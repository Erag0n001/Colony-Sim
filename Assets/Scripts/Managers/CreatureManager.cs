using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    public static class CreatureManager
    {
        public static GameObject creatureObject;
        public static void SpawnNewCreature() 
        {
            Map map = MainManager.currentMap;
            Creature creatureData = new Creature(CreatureBase.creatureList.First());
            creatureData.position = new Position(0,0);
            creatureData.targetPos = new Position(0, 0);
            GameObject gameobject = GameObject.Instantiate(creatureObject);
            gameobject.GetComponent<CreatureMono>().creature = creatureData;
            gameobject.GetComponent<CreatureMono>().currentMap = map;
            gameobject.transform.position = creatureData.position.ToVector3Int() + new Vector3(0,0,-1);
            gameobject.transform.rotation = new Quaternion(-90,0,0,90);
            gameobject.transform.SetParent(GameObject.Find("CreatureManager").transform);
        }
    }
}
