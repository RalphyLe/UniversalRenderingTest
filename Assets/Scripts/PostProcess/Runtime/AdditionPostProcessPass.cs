using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Experiemntal.Rendering.Universal
{
    /// <summary>
    /// 对URP屏幕后处理扩展
    /// </summary>
    public class AdditionPostProcessPass : ScriptableRenderPass
    {
        RenderTextureDescriptor m_Descriptor;
        RenderTargetHandle m_Source;
        RenderTargetIdentifier m_Identifier;
        RenderTargetHandle m_Destination;
        RenderTargetHandle m_Depth;
        RenderTargetHandle m_InternalLut;

        const string k_RenderPostProcessingTag = "Render AdditionalPostProcessing Effects";
        const string k_RenderFinalPostProcessingTag = "Render Final AdditionalPostProcessing Pass";

        //additonal effects settings
        GaussianBlur m_GaussianBlur;

        MaterialLibrary m_Materials;
        AdditionalPostProcessData m_Data;

        RenderTargetHandle m_TemporaryColorTexture01;

        RenderTargetHandle m_TemporaryColorTexture02;

        RenderTargetHandle m_TemporaryColorTexture03;

        public AdditionPostProcessPass(RenderPassEvent evt)
        {
            renderPassEvent = evt;

            m_TemporaryColorTexture01.Init("_TemporaryColorTexture1");

            m_TemporaryColorTexture02.Init("_TemporaryColorTexture2");

            m_TemporaryColorTexture03.Init("_TemporaryColorTexture3");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var stack = VolumeManager.instance.stack;
            m_GaussianBlur = stack.GetComponent<GaussianBlur>();
            var cmd = CommandBufferPool.Get(k_RenderPostProcessingTag);
            Render(cmd, ref renderingData);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Setup(RenderTargetIdentifier source, AdditionalPostProcessData data, RenderTargetHandle destination)
        {
            m_Data = data;
            m_Materials = new MaterialLibrary(data);
            this.m_Identifier = source;
            this.m_Destination = destination;
        }

        void Render(CommandBuffer cmd,ref RenderingData renderingData)
        {
            int width = renderingData.cameraData.cameraTargetDescriptor.width;
            int height = renderingData.cameraData.cameraTargetDescriptor.height;
            ref var cameraData = ref renderingData.cameraData;
            if (m_GaussianBlur.IsActive() && !cameraData.isSceneViewCamera)
            {
                Debug.Log("高斯模糊");
                SetupGaussianBlur(cmd, ref renderingData, m_Materials.gaussianBlur);
            }
        }

        public void SetupGaussianBlur(CommandBuffer cmd, ref RenderingData renderingData, Material blurMaterial)
        {
            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.width = opaqueDesc.width >> m_GaussianBlur.downSample.value;
            opaqueDesc.height = opaqueDesc.height >> m_GaussianBlur.downSample.value;
            opaqueDesc.depthBufferBits = 0;
            cmd.GetTemporaryRT(m_TemporaryColorTexture01.id, opaqueDesc, m_GaussianBlur.filterMode.value);
            cmd.GetTemporaryRT(m_TemporaryColorTexture02.id, opaqueDesc, m_GaussianBlur.filterMode.value);
            cmd.GetTemporaryRT(m_TemporaryColorTexture03.id, opaqueDesc, m_GaussianBlur.filterMode.value);
            cmd.BeginSample("Blur");
            cmd.Blit(this.m_Identifier, m_TemporaryColorTexture03.Identifier());
            for (int i = 0; i < m_GaussianBlur.blurCount.value; i++)
            {
                blurMaterial.SetVector("_offsets", new Vector4(0, m_GaussianBlur.indensity.value, 0, 0));
                cmd.Blit(m_TemporaryColorTexture03.Identifier(), m_TemporaryColorTexture01.Identifier(), blurMaterial);
                blurMaterial.SetVector("_offsets", new Vector4(m_GaussianBlur.indensity.value, 0, 0, 0));
                cmd.Blit(m_TemporaryColorTexture01.Identifier(), m_TemporaryColorTexture02.Identifier(), blurMaterial);
                cmd.Blit(m_TemporaryColorTexture02.Identifier(), m_TemporaryColorTexture03.Identifier());
            }
            cmd.Blit(m_TemporaryColorTexture03.Identifier(), this.m_Identifier);
            cmd.EndSample("Blur");
        }
    }
}
