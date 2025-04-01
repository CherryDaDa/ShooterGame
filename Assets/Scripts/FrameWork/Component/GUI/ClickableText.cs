using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Framework.Component.GUI
{
    /// <summary>
    /// 可点击的文本（需要使用富文本，例：<link="click">点击文本</link>）
    /// </summary>
    public class ClickableText : MonoBehaviour, IPointerClickHandler
    {
        public event Action<string> OnClickLinkEvent;
        
        private TextMeshProUGUI _textMeshProUGUI;

        private void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMeshProUGUI, UnityEngine.Input.mousePosition, null);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = _textMeshProUGUI.textInfo.linkInfo[linkIndex];
                var linkStr = linkInfo.GetLinkText();
                //Debug.Log("当前的文本是：" + linkStr);

                OnClickLinkEvent?.Invoke(linkStr);
            }
        }
    }
}