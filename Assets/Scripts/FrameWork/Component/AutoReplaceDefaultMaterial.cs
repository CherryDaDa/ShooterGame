#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.Component
{
    /// <summary>
    /// 自动替换默认材质为指定材质
    /// </summary>
    [ExecuteInEditMode]
    public class AutoReplaceDefaultMaterial : MonoBehaviour
    {
        /// <summary>
        /// 要替换的新材质
        /// </summary>
        public Material[] m_Materials;

        /// <summary>
        /// 该节点下遍历到的所有渲染器
        /// </summary>
        private Renderer[] m_RendererList;

        /// <summary>
        /// 缓存渲染器的材质
        /// </summary>
        // private Dictionary<Renderer, Material> m_RendererMaterilas;

        private static readonly int MetallicGlossMap = Shader.PropertyToID("_MetallicGlossMap");
        private static readonly int BumpMap = Shader.PropertyToID("_BumpMap");
        private static readonly int Smoothness = Shader.PropertyToID("_Smoothness");

        // public void Awake()
        // {
        //     // if (m_Materials == null || m_Materials.Length == 0)
        //     //     return;
        //     
        //     // if (m_RendererMaterilas == null)
        //     // {
        //     //     m_RendererMaterilas = new Dictionary<Renderer, Material>();
        //     // }
        // }
        
        // public Material templateMaterial;    // 这是你的模板材质
        public string savePath = "Assets/AssetBundle/Material/"; // 材质保存路径

        [ContextMenu("Clone and Assign Material")]
        public void CloneAndAssignMaterial()
        {
            // if (templateMaterial == null)
            // {
            //     Debug.LogError("Template material is not assigned.");
            //     return;
            // }
            if (m_Materials == null || m_Materials.Length == 0)
                return;

            m_RendererList ??= GetComponentsInChildren<Renderer>();

            if (m_RendererList is { Length: > 0 })
            {
                foreach (var render in m_RendererList)
                {
                    var rendMat = render.sharedMaterial;
                    foreach (var mat in m_Materials)
                    {
                        // 复制模板材质
                        var newMat = new Material(mat)
                        {
                            mainTexture = rendMat.mainTexture
                        };
                        if (rendMat.HasTexture(MetallicGlossMap))
                        {
                            newMat.SetTexture("_MetallicMap", rendMat.GetTexture(MetallicGlossMap));
                        }
                        if (rendMat.HasTexture(BumpMap))
                        {
                            newMat.SetTexture("_NormalMap", rendMat.GetTexture(BumpMap));
                        }
                        if (rendMat.HasFloat(Smoothness))
                        {
                            newMat.SetFloat(Smoothness, rendMat.GetFloat(Smoothness));
                        }
        
                        // 保存到指定路径
                        AssetDatabase.CreateAsset(newMat, savePath + render.gameObject.name + "_Mat.mat");

                        // 分配新材质给当前对象的 Renderer
                        // Renderer renderer = GetComponent<Renderer>();
                        if (render != null)
                        {
                            render.material = newMat;
                        }
                        else
                        {
                            Debug.LogError("No Renderer found on this object.");
                        }
                    }
                }
            }
            // m_RendererMaterilas.Clear();
        }

        private void OnDrawGizmosSelected()
        {
            // if (m_Materials == null || m_Materials.Length == 0)
            //     return;
            //
            // if (m_RendererList == null)
            // {
            //     m_RendererList = GetComponentsInChildren<Renderer>();
            // }
            // if (m_RendererMaterilas == null)
            // {
            //     m_RendererMaterilas = new Dictionary<Renderer, Material>();
            // }
            // if (m_RendererList is { Length: > 0 })
            // {
            //     foreach (Renderer r in m_RendererList)
            //     {
            //         ReplaceMaterials(r);
            //     }
            // }

            // CloneAndAssignMaterial();
        }

        /// <summary>
        /// 替换材质
        /// </summary>
        /// <param name="rend"></param>
        // private void ReplaceMaterials(Renderer rend)
        // {
        //     if (rend == null) return;
        //     
        //     //复制渲染器的所有材质到缓存列表
        //     if (!m_RendererMaterilas.TryGetValue(rend, out var randMat))
        //     {
        //         randMat = rend.sharedMaterial;
        //         m_RendererMaterilas.Add(rend, randMat);
        //     }
        //     
        //     //将材质属性赋值给新替换的材质
        //     List<Material> newMatList = new List<Material>();
        //     foreach (var mat in m_Materials)
        //     {
        //         if(mat.shader.name.Equals(randMat.shader.name)) continue;
        //
        //         // var newMat = new Material(Shader.Find(mat.name));
        //         // // 指定材质保存的路径和文件名
        //         // string path = $"Assets/{rend.gameObject.name}.mat";
        //         //
        //         // // 保存材质到项目
        //         // AssetDatabase.CreateAsset(randMat, path);
        //         // AssetDatabase.SaveAssets();
        //         
        //         var newMat = Instantiate(mat);
        //         newMat.mainTexture = randMat.mainTexture;
        //         if (randMat.HasTexture(MetallicGlossMap))
        //         {
        //             newMat.SetTexture("_MetallicMap", randMat.GetTexture(MetallicGlossMap));
        //         }
        //         if (randMat.HasTexture(BumpMap))
        //         {
        //             newMat.SetTexture("_NormalMap", randMat.GetTexture(BumpMap));
        //         }
        //         if (randMat.HasFloat(Smoothness))
        //         {
        //             newMat.SetFloat(Smoothness, randMat.GetFloat(Smoothness));
        //         }
        //         
        //         newMatList.Add(newMat);
        //     }
        //
        //     if (newMatList.Count > 0)
        //     {
        //         rend.materials = newMatList.ToArray();
        //     }
        // }
    }
}
#endif



