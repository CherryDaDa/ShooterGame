using System;
using UnityEngine;

namespace Framework.Component
{
    public class AutoDestroy : MonoBehaviour
    {
        [Min(0.1f)] public float duration;

        private float _startTime;

        private void Start()
        {
            _startTime = Time.time;
        }

        private void Update()
        {
            if (Time.time - _startTime > duration)
            {
                Destroy(gameObject);
            }
        }
    }
}