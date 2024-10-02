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

        public Vector3Int realPosition => creature.position;

        public Vector3Int targetPosition => creature.targetPos;

        public bool isMoving;

        public void Update()
        {
            if(realPosition != targetPosition && isMoving == false) 
            {
                StartCoroutine(Lerp());
            }
        }

        public IEnumerator Lerp() 
        {
            Vector3 startRot = realPosition;

            float currentTime = 0f;
            float endTime = 0.250f;
            while (currentTime < endTime)
            {
                currentTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startRot, targetPosition, currentTime / endTime);
                yield return null;
            }
        }
    }
}