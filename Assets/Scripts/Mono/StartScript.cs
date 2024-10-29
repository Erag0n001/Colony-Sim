using UnityEngine;

namespace Client 
{
    public class Startup : MonoBehaviour 
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            MainManager.Startup();
        }

        private void OnApplicationQuit()
        {
            TickManager.Paused = true;
        }
    }
}