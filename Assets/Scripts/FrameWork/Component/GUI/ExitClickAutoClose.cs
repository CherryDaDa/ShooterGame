using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Component.GUI
{
    /// <summary>
    /// 点击其他区域关闭当前界面
    /// </summary>
    public class ExitClickAutoClose : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool onCloseDestroy = true;
        
        private bool _isEnterSelf;
        
        public void OnPointerExit(PointerEventData eventData)
        {
            _isEnterSelf = false;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isEnterSelf = true;
        }

        private void Update()
        {
            if (!_isEnterSelf && UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (onCloseDestroy)
                {
                    Destroy(gameObject);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}