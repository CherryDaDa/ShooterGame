using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework.Component.GUI
{
    /// <summary>
    /// UI跟随3D空间中的点
    /// </summary>
    public class UIFollow3DPoint : MonoBehaviour
    {
        public Camera targetCamera;
        public Vector3 worldPoint;
        public Vector2 screenOffset;

        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>() ?? transform;
            if (!targetCamera)
            {
                targetCamera = Camera.main;
            }
        }

        private void Update()
        {
            var screenPoint = targetCamera.WorldToScreenPoint(worldPoint);
            _transform.position = screenPoint + (Vector3)screenOffset;
        }
    }
}