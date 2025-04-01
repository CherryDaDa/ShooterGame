using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// AB包编辑器
/// </summary>
public class AssetBundleEditor : EditorWindow
{
    [MenuItem("Tools/打包工具")]
    public static void ShowWindow()
    {
        //显示窗口
        EditorWindow.GetWindow(typeof(AssetBundleEditor));
    }
}
