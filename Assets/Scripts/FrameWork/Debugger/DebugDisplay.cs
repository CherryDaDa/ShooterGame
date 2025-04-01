using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Debugger
{
    public class DebugDisplay : MonoBehaviour
    {
        public int maxLogs = 10;
        
        private Queue<string> _logQueue = new Queue<string>();
        private bool _openDebug;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.F1))
            {
                _openDebug = !_openDebug;
            }
        }

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (_logQueue.Count >= maxLogs)
            {
                _logQueue.Dequeue();
            }

            _logQueue.Enqueue(logString + "\nstackTrace:" + stackTrace);
        }

        void OnGUI()
        {
            if (_openDebug)
            {
                GUILayout.BeginArea(new Rect(10, 10, 500, 1000));
                foreach (string log in _logQueue)
                {
                    GUILayout.Label(log);
                }
                GUILayout.EndArea();
            }
        }
    }
}