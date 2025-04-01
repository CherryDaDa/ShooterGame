using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct MaterialTexture
{
    public Texture MainTexture { get; set; }
    public Texture NormalTexture { get; set; }
}

namespace Framework.Component
{
    /// <summary>
    /// 自动替换材质（主贴图）
    /// </summary>
    [ExecuteInEditMode]
    public class AutoMaterials : MonoBehaviour
    {
        public Material[] materials;

        private Renderer[] m_RendererList;

        private Dictionary<Renderer, MaterialTexture> m_RendererTextureCache;

        private void OnDrawGizmosSelected()
        {
            if (materials == null || materials.Length == 0)
                return;

            if (m_RendererList == null)
            {
                m_RendererList = GetComponentsInChildren<Renderer>();
            }
            if (m_RendererTextureCache == null)
            {
                m_RendererTextureCache = new Dictionary<Renderer, MaterialTexture>();
            }
            if (m_RendererList != null)
            {
                foreach (Renderer r in m_RendererList)
                {
                    ReplaceMaterials(r);
                }
            }
        }

        private void ReplaceMaterials(Renderer rend)
        {
            //Renderer rend = go.GetComponent<Renderer>();
            if (rend != null)
            {
                List<Texture> originalTextures = new List<Texture>();

                // 遍历原有的材质，缓存它们的主贴图
                 foreach (Material mat in rend.materials)
                {
                    if (mat && mat.mainTexture != null)
                    {
                        originalTextures.Add(mat.mainTexture);
                    }
                    else
                    {
                        originalTextures.Add(null);  // 为没有主贴图的材质添加null占位符
                    }
                }

                List<Material> newMaterialInstances = new List<Material>();

                // 为新的材质创建实例，并设置之前缓存的主贴图
                for (int i = 0; i < materials.Length; i++)
                {
                    Material newMatInstance = Instantiate(materials[i]);
                    if (i < originalTextures.Count)
                    {
                        newMatInstance.mainTexture = originalTextures[i];
                    }
                    newMaterialInstances.Add(newMatInstance);
                }

                // 替换原有的材质列表为新的材质实例列表
                rend.materials = newMaterialInstances.ToArray();
            }

            // 递归处理所有子对象
            //foreach (Transform child in go.transform)
            //{
            //    ReplaceMaterials(child.gameObject);
            //}
        }
    }
}

