using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Client
{
    public class Creature
    {
        public Position position;
        public MapTile targetTile;
        public Position TargetPos => targetTile.position;

        public List<MapTile> currentPath = new List<MapTile>();

        public readonly CreatureBase baseType;
        public float speed;
        public float SpeedPerTick => speed / TickManager.TickPerSecondBase * (TickManager.tickPerSecondTarget / TickManager.TickPerSecondBase);

        public string displayName;
        public Creature(CreatureBase data) 
        {
            this.baseType = data;
            speed = data.BaseSpeed;
        }

        public void Tick(float delta) 
        {
            if (currentPath != null && currentPath.Count != 0)
            {
                if (targetTile == null) targetTile = currentPath.First();

                if ((position.gameObjectPosition - TargetPos.gameObjectPosition).sqrMagnitude < 0.01f * 0.01f) 
                {
                    currentPath.RemoveAt(0);
                    position = TargetPos;
                    position.SyncGameObjectToGrid();
                    if (currentPath.Count != 0) targetTile = currentPath.First();
                    else return;
                }

                Vector2 direction = (TargetPos.gameObjectPosition - position.gameObjectPosition).normalized * SpeedPerTick * targetTile.WalkSpeed; //Fix add tile walk speed modifier here later

                float remainingDistance = (TargetPos.gameObjectPosition - position.gameObjectPosition).magnitude;

                if (remainingDistance <= SpeedPerTick )
                {
                    position = TargetPos;
                    position.SyncGameObjectToGrid();
                } 
                else 
                {
                    position.gameObjectPosition += direction;
                }
                Printer.LogWarning($"Current {position}, targetposition{targetTile.position},  GameObject position= {position.gameObjectPosition}, Direction {direction}");
            }
        }

        public void LongTick() 
        {
            if(currentPath == null) currentPath = new List<MapTile>();
            if(currentPath.Count == 0)
            {
                Random rand = new Random();
                int x = rand.Next(0, (int)MainManager.currentMap.size.x - 1);
                int y = rand.Next(0, (int)MainManager.currentMap.size.y - 1);
                int z = rand.Next(0, (int)MainManager.currentMap.zLevels -1);
                currentPath = PathFindManager.PathFindTo(position, new Position(x, y, z));
                Printer.LogError($"{z}");
            }
        }
    }
}
