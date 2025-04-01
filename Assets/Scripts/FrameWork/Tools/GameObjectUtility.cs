using UnityEngine;

namespace Framework.Tools
{
    public static class GameObjectUtility
    {
        public static GameObject FindChildByName(GameObject parent, string childName)
        {
            Transform childTransform = parent.transform.Find(childName);
            return childTransform ? childTransform.gameObject : null;
        }

        public static void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        // ... 其他游戏对象处理方法
    }
}