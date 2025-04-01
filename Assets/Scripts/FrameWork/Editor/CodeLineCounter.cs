using System.IO;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public abstract class CodeLineCounter
    {
        [MenuItem("Tools/Count Code Lines")]
        public static void CountCodeLines()
        {
            string[] filePaths = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
            int totalLines = 0;

            foreach (string filePath in filePaths)
            {
                string[] lines = File.ReadAllLines(filePath);
                totalLines += lines.Length;
            }

            Debug.Log("Total lines of code: " + totalLines);
        }
    }
}