using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Attributes;
using Framework.Extension;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.Component.GUI
{
    /// <summary>
    /// 图片滑动切换组件
    /// </summary>
    public class ImageSlider : MonoBehaviour
    {
        public Sprite[] spriteList;
        [HideInInspector] public string[] spriteNameList;
        public Image imageContainerTmp;
        public Toggle imageIndexToggleTmp;
        public ScrollRect scrollView;
        public Progress intervalProgressBar;

        public Button leftArrow = null;
        public Button rightArrow = null;
        public float dragThreshold = 50.0f;
        public float changeTime = 0.5f;

        [Header("Auto Play")]
        public bool autoPlay = true;

        [IsShow("autoPlay == true")]
        public float interval = 5.0f;

        [IsShow("autoPlay == true")]
        public bool isShowScrollBar = true;

        [Header("是否显示缩略图")]
        public bool isShowThumbnail = false;

        public Action<Sprite, string> buttonEvent;
        public Action<Sprite, string> currentSpriteEvent;
        public Sprite defaultImage;

        private readonly List<Toggle> _toggles = new List<Toggle>();
        private readonly List<Image> _images = new List<Image>();
        private int _index;
        private bool _isDrag;
        private Vector2 _beginMousePosition;
        private float _autoPlayTimer;

        public void ResetImage()
        {
            //初始化参数
            _index = 0;
            _autoPlayTimer = 0;
            _isDrag = false;
            _isScrolling = false;

            //初始化图片
            scrollView.content.ClearChildren(imageContainerTmp.transform);
            _images.Clear();
            if (spriteList.Length == 0)
            {
                if (defaultImage)
                {
                    spriteList = new[] { defaultImage };
                }
            }

            for (var i = 0; i < spriteList.Length; i++)
            {
                var t = spriteList[i];
                var image = Instantiate(imageContainerTmp, scrollView.content);
                image.gameObject.SetActive(true);
                if (image.transform.childCount > 0) //给滚动资源加遮罩 第一个子物体为要展示的图片
                {
                    Image childImage = image.transform.GetChild(0).GetComponent<Image>();
                    if (childImage != null)
                    {
                        childImage.sprite = t;
                    }

                    if (buttonEvent != null)
                    {
                        var i1 = i;
                        childImage.AddComponent<Button>().onClick.AddListener(() => { buttonEvent?.Invoke(t, spriteNameList[i1]); });
                    }
                }
                else
                {
                    image.sprite = t;
                }

                image.enabled = true;
                _images.Add(image);
            }

            //初始化Toggle
            var parent = imageIndexToggleTmp.transform.parent;
            parent.ClearChildren(imageIndexToggleTmp.transform);
            parent.gameObject.SetActive(false);
            foreach (var t in _toggles)
            {
                t.onValueChanged.RemoveAllListeners();
            }

            _toggles.Clear();
            //先解除初始必选 防止toggle自动多次自动调用
            parent.GetComponent<ToggleGroup>().allowSwitchOff = true;
            for (var i = 0; i < spriteList.Length; i++)
            {
                var t = Instantiate(imageIndexToggleTmp, parent);
                var index = i;
                t.gameObject.SetActive(true);
                if (isShowThumbnail)
                {
                    t.targetGraphic.GetComponent<Image>().sprite = spriteList[i];
                    SetImageSize(t.targetGraphic.GetComponent<Image>(), spriteList[i]);
                }

                t.onValueChanged.RemoveAllListeners();
                t.onValueChanged.AddListener((isOn) => { OnToggleClickHandler(t, index); });

                // t.isOn = i == 0;
                t.isOn = false;
                _toggles.Add(t);
            }

            parent.GetChild(0).GetComponent<Toggle>().isOn = true;
            parent.GetComponent<ToggleGroup>().allowSwitchOff = false;
            parent.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// 加载远程资源
        /// </summary>
        /// <param name="index"></param>
        /// <param name="url"></param>
        public void LoadTexture(int index, string url)
        {
            _images[index].LoadRemoteTexture(url, () =>
            {
                _images[index].transform.ClearChildren();
            });    
        }

        private void Awake()
        {
            intervalProgressBar.gameObject.SetActive(false);
            imageContainerTmp.gameObject.SetActive(false);
            imageIndexToggleTmp.gameObject.SetActive(false);
            ResetImage();
        }

        private void Start()
        {
            if (leftArrow == null || rightArrow == null) return;
            leftArrow.onClick.AddListener(() => OnButtonClickHandler(-1));
            rightArrow.onClick.AddListener(() => OnButtonClickHandler(1));
        }

        private void Update()
        {
            if (spriteList.Length <= 1) return;
            if (autoPlay)
            {
                if (_autoPlayTimer < interval)
                {
                    _autoPlayTimer += Time.deltaTime;
                    intervalProgressBar.SetProgress(_autoPlayTimer / interval);
                }
                else
                {
                    _autoPlayTimer = 0;
                    _index++;
                    _toggles[_index % _toggles.Count].isOn = true;
                }

                if (!intervalProgressBar.gameObject.activeSelf) intervalProgressBar.gameObject.SetActive(isShowScrollBar);
            }
            else
            {
                if (intervalProgressBar.gameObject.activeSelf)
                {
                    intervalProgressBar.gameObject.SetActive(false);
                    _autoPlayTimer = 0;
                }
            }

            if (spriteList.Length > 1 && !_isScrolling)
            {
                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    PointerEventData eventData = new PointerEventData(EventSystem.current)
                    {
                        position = UnityEngine.Input.mousePosition
                    };
                    var results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(eventData, results);

                    if (results.Count > 0)
                    {
                        GameObject clickedObject = results[0].gameObject;
                        if (clickedObject == scrollView.viewport.gameObject || clickedObject.transform.IsChildOf(scrollView.content))
                        {
                            _isDrag = true;
                            _beginMousePosition = eventData.position;
                        }
                    }
                }
                else if (_isDrag && UnityEngine.Input.GetMouseButtonUp(0))
                {
                    _isDrag = false;
                    var v = UnityEngine.Input.mousePosition.x - _beginMousePosition.x;
                    var dir = v < 0 ? 1 : -1;
                    if (Mathf.Abs(v) > dragThreshold)
                    {
                        _index = Mathf.Max(0, Mathf.Min(_index + dir, spriteList.Length - 1));
                    }

                    if (!_toggles[_index].isOn)
                    {
                        _toggles[_index].isOn = true;
                    }

                    _autoPlayTimer = 0;
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var t in _toggles)
            {
                t.onValueChanged.RemoveAllListeners();
            }

            if (leftArrow == null || rightArrow == null) return;
            leftArrow.onClick.RemoveAllListeners();
            rightArrow.onClick.RemoveAllListeners();
        }

        private void OnToggleClickHandler(Toggle toggle, int index)
        {
            if (!toggle.isOn) return;
            _index = index;
            if (_index >= spriteList.Length)
            {
                return;
            }

            currentSpriteEvent?.Invoke(spriteList[_index], spriteNameList[_index]);
            RefreshImage();
        }

        private void OnButtonClickHandler(int step)
        {
            _index += step;
            if (_index > _toggles.Count - 1)
            {
                _index = 0;
            }

            if (_index < 0)
            {
                _index = _toggles.Count - 1;
            }

            _toggles[_index].isOn = true;
        }

        private bool _isScrolling;

        private void RefreshImage()
        {
            var count = spriteList.Length;
            var pos = Mathf.Lerp(0, 1, (float)_index / (count - 1));
            if (Math.Abs(pos - scrollView.horizontalNormalizedPosition) > 0.001f)
            {
                // scrollView.horizontalNormalizedPosition = pos;
                _isScrolling = true;
                scrollView.DOKill();
                scrollView.DOHorizontalNormalizedPos(pos, changeTime).onComplete = () => { _isScrolling = false; };
                // Debug.Log($"滚动目标：{pos}");
            }
        }

        /// <summary>
        /// 以图片最小边进行缩放
        /// </summary>
        private void SetImageSize(Image targetImg, Sprite curSprite)
        {
            Vector2 imgSize = targetImg.GetComponent<RectTransform>().rect.size;
            targetImg.sprite = curSprite;
            targetImg.SetNativeSize();

            float xScale = imgSize.x / curSprite.rect.width;
            float yScale = imgSize.y / curSprite.rect.height;
            float setScale = 1;
            setScale = Mathf.Max(xScale, yScale);
            targetImg.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            targetImg.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            targetImg.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            targetImg.rectTransform.localScale = Vector3.one * setScale;
        }
    }
}