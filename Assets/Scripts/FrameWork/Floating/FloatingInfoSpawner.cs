using UnityEngine;

namespace Framework.Floating
{
    /// <summary>
    /// 浮动信息生成器
    /// </summary>
    public class FloatingInfoSpawner : MonoBehaviour
    {
        public Vector2 offset;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
        }

        // public void CreateText(string content, int channel = 0)
        // {
        //     var floatingGo = FloatingInfoFactory.Instance.CreateFloatingText(channel);
        //     floatingGo.Play(content, (Vector2)_rectTransform.position + offset, Vector2.up);
        // }
    }
}