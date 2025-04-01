using System;
using Framework.Attributes;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(Enum), true)]
    public class EnumLabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Enum)
            {
                Type enumType = fieldInfo.FieldType;
                string[] names = property.enumNames;
                string[] displayedNames = new string[names.Length];

                for (int i = 0; i < names.Length; i++)
                {
                    var memInfo = enumType.GetMember(names[i]);
                    var attributes = memInfo[0].GetCustomAttributes(typeof(EnumLabelAttribute), false);
                
                    if (attributes.Length > 0)
                    {
                        displayedNames[i] = ((EnumLabelAttribute)attributes[0]).Label;
                    }
                    else
                    {
                        displayedNames[i] = names[i];
                    }
                }

                property.enumValueIndex = EditorGUI.Popup(position, label.text, property.enumValueIndex, displayedNames);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}