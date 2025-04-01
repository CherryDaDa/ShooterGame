using Neu;
using UnityEngine;

namespace Game.Game
{
    /// <summary>
    /// 全局静态数据
    /// </summary>
    public static class GlobalData
    {
        /// <summary>
        /// 用户数据
        /// </summary>
        public static UserModel UserData { get; set; }
        
        
        /// <summary>
        /// 清除用户所有本地数据
        /// </summary>
        public static void ClearAll()
        {
            UserData = null;
        }
        
        
    }
}