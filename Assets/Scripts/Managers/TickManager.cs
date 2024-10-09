using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Client 
{
    public static class TickManager 
    {

        public static int tickPerSecondTarget = 1000;
        public static int tickPerSecond = 0;
        private static bool paused;

        public static bool Paused 
        {
            get { return paused; }
            set { 
                paused = value; 
                if(value == false) 
                {
                    Tick();
                }
            }
        }
        public static float TimePerTick
        {
            get
            {
                return 1000f / tickPerSecondTarget;
            }
        }

        public static void Tick()
        {
            int tickCount = 0;
            int tickElapsed = 0;
            float msElapsed = 0;
            float msElapsedSecond = 0;
            float delta = 0;
            Stopwatch clock = new Stopwatch();
            clock.Start();
            float nextTickTime = clock.ElapsedMilliseconds + TimePerTick;
            while (!paused)
            {
                try
                {
                    delta = (float)clock.Elapsed.TotalMilliseconds;
                    msElapsed += delta;
                    msElapsedSecond += delta;

                    clock.Restart();

                    if (msElapsed >= nextTickTime)
                    {
                        foreach (MapLayer layer in MainManager.currentMap.layers.Values)
                        {
                            foreach (Creature creature in layer.creatures.Values)
                            {
                                try { creature.Tick(delta); }
                                catch (Exception e)
                                {
                                    Printer.LogError($"Error ticking {creature.baseType.IdName} with name {creature.displayName}\n{e}");
                                }
                            }
                        }
                        if (tickCount == 50)
                        {
                            LongTick();
                            tickCount = 0;
                        }

                        msElapsed -= TimePerTick;
                        tickElapsed++;
                        tickCount++;
                    }

                    if (msElapsedSecond >= 1000)
                    {
                        tickPerSecond = tickElapsed;
                        tickElapsed = 0;
                        msElapsedSecond = 0;
                    }

                    float timeUntilNextTick = TimePerTick - msElapsed;

                    if (timeUntilNextTick > 0)
                    {
                        Thread.Sleep((int)timeUntilNextTick);
                    }
                }
                catch (Exception e)
                {
                    Printer.LogError(e.ToString());
                }
            }
        }

        public static void LongTick()
        {
        }
    }
}