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

        public static int framePerSecondTarget = 10000;

        public static void Startup() 
        {
            Application.targetFrameRate = framePerSecondTarget;
            AssetManager.LoadAllAssets();
            GridManager.GenerateMap();
            Task.Run(() =>
            {
                TickManager.Paused = false;
            });
            }
    }
}
