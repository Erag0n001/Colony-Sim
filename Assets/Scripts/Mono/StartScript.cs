using UnityEngine;

namespace Client 
{
    public class Startup : MonoBehaviour 
    {
        private void Awake()
        {
            MainManager.Startup();
        }

        private void OnApplicationQuit()
        {
            TickManager.Paused = true;
        }
    }
}