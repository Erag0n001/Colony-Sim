using Client;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Client
{
    public class FpsUICount : MonoBehaviour
    {
        public TextMeshProUGUI textComp;
        void Start()
        {
            textComp = gameObject.GetComponent<TextMeshProUGUI>();
            //InvokeRepeating()
        }

        // Update is called once per frame
        void Update()
        {
            textComp.text = TickManager.tickPerSecond.ToString() + "/" + TickManager.tickPerSecondTarget;
        }
    }
}