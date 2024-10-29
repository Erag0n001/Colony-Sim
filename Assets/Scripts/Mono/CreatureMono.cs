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

        public Vector3 RealPosition => creature.position.gameObjectPosition;

        public int layer => creature.position.z;

        private MeshRenderer renderer;
        private void Start()
        {
            renderer = gameObject.GetComponent<MeshRenderer>();
        }
        public void Update()
        {
            if (transform.position != RealPosition)
            {
                gameObject.transform.position = RealPosition;
            }
            //if (MainManager.currentMap.ActiveLayer != layer)
            //{
            //    renderer.enabled = false;
            //} else 
            //{
            //    renderer.enabled = true;
            //}
        }
    }
}