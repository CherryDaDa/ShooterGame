using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Framework.Attributes;

namespace Framework.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(IsShowAttribute))]
    public class IsShowDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            IsShowAttribute showAttr = (IsShowAttribute)attribute;

            // Check if the property is an array
            if (property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                if (ShouldShowProperty(property, showAttr))
                {
                    // Draw the array size field
                    EditorGUI.PropertyField(position, property, label, false);

                    if (property.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height), property.FindPropertyRelative("Array.size"));

                        for (int i = 0; i < property.arraySize; i++)
                        {
                            EditorGUI.PropertyField(position, property.GetArrayElementAtIndex(i));
                        }
                        EditorGUI.indentLevel--;
                    }
                }
            }
            else
            {
                if (ShouldShowProperty(property, showAttr))
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            IsShowAttribute showAttr = (IsShowAttribute)attribute;

            if (!ShouldShowProperty(property, showAttr))
            {
                return -2f; // hide the property
            }

            if (property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                if (property.isExpanded)
                {
                    float totalHeight = EditorGUIUtility.singleLineHeight; // for the main array foldout
                    totalHeight += EditorGUIUtility.singleLineHeight; // for the array size field

                    for (int i = 0; i < property.arraySize; i++)
                    {
                        totalHeight += EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(i)) + EditorGUIUtility.standardVerticalSpacing;
                    }

                    return totalHeight;
                }
                else
                {
                    return EditorGUIUtility.singleLineHeight;
                }
            }

            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        private bool ShouldShowProperty(SerializedProperty property, IsShowAttribute showAttr)
        {
            SerializedObject serializedObject = property.serializedObject;

            // Split multiple conditions using ';' as a separator
            string[] conditions = showAttr.condition.Split(';');

            // Check if all conditions are met
            return conditions.All(condition => EvaluateCondition(property, serializedObject, condition));
        }

        private bool EvaluateCondition(SerializedProperty property, SerializedObject serializedObject, string condition)
        {
            // Split condition into parts
            string[] conditionParts = condition.Split(new string[] { "&&", "||" }, StringSplitOptions.None);
            bool[] results = new bool[conditionParts.Length];

            for (int i = 0; i < conditionParts.Length; i++)
            {
                string conditionPart = conditionParts[i].Trim();
                string[] conditionElements = conditionPart.Split(' ');

                if (conditionElements.Length == 3)
                {
                    SerializedProperty conditionProperty = serializedObject.FindProperty(conditionElements[0]);
                    string operatorSymbol = conditionElements[1];
                    string compareValue = conditionElements[2];

                    if (conditionProperty == null) return true;

                    switch (conditionProperty.propertyType)
                    {
                        case SerializedPropertyType.Boolean:
                            bool boolValue = conditionProperty.boolValue;
                            bool compareBool = Convert.ToBoolean(compareValue);
                            results[i] = CompareValues(boolValue, compareBool, operatorSymbol);
                            break;
                        case SerializedPropertyType.Enum:
                            string[] enumParts = compareValue.Split('.');
                            if (enumParts.Length == 2)
                            {
                                string enumTypeName = enumParts[0];
                                string enumValueName = enumParts[1];

                                Type enumType = FindTypeInCSharpAssembly(enumTypeName);

                                if (enumType != null)
                                {
                                    int enumValue = conditionProperty.enumValueIndex;
                                    int compareEnum = (int)Enum.Parse(enumType, enumValueName);
                                    results[i] = CompareValues(enumValue, compareEnum, operatorSymbol);
                                }
                            }
                            break;
                        case SerializedPropertyType.Integer:
                            int intValue = conditionProperty.intValue;
                            int compareInt = Convert.ToInt32(compareValue);
                            results[i] = CompareValues(intValue, compareInt, operatorSymbol);
                            break;
                        case SerializedPropertyType.Float:
                            float floatValue = conditionProperty.floatValue;
                            float compareFloat = Convert.ToSingle(compareValue);
                            results[i] = CompareValues(floatValue, compareFloat, operatorSymbol);
                            break;
                        case SerializedPropertyType.String:
                            string stringValue = conditionProperty.stringValue;
                            results[i] = CompareValues(stringValue, compareValue, operatorSymbol);
                            break;
                    }
                }
            }

            // Combine results using && and || operators
            for (int i = 0; i < conditionParts.Length - 1; i++)
            {
                if (condition.Contains("&&"))
                {
                    if (!results[i]) return false;
                }
                else if (condition.Contains("||"))
                {
                    if (results[i]) return true;
                }
            }

            return results.All(r => r);
        }

        private bool CompareValues<T>(T value, T compareValue, string operatorSymbol) where T : IComparable<T>
        {
            switch (operatorSymbol)
            {
                case "==":
                    return value.CompareTo(compareValue) == 0;
                case "!=":
                    return value.CompareTo(compareValue) != 0;
                case "<":
                    return value.CompareTo(compareValue) < 0;
                case "<=":
                    return value.CompareTo(compareValue) <= 0;
                case ">":
                    return value.CompareTo(compareValue) > 0;
                case ">=":
                    return value.CompareTo(compareValue) >= 0;
            }
            return false;
        }

        private Type FindTypeInCSharpAssembly(string typeName)
        {
            Assembly csharpAssembly = Assembly.Load("Assembly-CSharp");
            if (csharpAssembly != null)
            {
                return csharpAssembly.GetTypes().FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.Ordinal));
            }
            return null;
        }
    }
}
