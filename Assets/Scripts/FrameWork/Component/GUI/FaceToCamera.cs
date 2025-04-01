using UnityEngine;

namespace Framework.Component.GUI
{
    /// <summary>
    /// 使3D空间中的物体始终面对摄像机的组件
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class FaceToCamera : MonoBehaviour
    {
        public Camera mainCamera;

        private void Start()
        {
            if (!mainCamera)
            {
                mainCamera = Camera.main;
            }

            if (!mainCamera)
            {
                Debug.LogError("No camera found. Please assign a camera to the Billboard component.");
            }
        }

        private void LateUpdate()
        {
            if (!mainCamera)
            {
                mainCamera = Camera.main;
            }
            if (mainCamera)
            {
                transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                    mainCamera.transform.rotation * Vector3.up);
            }
        }
    }
}