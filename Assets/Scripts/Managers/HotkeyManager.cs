using Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Client {
    public class HotkeyManager : MonoBehaviour
    {
        public static HotkeyManager instance;
        public KeyCode[] keys;
        private void Awake()
        {

        }
        void Start()
        {
            HotkeyManager.instance = this;
            keys = new KeyCode[] {KeyCode.LeftShift};
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (Input.GetKeyDown(keys[i]))
                {
                    switch (keys[i])
                    {
                        case KeyCode.LeftShift:
                           CameraManager.instance.sensitivity = 40;
                            break;
                    }
                }
                if (Input.GetKeyDown(KeyCode.Escape)) 
                {
                    Printer.LogWarning("pausing");
                    TickManager.Paused = true;
                }
            }
            for (int i = 0; i < keys.Length; i++)
            {
                if (Input.GetKeyUp(keys[i]))
                {
                    switch (keys[i])
                    {
                        case KeyCode.LeftShift:
                            CameraManager.instance.sensitivity = 20;
                            break;
                    }
                }
            }
        }
    }
}