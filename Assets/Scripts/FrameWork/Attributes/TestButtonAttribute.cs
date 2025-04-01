using System;
using UnityEngine;

namespace Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class TestButtonAttribute : Attribute
    {
        public readonly string ButtonName;

        /// <summary>
        /// 自定义编辑器枚举名称
        /// </summary>
        /// <param name="buttonName"></param>
        public TestButtonAttribute(string buttonName)
        {
            this.ButtonName = buttonName;
        }
    }
}