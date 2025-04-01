using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.RenderFeatures.CutMask
{
    [ExecuteAlways]
    public class CircleMaskController : MonoBehaviour
    {
        public GameObject loadingIcon;
        public Material maskMaterial;
        public Texture2D maskTexture;
        [Range(0, 1.0f)] public float maskSize = 0.5f;
        public Vector2 center = new Vector2(0.5f, 0.5f);
        public Vector2 range = new Vector2(0, 2.0f);
        public float transitionTime = 0.5f;
        
        public bool IsFade { get; private set; }
        
        // private RawImage _rawImage;
        private Action _call;
        private static readonly int MaskSize = Shader.PropertyToID("_MaskSize");
        private static readonly int Center = Shader.PropertyToID("_Center");
        private static readonly int AspectRatio = Shader.PropertyToID("_AspectRatio");
        private static readonly int MaskColor = Shader.PropertyToID("_MaskColor");
        private static readonly int MaskTexture = Shader.PropertyToID("_MaskTexture");

        private void Awake()
        {
            // _rawImage = GetComponent<RawImage>();
            if(loadingIcon) loadingIcon.SetActive(false);
        }

        public void PlayIn(Action call = null)
        {
            if (maskSize == 0) return;
            _call = call;
            StopAllCoroutines();
            StartCoroutine(nameof(PlayTransition));
        }

        public void PlayOut(Action call = null)
        {
            _call = call;
            StopAllCoroutines();
            StartCoroutine(nameof(StopTransition));
        }
        
        private IEnumerator PlayTransition()
        {
            IsFade = true;
            if(loadingIcon) loadingIcon.SetActive(false);
            //从1-0
            var t = transitionTime;
            while (t > 0)
            {
                maskSize = Mathf.Clamp01(t / transitionTime);
                t -= Time.smoothDeltaTime;
                UpdateMask();
                yield return null;
            }
            maskSize = 0;
            UpdateMask();
            yield return null;
            _call?.Invoke();
            if(loadingIcon) loadingIcon.SetActive(true);
            IsFade = false;
        }
        
        private IEnumerator StopTransition()
        {
            IsFade = true;
            if(loadingIcon) loadingIcon.SetActive(false);
            //从0-1
            float t = 0;
            while (t < transitionTime)
            {
                maskSize = Mathf.Clamp01(t / transitionTime);
                t += Time.smoothDeltaTime;
                UpdateMask();
                yield return null;
            }
            maskSize = 1;
            UpdateMask();
            yield return null;
            _call?.Invoke();
            IsFade = false;
        }

        // private void OnValidate()
        // {
        //     if (_rawImage != null)
        //     {
        //         maskMaterial.SetColor(MaskColor, _rawImage.color);
        //     }
        //         
        //     maskMaterial.SetTexture(MaskTexture, maskTexture);
        //     UpdateMask();
        // }

        private void UpdateMask()
        {
            if (maskMaterial != null)
            {
                maskMaterial.SetFloat(MaskSize, maskSize * (range.y - range.x));
                maskMaterial.SetVector(Center, center);

                float aspectRatio = (float)Screen.width / Screen.height;
                maskMaterial.SetFloat(AspectRatio, aspectRatio);
            }
            else
            {
                Debug.LogError("Mask material is null");
            }
        }
    }
}