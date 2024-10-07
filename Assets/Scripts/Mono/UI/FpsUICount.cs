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
            InvokeRepeating(nameof(GetFPS), 1, 1);
        }

        // Update is called once per frame
        void GetFPS()
        {
            textComp.text = ((int)1 / Time.unscaledDeltaTime).ToString();
        }
    }
}