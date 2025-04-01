using UnityEngine;
using UnityEngine.UI;

namespace Framework.Component.GUI
{
    /// <summary>
    /// 弧形进度条
    /// </summary>
    [ExecuteInEditMode]
    public class ArcSlider : MonoBehaviour
    {
        // public float minValue = 0f;
        // public float maxValue = 1f;
        public float minAngle = 0;
        public float maxAngle = 90.0f;
        [Range(0,1)] public float value;
        public float radius = 10.0f;

        public Image sliderFillImage;
        public Transform handle;

        private void Update()
        {
            // 获取滑动条值
            float sliderValue = Mathf.Lerp(0.25f, 0.5f, value);

            // 更新UI显示
            UpdateSliderUI(sliderValue);
            GetCurrentAngle(value);
        }

        private void GetCurrentAngle(float valueTest)
        {
            // 在这里根据输入值进行插值，计算当前角度（弧度）
            float normalizedValue = Mathf.Clamp01((UnityEngine.Input.GetAxis("Horizontal") + 1f) / 2f); // 用于测试的示例输入
          //  float currentAngle = Mathf.Lerp(minAngle, maxAngle, normalizedValue);
            float currentAngle = Mathf.Lerp(minAngle, maxAngle, valueTest);

            // 更新UI的角度和位置
            UpdateHandlePosition(currentAngle);

          //  return currentAngle * Mathf.Deg2Rad;
        }

        private void UpdateHandlePosition(float angle)
        {
            // 根据角度计算位置
            // float radius = 100f; // 设置滑动条的半径
            var angleRadians = angle * Mathf.Deg2Rad;
            var x = radius * Mathf.Cos(angleRadians);
            var y = radius * Mathf.Sin(angleRadians);
            Vector3 position = new Vector3(x, y, 0f);

            // 更新滑块的位置
            handle.localPosition = position;
        }

        private void UpdateSliderUI(float progress)
        {
            // 更新滑动条填充图像的填充量
            sliderFillImage.fillAmount = progress;
        }
    }
}