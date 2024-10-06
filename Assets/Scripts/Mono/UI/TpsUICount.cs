using Client;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Client
{
    public class TpsUICount : MonoBehaviour
    {
        public TextMeshProUGUI textComp;
        void Start()
        {
            textComp = gameObject.GetComponent<TextMeshProUGUI>();
            InvokeRepeating(nameof(GetTPS), 1, 1);
        }


        void GetTPS()
        {
            textComp.text = TickManager.tickPerSecond.ToString() + "/" + TickManager.tickPerSecondTarget;
        }
    }
}