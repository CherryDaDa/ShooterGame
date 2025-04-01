using System;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{
    /// <summary>
    /// 界面基类
    /// </summary>
    public class PanelBase : MonoBehaviour
    {
        /// <summary>
        /// 目标界面（默认为组件的GameObject）
        /// </summary>
        [SerializeField] private Transform m_Panel;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        [SerializeField] private Button m_CloseButton;

        protected RectTransform _rectTransform;

        public bool IsInitCompleted { get; private set; }

        public void Close()
        {
         
            OnClosed();
            Destroy(this.gameObject);
            
        }


        #region 可由子类重写的函数

        /// <summary>
        /// 初始化（Awake）
        /// </summary>
        public virtual void Init()
        {

        }

        /// <summary>
        /// 添加事件
        /// </summary>
        protected virtual void AddEvents()
        {

        }

        /// <summary>
        /// 移除事件
        /// </summary>
        protected virtual void RemoveEvents()
        {

        }
        
        /// <summary>
        /// 当界面打开时
        /// </summary>
        protected virtual void OnOpened()
        {
            
        }

        /// <summary>
        /// 当界面关闭时触发
        /// </summary>
        protected virtual void OnClosed()
        {

        }

        #endregion


        #region 界面基类生命周期

        private void Awake()
        {
            if (!m_Panel)
            {
                m_Panel = this.transform;
            }

            _rectTransform = m_Panel.GetComponent<RectTransform>();
            
            Init();
            IsInitCompleted = true;
        }

        private void Start()
        {
            if (m_CloseButton)
            {
                m_CloseButton.onClick.AddListener(OnCloseButtonHandler);
            }
            
            OnOpened();
        }

        protected virtual void OnEnable()
        {
            AddEvents();
        }

        protected virtual void OnDisable()
        {
            RemoveEvents();
        }

        private void OnDestroy()
        {
            if (m_CloseButton)
            {
                m_CloseButton.onClick.RemoveListener(OnCloseButtonHandler);
            }
        }

        private void OnCloseButtonHandler()
        {
            Close();
        }

        #endregion
    }
}

