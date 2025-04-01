using Framework.Asset;
using Framework.Core;
using UnityEngine;

namespace Framework.UI
{
    /// <summary>
    /// 界面管理类
    /// </summary>
    public class GUIMgr : MonoSingleton<GUIMgr>
    {
        [SerializeField]
        private Transform _canvas;

        private const string GUI_ROOT_PATH = "Prefabs/Panel/";
        
        /// <summary>
        /// 实例化界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">界面相对路径</param>
        /// <param name="parent">界面父容器</param>
        /// <returns></returns>
        public T InstantiatePanel<T>(string path, Transform parent = null) where T : PanelBase
        {
            if (!_canvas)
            {
                _canvas = GameObject.Find("UI/Canvas").transform;
            }

            var panelTmp = AssetMgr.LoadAsset<GameObject>(GUI_ROOT_PATH + path);
            var panelIns = Instantiate(panelTmp, parent ? parent : _canvas);
            return panelIns.GetComponent<T>();
        }
        
    }
}

