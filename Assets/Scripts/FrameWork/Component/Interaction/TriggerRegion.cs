using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Component.Interaction
{
    /// <summary>
    /// 触发区域
    /// </summary>
    // [ExecuteInEditMode]
    public class TriggerRegion : MonoBehaviour
    {
        public LayerMask targetLayer;

        public Action<Transform> OnTriggerEnterEvent;
        public Action<Transform> OnTriggerExitEvent;

        protected Collider _collider;
        public List<Collider> TriggerColliders { get; private set; }

        protected virtual void Awake()
        {
            gameObject.layer = 2;

            _collider = GetComponentInChildren<Collider>();
            if (_collider)
            {
                _collider.isTrigger = true;
            }

            TriggerColliders = new List<Collider>();

            // _collider.enabled = false;
        }

        private void Start()
        {
            // StartCoroutine(Init());
        }

        // private void OnDisable()
        // {
        //     while (TriggerColliders.Count > 0)
        //     {
        //         OnTriggerExit(TriggerColliders[0]);
        //     }
        // }

        // IEnumerator Init()
        // {
        //     // while (!SceneController.Instance.IsInitialized)
        //     // {
        //     yield return null;
        //     // }
        //     _collider.enabled = true;
        // }

        protected bool IsTriggerTarget(Collider other)
        {
            return !((targetLayer.value & 1 << other.gameObject.layer) <= 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsTriggerTarget(other)) return;
            TriggerColliders.Add(other);
            // Debug.Log($"进入触发区域:{gameObject.name}");
            OnTargetLayerTriggerEnter(other);
            OnTriggerEnterEvent?.Invoke(other.transform);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!IsTriggerTarget(other)) return;
            // Debug.Log("停留触发区域");
            OnTargetLayerTriggerStay(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsTriggerTarget(other)) return;
            TriggerColliders.Remove(other);
            // Debug.Log($"离开触发区域:{gameObject.name}");
            OnTargetLayerTriggerExit(other);
            OnTriggerExitEvent?.Invoke(other.transform);
        }

        // private void OnDestroy()
        // {
        //     foreach (var other in TriggerColliders)
        //     {
        //         OnTargetLayerTriggerExit(other);
        //         OnTriggerExitEvent?.Invoke(other.transform);
        //     }
        //     TriggerColliders.Clear();
        // }

        protected virtual void OnTargetLayerTriggerEnter(Collider other)
        {
            
        }

        protected virtual void OnTargetLayerTriggerStay(Collider other)
        {

        }

        protected virtual void OnTargetLayerTriggerExit(Collider other)
        {
            
        }
    }
}

