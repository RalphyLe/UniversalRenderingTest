using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Rendering.Universal.RayMarching
{
    public class RayMarchingProcessRendererFeature : ScriptableRendererFeature
    {
        public RenderPassEvent evt = RenderPassEvent.AfterRenderingOpaques;
        public RayMarchingProcessData postData;
        RayMarchingProcessPass postPass;

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            var cameraColorTarget = renderer.cameraColorTarget;
            var cameraDepth = renderer.cameraDepth;
            var cameraData = renderingData.cameraData;
            var dest = RenderTargetHandle.CameraTarget;
            if (postData == null)
                return;
            postPass.Setup(evt, cameraColorTarget, dest, postData);
            renderer.EnqueuePass(postPass);
        }

        public override void Create()
        {
            postPass = new RayMarchingProcessPass();
        }
    }
}
