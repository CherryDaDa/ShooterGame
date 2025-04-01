using UnityEngine;

namespace Framework.Attributes
{
    public class EnumLabelAttribute : PropertyAttribute
    {
        public readonly string Label;

        /// <summary>
        /// 自定义编辑器枚举名称
        /// </summary>
        /// <param name="label"></param>
        public EnumLabelAttribute(string label)
        {
            this.Label = label;
        }
    }
}