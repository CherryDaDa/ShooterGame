using UnityEngine;

namespace Framework.Tools
{
    public static class DebugUtil
    {
        public static void Log(object message, Object context = null)
        {
            Debug.Log($"---------->>>{message}", context);
        }
    
        public static void LogWarning(object message, Object context = null)
        {
            Debug.LogWarning($"---------->>>{message}", context);
        }
    
        public static void LogError(object message, Object context = null)
        {
            Debug.LogError($"---------->>>{message}", context);
        }
    }
}