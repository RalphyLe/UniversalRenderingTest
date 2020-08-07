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
        RenderTargetIdentifier m_ColorAttachment;
        RenderTargetIdentifier m_CameraDepthAttachment;
        RenderTargetHandle m_Destination;

        const string k_RenderPostProcessingTag = "Render RayMarchingProcessing Effects";
        const string k_RenderFinalPostProcessingTag = "Render Final RayMarchingProcessing Pass";

        MaterialLibrary m_Materials;
        RayMarchingProcessData m_Data;
        ScriptableRenderer m_renderer;
        RenderingData m_renderingData;

        RenderTargetHandle m_TemporaryColorTexture01;
        public RayMarchingProcessPass()
        {
            m_TemporaryColorTexture01.Init("_TemporaryMyTestTexture1");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get(k_RenderPostProcessingTag);
            Render(cmd, ref renderingData);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Setup(RenderPassEvent @event, RenderTargetIdentifier source, RenderTargetHandle destination, RayMarchingProcessData data)
        {
            m_Data = data;
            renderPassEvent = @event;
            m_ColorAttachment = source;
            m_Destination = destination;
            m_Materials = new MaterialLibrary(data);
        }

        void Render(CommandBuffer cmd,ref RenderingData renderingData)
        {
            ref var cameraData = ref renderingData.cameraData;
            DrawSimpleSphere(cmd, ref renderingData, m_Materials.rayMarching);
        }

        void DrawSimpleSphere(CommandBuffer cmd, ref RenderingData renderingData, Material rayMaterial)
        {
            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.width = opaqueDesc.width >> 1;
            opaqueDesc.height = opaqueDesc.height >> 1;
            cmd.GetTemporaryRT(m_TemporaryColorTexture01.id, opaqueDesc, FilterMode.Bilinear);
            cmd.BeginSample("RayMarching");
            cmd.Blit(this.m_ColorAttachment, m_TemporaryColorTexture01.Identifier());
            cmd.Blit(m_TemporaryColorTexture01.Identifier(), this.m_ColorAttachment, rayMaterial);

            cmd.Blit(m_TemporaryColorTexture01.Identifier(), RenderTargetHandle.CameraTarget.Identifier());
            cmd.EndSample("RayMarching");
        }
    }
}
