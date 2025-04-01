using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Framework.Component
{
    public class TextMeshProGUIWriterEffect : MonoBehaviour
    {
        public float duration = 0.5f;
        public float interval = 0.1f;
        
        private TextMeshProUGUI _textComponent; // TextMeshPro组件

        private string _fullText; // 完整的文本
        private string _currentText; // 当前显示的文本

        private void Awake()
        {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            
        }

        private void OnEnable()
        {
            if (_textComponent != null)
            {
                _fullText = _textComponent.text;
                _textComponent.text = "";
                StartCoroutine(TypeText());
            }
        }

        private IEnumerator TypeText()
        {
            foreach (char letter in _fullText)
            {
                _currentText += letter;
                _textComponent.text = _currentText;
                yield return new WaitForSeconds(interval);
            }
        }

        protected virtual void CharEffect()
        {
            
        }
    }
}