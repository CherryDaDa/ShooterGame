using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Framework.Component
{
    public class AutoRotate : MonoBehaviour
    {
        [Header("Rotation Speeds")]
        public float rotationSpeedX = 10f;  // X轴的旋转速度
        public float rotationSpeedY = 10f;  // Y轴的旋转速度
        public float rotationSpeedZ = 10f;  // Z轴的旋转速度

        public bool playOnAwake = true;
        public bool isRandom;
        public bool isOnlyOne;

        private bool _isPlaying; 

        private Vector3 _oldValues;

        public void Play()
        {
            _isPlaying = true;
        }

        public void Stop()
        {
            _isPlaying = false;
        }

        private void Awake()
        {
            _oldValues = new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ);
            _isPlaying = playOnAwake;
        }

        private void OnEnable()
        {
            if (isRandom)
            {
                rotationSpeedX = Random.Range(-_oldValues.x, _oldValues.x);
                rotationSpeedY = Random.Range(-_oldValues.y, _oldValues.y);
                rotationSpeedZ = Random.Range(-_oldValues.z, _oldValues.z);
            }
        }

        private void Update()
        {
            if (!_isPlaying) return;
            
            // 计算每一帧的旋转量
            float rotationX = rotationSpeedX * Time.deltaTime;
            float rotationY = rotationSpeedY * Time.deltaTime;
            float rotationZ = rotationSpeedZ * Time.deltaTime;

            // 将旋转应用到物体上
            transform.Rotate(rotationX, rotationY, rotationZ);
        }

        private void OnDrawGizmosSelected()
        {
            // 计算每一帧的旋转量
            float rotationX = rotationSpeedX * Time.deltaTime;
            float rotationY = rotationSpeedY * Time.deltaTime;
            float rotationZ = rotationSpeedZ * Time.deltaTime;

            // 将旋转应用到物体上
            transform.Rotate(rotationX, rotationY, rotationZ);
        }
    }
}