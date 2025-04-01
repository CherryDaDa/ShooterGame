using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Framework.RenderFeatures.ObjectOutline
{
    public class OutlineRenderFeature : ScriptableRendererFeature
    {
        class OutlinePass : ScriptableRenderPass
        {
            private Material outlineMaterial;
            private List<Renderer> outlineRenderers;

            public OutlinePass(Material material)
            {
                outlineMaterial = material;
            }

            public void Setup(List<Renderer> renderers)
            {
                outlineRenderers = renderers;
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                CommandBuffer cmd = CommandBufferPool.Get("OutlinePass");

                foreach (Renderer renderer in outlineRenderers)
                {
                    // Debug.LogError("renderer:" + renderer);
                    // Debug.LogError("outlineMaterial:" + outlineMaterial);
                    cmd.DrawRenderer(renderer, outlineMaterial);
                }

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
        }

        public Material outlineMaterial;
        private OutlinePass outlinePass;

        public override void Create()
        {
            outlinePass = new OutlinePass(outlineMaterial)
            {
                renderPassEvent = RenderPassEvent.AfterRenderingOpaques
            };
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            List<Renderer> renderers = ObjectOutlineManager.Instance.GetOutlineRenderers();
            outlinePass.Setup(renderers);
            renderer.EnqueuePass(outlinePass);
        }
    }
}