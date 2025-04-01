using System;
using System.Collections.Generic;
using Framework.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.Component.GUI
{
    /// <summary>
    /// 按Tab键自动切换具有焦点的组件
    /// </summary>
    public class AutoTabCutSelectable : MonoBehaviour
    {
        public bool autoCheck;
        
        [IsShow("autoCheck == false")]
        public Selectable[] selectableObjs;
        
        private EventSystem _system;
        private int _index;

        private void Awake()
        {
            _system = EventSystem.current;
            if (autoCheck)
            {
                selectableObjs = GetComponentsInChildren<Selectable>();
            }
        }

        private void Update()
        {
            if(selectableObjs.Length == 0) return;
            if (UnityEngine.Input.GetKeyDown(KeyCode.Tab))
            {
                CutNextGameObject();
            }
        }

        private void CutNextGameObject()
        {
            if (_index < 0)
            {
                _index = Math.Max(Array.IndexOf(selectableObjs, _system.currentSelectedGameObject), 0);
            }
            _system.SetSelectedGameObject(selectableObjs[_index].gameObject);

            //如果对象被隐藏，则寻找一下个非隐藏的对象
            while (true)
            {
                _index = (_index+1) % selectableObjs.Length;
                if (selectableObjs[_index].gameObject.activeInHierarchy)
                {
                    break;
                }
            }
        }
    }
}