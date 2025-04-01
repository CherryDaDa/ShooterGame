using System.Collections;
using DG.Tweening;
using Framework.Component;
using TMPro;
using UnityEngine;

namespace Framework.Floating
{
    /// <summary>
    /// 浮动信息实例
    /// </summary>
    public class FloatingInfoInstance : AutoDestroy
    {
        public Color color = Color.white;

        private TextMeshProUGUI _text;
        private RectTransform _rect;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _rect = GetComponent<RectTransform>();
        }

        public void Play(string content, Vector2 uiPosition, Vector2 direction)
        {
            _text.color = color;
            _text.text = content;
            _rect.localPosition = uiPosition + direction * 30;

            // _rect.localScale = Vector3.zero;
            // _rect.DOScale(1.0f, 0.25f).SetEase(Ease.OutBack);
            _rect.DOLocalMoveY(_rect.localPosition.y + direction.y * 20.0f, 0.5f);
        }
    }
}