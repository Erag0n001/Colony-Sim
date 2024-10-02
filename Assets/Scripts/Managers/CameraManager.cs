using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Client
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager instance;
        [SerializeField] private GameObject player;
        private Vector3 movement;
        public bool isLocked;
        public float sensitivity;

        void Start()
        {
            CameraManager.instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            CameraMovement();
        }

        public void CameraMovement()
        {
            if(isLocked) return;
            float y = Input.mouseScrollDelta.y * 5;
            if (player.transform.position.y + y <= -20) y = 0;
            if (player.transform.position.y + y >= 50) y = 0;
            movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Camera mainCamera = player.GetComponent<Camera>();
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                mainCamera.orthographicSize -= 1f;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                mainCamera.orthographicSize += 1f;
            }


            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 1f, 100f);
            player.transform.Translate(movement * sensitivity *  Time.deltaTime);
        }
    }
}