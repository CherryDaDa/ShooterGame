using System.Collections.Generic;

namespace Framework.Tools
{
    public static class ListUtility
    {
        public static T RandomElement<T>(List<T> list)
        {
            if (list.Count == 0) return default(T);
            int index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        public static void Shuffle<T>(List<T> list)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                int r = i + UnityEngine.Random.Range(0, count - i);
                (list[i], list[r]) = (list[r], list[i]);
            }
        }

        // ... 其他列表处理方法
    }
}