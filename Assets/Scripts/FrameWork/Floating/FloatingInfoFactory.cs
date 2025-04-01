using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Core;
using UnityEngine;

namespace Framework.Floating
{
    [Serializable]
    public class FloatingTextDef
    {
        public Color color = Color.white;
        public FloatingChannel channel;
        public FloatingInfoInstance tmp;
    }

    public enum FloatingChannel
    {
        Default,
        Top,
        Warning
    }
    
    /// <summary>
    /// 浮动信息控制器
    /// </summary>
    public class FloatingInfoFactory : MonoSingleton<FloatingInfoFactory>
    {
        // public RectTransform textInstanceParent;
        public FloatingTextDef[] floatingItems;
        
        private readonly Dictionary<int, List<FloatingInfoInstance>> _floatingDic = new Dictionary<int, List<FloatingInfoInstance>>();

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private FloatingTextDef GetTextDef(FloatingChannel channel)
        {
            return floatingItems.SingleOrDefault(f => f.channel == channel);
        }

        public FloatingInfoInstance CreateFloatingText(FloatingChannel channel)
        {
            // if (!textInstanceParent)
            // {
            //     textInstanceParent =
            //         GUIManager.Instance.GetLayerCanvas(GUILayer.Floating)?.GetComponent<RectTransform>();
            // }
            //
            // if (!textInstanceParent)
            // {
            //     Debug.LogError("缺少浮动信息可用的Canvas");
            //     return null;
            // }
            
            var def = GetTextDef(channel);
            var ins = Instantiate(def.tmp);
            ins.color = def.color;
            // if (!FloatingDic.ContainsKey(channel))
            // {
            //     FloatingDic.Add(channel, new List<FloatingTextInstance>());
            // }
            // FloatingDic[channel].Add(ins);
            return ins;
        }
    }
}