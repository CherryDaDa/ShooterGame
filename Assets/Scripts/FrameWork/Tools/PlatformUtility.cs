using UnityEngine;

namespace Framework.Tools
{
    public static class PlatformUtility
    {
        public static bool IsMobilePlatform()
        {
            return Application.isMobilePlatform;
        }

        public static bool IsAndroidPlatform()
        {
            return Application.platform == RuntimePlatform.Android;
        }

        public static bool IsIOSPlatform()
        {
            return Application.platform == RuntimePlatform.IPhonePlayer;
        }

        // ... 其他平台检查和特定功能
    }
}