using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Rendering.Universal.RayMarching
{
    /// <summary>
    /// ray marching
    /// </summary>
    public class RayMarchingProcessPass : ScriptableRenderPass
    {
        const string k_RenderPostProcessingTag = "Render RayMarchingProcessing Effects";
        const string k_RenderFinalPostProcessingTag = "Render Final RayMarchingProcessing Pass";

        MaterialLibrary m_Materials;
        RayMarchingProcessData m_Data;
        ScriptableRenderer m_renderer;
        RenderingData m_renderingData;

        public RayMarchingProcessPass()
        {

        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get(k_RenderPostProcessingTag);
            Render(cmd, ref renderingData);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Setup(RenderPassEvent @event, ScriptableRenderer renderer, RenderingData renderingData, RayMarchingProcessData data)
        {
            m_Data = data;
            m_renderer = renderer;
            m_renderingData = renderingData;
            renderPassEvent = @event;
            m_Materials = new MaterialLibrary(data);
        }

        void Render(CommandBuffer cmd,ref RenderingData renderingData)
        {
            ref var cameraData = ref renderingData.cameraData;
            if (!cameraData.isSceneViewCamera)
            {
                DrawSimpleSphere(cmd, ref renderingData, m_Materials.rayMarching);
            }
        }

        void DrawSimpleSphere(CommandBuffer cmd, ref RenderingData renderingData, Material rayMaterial)
        {
            cmd.Blit(m_renderer.cameraColorTarget, RenderTargetHandle.CameraTarget.Identifier(), rayMaterial);
        }
    }
}
