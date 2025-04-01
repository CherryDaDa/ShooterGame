using System;
using Random = UnityEngine.Random;

namespace Framework.Tools
{
    public static class MathUtility
    {
        public static float Percentage(float value, float total)
        {
            return (value / total) * 100;
        }

        /// <summary>
        /// 返回两个数之间的随机值
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float RandomValue(float min, float max)
        {
            return min < max ? Random.Range(min, max) : max;
        }
        
        /// <summary>
        /// 返回指定长度的数字
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int GenerateNumber(int length = 4)
        {
            if (length <= 0)
                return 0;

            System.Random rand = new System.Random();
            int min = (int)Math.Pow(10, length - 1);
            int max = (int)Math.Pow(10, length) - 1;
            return rand.Next(min, max);
        }
    }
}