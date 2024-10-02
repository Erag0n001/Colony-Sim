using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    public class CreatureManager : MonoBehaviour
    {
        public static CreatureManager instance;

        public static GameObject creatureObject;
        public void Start()
        {
            CreatureManager.instance = this;
        }
        public void SpawnNewCreature() 
        {
            Map map = MainManager.currentMap;
            Creature creatureData = new Creature(CreatureBase.creatureList.First());
            creatureData.position = new Vector3Int(0,0);
            creatureData.targetPos = new Vector3Int(0, 0);
            GameObject gameobject = GameObject.Instantiate(creatureObject);
            gameobject.GetComponent<CreatureMono>().creature = creatureData;
            gameobject.GetComponent<CreatureMono>().currentMap = map;
            gameobject.transform.position = creatureData.position + new Vector3(0,0,-1);
            gameobject.transform.rotation = new Quaternion(-90,0,0,90);
            gameobject.transform.SetParent(this.transform);
        }

        public void TickAllCreatures()
        {
            foreach(Creature creature in MainManager.currentMap.creatures.Values)
            {
                try { creature.Tick(); } catch(Exception e) { Printer.LogError($"Error ticking {creature.type.IdName} with name {creature.displayName}\n{e}"); }
            }
        }
    }
}
