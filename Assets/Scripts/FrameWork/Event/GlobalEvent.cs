using System.Collections.Generic;
using Framework.Tools;

namespace Framework.Event
{
    public delegate void GlobalEventHandler(IEventTargetBase data);

    public interface IEventTargetBase
    {
        
    }
    
    public interface IEventTarget<T> : IEventTargetBase
    {
        T Data { get; set; }
    }

    public class EventInfo<T> : IEventTarget<T>
    {
        public T Data { get; set; }
    }
    
    /// <summary>
    /// 全局事件 常用方法
    /// </summary>
    public partial class GlobalEvent
    {
        private static readonly Dictionary<string, List<GlobalEventHandler>> EventDic = new Dictionary<string, List<GlobalEventHandler>>();
        
        //---------------------------监听事件---------------------------------
        public static void AddListener(string evtName, GlobalEventHandler handler)
        {
            if (!EventDic.ContainsKey(evtName))
            {
                EventDic.Add(evtName, new List<GlobalEventHandler>());
            }
            var events = EventDic[evtName];
            if (!events.Contains(handler))
            {
                events.Add(handler);
            }
            DebugUtil.Log($"Add listener ---> Event:{evtName}  Handler:{handler.ToString()}");
        }

        //---------------------------移除事件---------------------------------
        public static void RemoveListener(string evtName, GlobalEventHandler handler)
        {
            if (EventDic.TryGetValue(evtName, out var events))
            {
                if (events.Contains(handler))
                {
                    events.Remove(handler);
                    DebugUtil.Log($"Remove listener ---> Event:{evtName}  Handler:{handler.ToString()}");
                }
            }
        }
        
        //---------------------------派发事件---------------------------------
        
        /// <summary>
        /// 派发事件
        /// </summary>
        /// <param name="evtName"></param>
        /// <param name="data"></param>
        public static void Dispatch(string evtName, IEventTargetBase data = null)
        {
            if (!EventDic.TryGetValue(evtName, out var events)) return;
            DebugUtil.Log($"Dispatch event ---> Event:{evtName}----------------------------");
            foreach (var evt in events)
            {
                DebugUtil.Log($"Handler:{data}");
                evt?.Invoke(data);
            }
        } 
    }
}
