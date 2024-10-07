using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
namespace Client
{
    public class CreatureMono : MonoBehaviour
    {
        public Creature creature;

        public Map currentMap;

        public float count;

        public Vector3Int RealPosition => creature.position.ToVector3Int();

        public Vector3Int TargetPosition => creature.targetPos.ToVector3Int();

        public bool isMoving;

        public void Update()
        {
            if(RealPosition != TargetPosition && isMoving == false) 
            {
                StartCoroutine(Lerp());
            }
        }

        public IEnumerator Lerp() 
        {
            Vector3 startRot = RealPosition;

            float currentTime = 0f;
            float endTime = 0.250f;
            while (currentTime < endTime)
            {
                currentTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startRot, TargetPosition, currentTime / endTime);
                yield return null;
            }
        }
    }
}