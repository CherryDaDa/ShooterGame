using System.Collections;
using System.Collections.Generic;
using Framework.Core;
using UnityEngine;

namespace Framework.Version
{
    public struct VersionInfo
    {
        /// <summary>
        /// 应用版本号
        /// </summary>
        public string AppVersion;
        
        /// <summary>
        /// 资源版本号
        /// </summary>
        public string ResVersion;
    }
    
    /// <summary>
    /// 版本管理器
    /// </summary>
    public class VersionMgr : MonoSingleton<VersionMgr>
    {
        /// <summary>
        /// 本地版本文件
        /// </summary>
        public VersionInfo LocalVersion{ get; private set; }
        
        /// <summary>
        /// 远程版本文件
        /// </summary>
        public VersionInfo RemoteVersion { get; private set; }

        /// <summary>
        /// 比较版本号 v1高于v2返回1 v1低于v2返回-1 v1等于v2返回0
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static int ComparativeVersion(string v1, string v2)
        {
            if (v1.Equals(v2))
            {
                return 0;
            }
            
            var v1Arr = v1.Split(".");
            var v2Arr = v2.Split(".");

            for (var i = 0; i < v1Arr.Length; i++)
            {
                var l = Mathf.Max(v1Arr[i].Length, v2Arr[i].Length);
                var n1 = int.Parse(v1Arr[i].PadRight(l));
                var n2 = int.Parse(v2Arr[i].PadRight(l));
                if (n1 > n2)
                {
                    return 1;
                }
            }
            
            return -1;
        }
    }
}

