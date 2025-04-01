using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.Component.GUI
{
    public class ColorPicker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public RawImage colorPaletteImage;   // 调色盘图片
        public Image selectedColorImage;     // 显示选中颜色的图片
        public TMP_Text rgbText;                 // 显示RGB值的Text
        public TMP_Text hexText;                 // 显示HEX值的Text
        public Slider brightnessSlider;      // 用于调整亮度的滑动条
        public Slider alphaSlider;           // 用于调整透明度的滑动条
        public int textureWidth = 256;
        public int textureHeight = 256;

        private Texture2D colorPaletteTexture;
        private float currentBrightness = 1f; // 当前亮度，初始为1
        private float currentAlpha = 1f;      // 当前Alpha值，初始为1

        void Start()
        {
            CreateColorPalette();

            // 监听滑动条的值变化
            if (brightnessSlider != null)
            {
                brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
            }

            if (alphaSlider != null)
            {
                alphaSlider.onValueChanged.AddListener(OnAlphaChanged);
            }
        }

        // 创建颜色调色盘纹理
        void CreateColorPalette()
        {
            colorPaletteTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
            colorPaletteTexture.wrapMode = TextureWrapMode.Clamp;

            // 填充每个像素的颜色
            for (int y = 0; y < textureHeight; y++)
            {
                for (int x = 0; x < textureWidth; x++)
                {
                    float hue = (float)x / textureWidth;
                    float saturation = (float)y / textureHeight;
                    Color color = Color.HSVToRGB(hue, saturation, currentBrightness); // 使用当前亮度
                    colorPaletteTexture.SetPixel(x, y, color);
                }
            }

            colorPaletteTexture.Apply();

            // 将生成的纹理赋值给RawImage
            if (colorPaletteImage != null)
            {
                colorPaletteImage.texture = colorPaletteTexture;
            }
        }

        // 当用户点击或拖动时触发
        public void OnPointerDown(PointerEventData eventData)
        {
            SelectColor(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SelectColor(eventData);
        }

        public void OnPointerUp(PointerEventData eventData) { }

        // 根据点击位置选择颜色
        private void SelectColor(PointerEventData eventData)
        {
            // 获取点击的位置（相对于调色盘）
            RectTransform rectTransform = colorPaletteImage.GetComponent<RectTransform>();
            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localCursor))
            {
                return;
            }

            // 将本地坐标转换为纹理坐标
            float x = Mathf.Clamp(localCursor.x / rectTransform.rect.width + 0.5f, 0f, 1f) * textureWidth;
            float y = Mathf.Clamp(localCursor.y / rectTransform.rect.height + 0.5f, 0f, 1f) * textureHeight;

            // 获取对应的颜色并应用当前亮度和透明度
            Color selectedColor = colorPaletteTexture.GetPixel((int)x, (int)y);
            selectedColor *= currentBrightness; // 应用亮度
            selectedColor.a = currentAlpha;     // 应用透明度

            // 显示选中的颜色
            if (selectedColorImage != null)
            {
                selectedColorImage.color = selectedColor;
            }

            // 显示RGB值
            if (rgbText != null)
            {
                rgbText.text = $"RGB: {Mathf.RoundToInt(selectedColor.r * 255)}, {Mathf.RoundToInt(selectedColor.g * 255)}, {Mathf.RoundToInt(selectedColor.b * 255)}";
            }

            // 显示HEX值
            if (hexText != null)
            {
                hexText.text = $"HEX: #{ColorUtility.ToHtmlStringRGB(selectedColor)}";
            }
        }

        // 当亮度滑动条的值发生变化时
        public void OnBrightnessChanged(float value)
        {
            currentBrightness = value; // 更新当前亮度
            CreateColorPalette(); // 重新生成调色盘
        }

        // 当透明度滑动条的值发生变化时
        public void OnAlphaChanged(float value)
        {
            currentAlpha = value; // 更新当前透明度
        }
    }
}
