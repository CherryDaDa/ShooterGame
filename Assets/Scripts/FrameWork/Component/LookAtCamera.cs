using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Component
{
    /// <summary>
    /// 面向摄像机方向
    /// </summary>
    public class LookAtCamera : MonoBehaviour
    {
        private Transform m_MainCamera;

        [Tooltip("取反")]
        public bool m_Invert;

        void Start()
        {
            m_MainCamera = Camera.main.transform;
        }

        private void Update()
        {
            if (!m_MainCamera) return;
            transform.forward = m_Invert ? m_MainCamera.forward : -m_MainCamera.forward;
        }

        private void LateUpdate()
        {
            
        }
    }
}

