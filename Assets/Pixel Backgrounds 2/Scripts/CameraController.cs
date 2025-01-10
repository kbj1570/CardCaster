
using UnityEngine;

 public class CameraController : MonoBehaviour
    {
        public GameObject camera;
        public float moveRate;

        private Vector3 tmpClickPos;
        private Vector3 tempCameraPos;
        void Start()
        {

        }
        void Update()
        {
            MouseMovement();
        }

        private void MouseMovement()
        {
            if(Input.GetMouseButtonDown(0))
            {
                tmpClickPos = Input.mousePosition;
                tempCameraPos = camera.transform.position;
            }
            else if(Input.GetMouseButton(0))
            {
                Vector3 movePos = Camera.main.ScreenToViewportPoint(tmpClickPos - Input.mousePosition);
                camera.transform.position = tempCameraPos + (movePos * moveRate);

            }
        }
    }

