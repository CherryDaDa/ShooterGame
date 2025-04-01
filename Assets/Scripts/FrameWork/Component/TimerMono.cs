using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Tools;
using UnityEngine;

namespace Framework.Component
{
    public delegate void GlobalTimerCall(string timerName, uint time); 
    public delegate void TimerCall(uint time); 
    
    /// <summary>
    /// Mono类型的计时器
    /// </summary>
    public class TimerMono : MonoBehaviour
    {
        public bool dontDestroy;
        public float interval = 1.0f;

        public string TimerName { get; private set; }
        
        /// <summary>
        /// 累计时间
        /// </summary>
        public uint TotalTime { get; private set; }

        /// <summary>
        /// 当时间变化时的事件
        /// </summary>
        public event TimerCall OnTimerChangedEvent;
        
        /// <summary>
        /// 开始计时
        /// </summary>
        /// <returns></returns>
        public TimerMono StartTimer()
        {
            TotalTime = 0;
            OnTimerChangedEvent?.Invoke(TotalTime);
            OnGlobalTimerChangedEvent?.Invoke(TimerName, TotalTime);
            StartCoroutine(nameof(SecondStepTimer));
            return this;
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        /// <returns></returns>
        public bool StopTimer()
        {
            StopAllCoroutines();
            return _timerDic.ContainsKey(TimerName) && _timerDic.Remove(TimerName);
        }

        
        
        
        private WaitForSecondsRealtime _secondsRealtime;

        private void Start()
        {
            _secondsRealtime = new WaitForSecondsRealtime(interval);
            if (dontDestroy)
            {
                DontDestroyOnLoad(this);
            }
        }

        private void OnDestroy()
        {
            StopTimer();
        }

        private IEnumerator SecondStepTimer()
        {
            while (true)
            {
                yield return _secondsRealtime;
                TotalTime++;
                // DebugUtil.Log($"在线时长：{TimeUtility.SecondsToHHMMSS(TotalTime)}");
                OnTimerChangedEvent?.Invoke(TotalTime);
                OnGlobalTimerChangedEvent?.Invoke(TimerName, TotalTime);
            }
        }
        
        
        
        #region 一些静态方法，方便创建、移除、获取以及监听时间变化

        public static event GlobalTimerCall OnGlobalTimerChangedEvent;
        
        private static Dictionary<string, TimerMono> _timerDic;

        /// <summary>
        /// 生成一个计时器
        /// </summary>
        /// <param name="timerName"></param>
        /// <param name="dontDestroy"></param>
        /// <returns></returns>
        public static TimerMono CreateTimer(string timerName, bool dontDestroy = false)
        {
            _timerDic ??= new Dictionary<string, TimerMono>();
            if (!_timerDic.TryGetValue(timerName, out var tm))
            {
                var gameObject = new GameObject($"Timer_{TimeUtility.GetUtcTimestamp()}");
                var timer = gameObject.AddComponent<TimerMono>();
                timer.TimerName = timerName;
                timer.dontDestroy = dontDestroy;
                _timerDic.Add(timerName, timer);
                return timer;
            }
            return tm;
        }

        public static bool StopTimer(string timerName)
        {
            return _timerDic[timerName].StopTimer();
        }

        public static TimerMono GetTimer(string timerName)
        {
            return _timerDic.TryGetValue(timerName, out var value) ? value : null;
        }

        #endregion
    }
}