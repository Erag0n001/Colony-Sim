using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    public static class MainManager
    {
        public static Map currentMap;
        public static List<GameObject> currentPackedScene = new List<GameObject>();

        public static float deltaTime = 0f;

        private static int tickCount = 0;
        public static GameObject FindPackedSceneByName(string name) 
        {
            return currentPackedScene.Where( S => S.name == name).FirstOrDefault();
        }

        public static void Tick() 
        {
            while (true)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                try { CreatureManager.instance.TickAllCreatures(); } catch (Exception e) { Printer.LogError($"Error ticking creatures: {e}"); }
                stopwatch.Stop();
                deltaTime = stopwatch.ElapsedMilliseconds;

                if (tickCount == 50)
                {
                    LongTick();
                    tickCount = 0;
                }
                tickCount++;
            }
        }

        public static void LongTick() 
        {
        }
    }
}
