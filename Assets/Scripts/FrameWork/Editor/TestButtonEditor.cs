using System;
using Framework.Attributes;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)] 
    public class TestButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MonoBehaviour script = (MonoBehaviour)target;

            System.Reflection.MethodInfo[] methods = script.GetType().GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (Attribute.IsDefined(method, typeof(TestButtonAttribute)))
                {
                    TestButtonAttribute customButtonAttribute = (TestButtonAttribute)method.GetCustomAttributes(typeof(TestButtonAttribute), true)[0];

                    if (GUILayout.Button(customButtonAttribute.ButtonName))
                    {
                        method.Invoke(script, null);
                    }
                }
            }
        }
    }
}