using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Input
{
    /// <summary>
    /// 设备摄像头图像
    /// </summary>
    public class DeviceCameraTexture : MonoBehaviour
    {
        [Tooltip("摄像机渲染器")]
        public RawImage m_CameraRenderer;

        [Tooltip("启动时自动开启设备摄像头")]
        public bool m_PlayOnAwake = true;

        [Tooltip("开启镜像")]
        public bool m_EnableMirroring;

        private WebCamTexture m_WebCameraTexture;
        private Vector3 m_CameraRendererScale;

        private void Awake()
        {
            m_CameraRendererScale = m_CameraRenderer.transform.localScale;
        }

        void Start()
        {
            if (WebCamTexture.devices.Length > 0)
            {
                WebCamDevice device = WebCamTexture.devices[0];
                m_WebCameraTexture = new WebCamTexture(device.name);
                m_CameraRenderer.texture = m_WebCameraTexture;

                if (m_PlayOnAwake)
                    Play();
            }
        }

        private void Update()
        {
            m_CameraRenderer.transform.localScale = new Vector3(
                m_EnableMirroring ? -m_CameraRendererScale.x : m_CameraRendererScale.x,
                m_CameraRendererScale.y,
                m_CameraRendererScale.z);
        }

        /// <summary>
        /// 启用摄像头图像
        /// </summary>
        public void Play()
        {
            m_WebCameraTexture.Play();
        }

        /// <summary>
        /// 关闭摄像头图像
        /// </summary>
        public void Stop()
        {
            m_WebCameraTexture.Stop();
        }
    }
}

