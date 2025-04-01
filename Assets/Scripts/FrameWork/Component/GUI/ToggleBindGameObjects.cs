using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Component.GUI
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleBindGameObjects : MonoBehaviour
    {
        public bool isFollowEnable = true;
        public GameObject[] objs;
        
        
        private Toggle _toggle;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            
        }

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(OnToggleValueChangedHandler);
            if (isFollowEnable)
            {
                Refresh(_toggle.isOn);
            }
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(OnToggleValueChangedHandler);
            if (isFollowEnable)
            {
                Refresh(false);
            }
        }

        private void OnToggleValueChangedHandler(bool isOn)
        {
            Refresh(isOn);
        }

        private void Refresh(bool state)
        {
            foreach (var o in objs)
            {
                o.SetActive(state);
            }
        }
    }
}