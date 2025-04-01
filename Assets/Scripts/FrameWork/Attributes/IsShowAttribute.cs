using UnityEngine;

namespace Framework.Attributes
{
    public class IsShowAttribute : PropertyAttribute
    {
        public string condition;

        /// <summary>
        /// 是否显示（条件）
        /// </summary>
        /// <param name="condition"></param>
        public IsShowAttribute(string condition)
        {
            this.condition = condition;
        }
    }
}
