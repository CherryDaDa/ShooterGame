// using UnityEngine;
// using UnityEditor;
// using System.IO;
// using System.Data;
// using Newtonsoft.Json;
// using System.Collections.Generic;
// using System;
// using System.Linq;
//
// namespace Framework.Tools
// {
//     public class ExcelToJsonConverter : EditorWindow
//     {
//         [MenuItem("Tools/导表工具")]
//         public static void ShowWindow()
//         {
//             EditorWindow.GetWindow(typeof(ExcelToJsonConverter));
//         }
//
//         private string excelFolderPath = "D:/Work/Metaverse/branches/project/Configs/";
//         private string jsonOutputFolderPath = "D:/Work/Metaverse/branches/project/Metaverse_20230808/Assets/Resources/Config/";
//
//         void OnGUI()
//         {
//             GUILayout.Label("导表工具", EditorStyles.boldLabel);
//
//             EditorGUILayout.BeginHorizontal();
//             excelFolderPath = EditorGUILayout.TextField("配置表文件夹路径", excelFolderPath);
//             if (GUILayout.Button("打开", GUILayout.Width(50)))
//             {
//                 System.Diagnostics.Process.Start("explorer.exe", excelFolderPath.Replace("/", "\\"));
//                 //EditorUtility.OpenFolderPanel(excelFolderPath, "", "");  //打开选择文件夹窗口
//             }
//             EditorGUILayout.EndHorizontal();
//
//             EditorGUILayout.BeginHorizontal();
//             jsonOutputFolderPath = EditorGUILayout.TextField("JSON表文件夹路径", jsonOutputFolderPath);
//             if (GUILayout.Button("打开", GUILayout.Width(50)))
//             {
//                 System.Diagnostics.Process.Start("explorer.exe", jsonOutputFolderPath.Replace("/", "\\"));
//             }
//             EditorGUILayout.EndHorizontal();
//
//             if (GUILayout.Button("导出"))
//             {
//                 ConvertExcelToJson(excelFolderPath, jsonOutputFolderPath);
//             }
//         }
//
//         private void ConvertExcelToJson(string excelPath, string outputPath)
//         {
//             // Ensure the paths are not empty
//             if (string.IsNullOrEmpty(excelPath) || string.IsNullOrEmpty(outputPath))
//             {
//                 Debug.LogError("Paths cannot be empty!");
//                 return;
//             }
//
//             //遍历配置表路径
//             string[] files = Directory.GetFiles(excelPath, "*.xlsx", SearchOption.AllDirectories);
//             foreach (string file in files)
//             {
//                 DataSet result;
//
//                 // Read Excel file
//                 using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
//                 {
//                     using (var reader = ExcelReaderFactory.CreateReader(stream))
//                     {
//                         result = reader.AsDataSet();
//                     }
//                 }
//
//                 // Convert to JSON
//                 var table = result.Tables[0];
//
//                 var vos = MappingClass(table);
//
//                 string json = JsonConvert.SerializeObject(vos, Formatting.Indented);
//                 //string json = JsonConvert.SerializeObject(result.Tables[0], Formatting.Indented);
//
//                 // Write JSON to file
//                 var configFileName = table.TableName + ".json";
//                 File.WriteAllText(Path.Combine(outputPath, configFileName), json);
//                 Debug.Log($"导出配置表：{configFileName}");
//             }
//
//             Debug.Log("成功导出所有配置表！");
//         }
//
//         private List<object> MappingClass(DataTable table)
//         {
//             List<object> result = new List<object>();
//
//             switch (table.TableName)
//             {
//                  case "Dialogue":
//                      for (int i = 3; i < table.Rows.Count; i++)
//                      {
//                          var data = table.Rows[i];
//                          result.Add(new DialogueVO()
//                          {
//                              ID = Convert.ToInt32(data[0]),
//                              Dialogue = data[1].ToString(),
//                              Actor = data[2].ToString(),
//                              Group = data[3].ToString(),
//                              Events = string.IsNullOrEmpty(data[4].ToString()) ? new string[]{} : data[4].ToString().Split("|"),
//                              Options = string.IsNullOrEmpty(data[5].ToString()) ? new string[]{} : data[5].ToString().Split("|"),
//                          });
//                      }
//                      break;
//                  //流程配置表
//                  case "Process":
//                      for (int i = 3; i < table.Rows.Count; i++)
//                      {
//                          var data = table.Rows[i];
//                          result.Add(new TeachProcessVO()
//                          {
//                              ID = Convert.ToInt32(data[0]),
//                              Process = data[1].ToString(),
//                              Parent = data[1].ToString()[..Mathf.Max(data[1].ToString().LastIndexOf(".", StringComparison.Ordinal), 0)],
//                              Name = data[2].ToString(),
//                              Events = string.IsNullOrEmpty(data[3].ToString()) ? new string[]{} : data[3].ToString().Split("|"),
//                          });
//                      }
//                      break;
//                  case "Question":
//                      for (int i = 3; i < table.Rows.Count; i++)
//                      {
//                          var data = table.Rows[i];
//                          result.Add(new QuestionVO()
//                          {
//                              ID = Convert.ToInt32(data[0]),
//                              Name = data[1].ToString(),
//                              Desc = data[2].ToString(),
//                              OptionTexts = string.IsNullOrEmpty(data[4].ToString()) ? new string[]{} : data[4].ToString().Split("|"),
//                              OptionImages = string.IsNullOrEmpty(data[5].ToString()) ? new string[]{} : data[5].ToString().Split("|"),
//                              Answers = data[6].ToString().Split(",").Select(s => Convert.ToInt32(s)).ToArray(),
//                              ValidationRule = Convert.ToInt32(data[7]),
//                              ValidationMode = Convert.ToInt32(data[8]),
//                              Events = string.IsNullOrEmpty(data[9].ToString()) ? new string[]{} : data[9].ToString().Split("|"),
//                          });
//                      }
//                      break;
//                  case "test":
//                      Debug.Log(table.TableName);
//                      for (int i = 3; i < table.Rows.Count; i++)
//                      {
//                          var data = table.Rows[i];
//                          result.Add(new ItemData()
//                          {
//                              ItemId = Convert.ToInt32(data[0]),
//                              ItemName = data[1].ToString(),
//                              ItemImagePath = data[2].ToString()
//                          });
//                      }
//                      break;
//                 default:
//                     Debug.Log(table.TableName);
//                     for (int i = 3; i < table.Rows.Count; i++)
//                     {
//                         
//                         var data = table.Rows[i];
//                         result.Add(new OrganVO
//                         {
//                             Id = Convert.ToInt32(data[0]),
//                             Name = data[1].ToString(),
//                             Desc = data[2].ToString(),
//                             Audio = data[3].ToString(),
//                             Objs = data[4].ToString().Split(","),
//                             CutObjs=data[5].ToString().Split(","),
//                             Photo = data[6].ToString(),
//                             ObjPrefab = data[7].ToString(),
//                             Process = data[8].ToString(),
//                             IsEquivalent = (bool)data[9],
//                             Parent = data[1].ToString()[..Mathf.Max(data[1].ToString().LastIndexOf(".", StringComparison.Ordinal), 0)]
//                         });
//                     }
//                     break;
//             }
//             return result;
//         }
//     }
// }
//
