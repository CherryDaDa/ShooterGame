using System;
using System.Collections;
using Framework.Tools;
using TMPro;
using UnityEngine;

namespace Framework.Component.GUI
{
    public class WebGLInputRegex : MonoBehaviour
    {
        public string regex;

        private TMP_InputField _input;

        private void Awake()
        {
            _input = GetComponent<TMP_InputField>();
        }

        private void Start()
        {
            _input.onValueChanged.AddListener(OnValueChangedHandler);
        }

        private void OnValueChangedHandler(string str)
        {
            var finalStr = RegexUtility.GetValidContent(str, $@"{regex}");
            // _input.text = finalStr;
            StartCoroutine(OnInput(finalStr)); //跟微软输入法有冲突 不能直接赋值 下一帧更新
        }

        private IEnumerator OnInput(string text)
        {
            yield return null;
            _input.text = text;
            // 强制Unity在下一帧更新文本
            StopAllCoroutines();
        }
    }
}