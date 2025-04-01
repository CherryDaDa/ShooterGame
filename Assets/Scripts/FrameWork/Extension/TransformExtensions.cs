using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.Extension
{
    public static class TransformExtensions
    {
        public static bool TryGetChildren(this Transform self, out Transform[] children)
        {
            children = new Transform[self.childCount];
            if (self.childCount > 0)
            {
                for (int i = 0; i < self.childCount; i++)
                {
                    children[i] = self.GetChild(i);
                }
            }
            return children.Length > 0;
        }

        /// <summary>
        /// [扩展方法] 清空Transform
        /// </summary>
        /// <param name="self"></param>
        /// <param name="reserved"></param>
        public static void ClearChildren(this Transform self, Transform[] reserved)
        {
            //生成临时的待删除的列表
            List<GameObject> waitRemoveObjs = new List<GameObject>();
            for (int i = 0; i < self.childCount; i++)
            {
                waitRemoveObjs.Add(self.GetChild(i).gameObject);
            }
            
            //如果有需要保留的子对象，则移除待删除列表
            if (reserved != null)
            {
                foreach (var r in reserved)
                {
                    if (waitRemoveObjs.Contains(r.gameObject))
                    {
                        waitRemoveObjs.Remove(r.gameObject);
                    }
                }
            }
            
#if !UNITY_EDITOR
            while (waitRemoveObjs.Count > 0)
            {
                 var obj = waitRemoveObjs[0];
                 waitRemoveObjs.RemoveAt(0);
                 Object.Destroy(obj);  
            }
#else
            while (waitRemoveObjs.Count > 0)
            {
                var obj = waitRemoveObjs[0];
                waitRemoveObjs.RemoveAt(0);
                Object.DestroyImmediate(obj);  
            }
#endif
        }
        
        public static void ClearChildren(this Transform self)
        {
            //生成临时的待删除的列表
            List<Transform> waitRemoveObjs = new List<Transform>();
            for (int i = 0; i < self.childCount; i++)
            {
                waitRemoveObjs.Add(self.GetChild(i));
            }
            
#if !UNITY_EDITOR
            while (waitRemoveObjs.Count > 0)
            {
                 var obj = waitRemoveObjs[0];
                 Object.Destroy(obj);  
            }
#else
            while (waitRemoveObjs.Count > 0)
            {
                var obj = waitRemoveObjs[0];
                Object.DestroyImmediate(obj);  
            }
#endif
        }
        
        public static void ClearChildren(this Transform self, Transform reserved)
        {
            ClearChildren(self, new Transform[] { reserved} );
        }

        /// <summary>
        /// 查找一个同名子对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static Transform FindFirstChildByName(this Transform parent, string childName)
        {
            // Debug.Log($"----------------Transform:{parent.name}-----------------");
            Transform foundChild = null;
            FindChildRecursive(parent, childName, ref foundChild);
            return foundChild;
        }
        
        private static void FindChildRecursive(Transform parent, string childName, ref Transform foundChild)
        {
            foreach (Transform child in parent)
            {
                if (child.name.Equals(childName))
                {
                    foundChild = child;
                    return; // 找到第一个匹配的子对象后立即停止查找
                }
                // Debug.Log($"Child:{child.name} (parent:{parent.name})");
                FindChildRecursive(child, childName, ref foundChild);
            }
        }

        /// <summary>
        /// 查找多个同名子对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static Transform[] FindChildrenByName(this Transform parent, string childName)
        {
            Transform[] foundChildren = new Transform[0];
            FindChildrenRecursive(parent, childName, ref foundChildren);
            return foundChildren;
        }
        
        private static void FindChildrenRecursive(Transform parent, string childName, ref Transform[] foundChildren)
        {
            foreach (Transform child in parent)
            {
                if (child.name == childName)
                {
                    System.Array.Resize(ref foundChildren, foundChildren.Length + 1);
                    foundChildren[^1] = child;
                }

                FindChildrenRecursive(child, childName, ref foundChildren);
            }
        }
        
        /// <summary>
        /// 移除组件
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveComponent<T>(this Transform self) where T : UnityEngine.Component
        {
            var com = self.GetComponent<T>();
            if (com != null)
            {
                Object.Destroy(com);
            }
        }
        
        // 将 UnityEngine.Vector3 转换为 System.Numerics.Vector3
        public static System.Numerics.Vector3 ToNumericsVector3(this UnityEngine.Vector3 vector)
        {
            return new System.Numerics.Vector3(vector.x, vector.y, vector.z);
        }

        // 将 System.Numerics.Vector3 转换为 UnityEngine.Vector3
        public static UnityEngine.Vector3 ToUnityVector3(this System.Numerics.Vector3 vector)
        {
            return new UnityEngine.Vector3(vector.X, vector.Y, vector.Z);
        }
        
        // 将 UnityEngine.Quaternion 转换为 System.Numerics.Quaternion
        public static System.Numerics.Quaternion ToNumericsQuaternion(this UnityEngine.Quaternion quaternion)
        {
            return new System.Numerics.Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }

        // 将 System.Numerics.Quaternion 转换为 UnityEngine.Quaternion
        public static UnityEngine.Quaternion ToUnityQuaternion(this System.Numerics.Quaternion quaternion)
        {
            return new UnityEngine.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        }
    }
}