using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Component.GUI
{
    public class Progress : MonoBehaviour
    {
        [SerializeField] private Slider bar;
        [SerializeField] private TextMeshProUGUI progressText;

        public bool autoDisplayState = true;
        public float duration;

        private Coroutine _delayHideCoroutine;

        public float GetProgress()
        {
            return bar.value;
        }

        public void SetProgress(float progress)
        {
            bar.value = Mathf.Clamp01(progress);
            progressText.text = $"{progress:P0}";
            if (autoDisplayState)
            {
                gameObject.SetActive(progress < 1);
            }
        }
        
        public void SetProgress(float current, float total)
        {
            SetProgress(current / total);
            progressText.text = $"{current}/{total}";
        }

        public void SetText(string str)
        {
            progressText.text = str;
        }

        private void OnEnable()
        {
            if (duration > 0)
            {
                if (_delayHideCoroutine != null)
                {
                    StopCoroutine(_delayHideCoroutine);
                }
                _delayHideCoroutine = StartCoroutine(DelayAutoHide());
            }
        }

        private void OnDisable()
        {
            if (_delayHideCoroutine != null)
            {
                StopCoroutine(_delayHideCoroutine);
                _delayHideCoroutine = null;
            }
        }

        private IEnumerator DelayAutoHide()
        {
            yield return new WaitForSeconds(duration);
            gameObject.SetActive(false);
            _delayHideCoroutine = null;
        }
    }
}