using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Experiemntal.Rendering.Universal
{
    public class AdditionPostProcessRendererFeature : ScriptableRendererFeature
    {
        public RenderPassEvent evt = RenderPassEvent.AfterRenderingTransparents;
        public AdditionalPostProcessData postData;
        AdditionPostProcessPass postPass;

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            var cameraColorTarget = renderer.cameraColorTarget;
            var cameraDepth = renderer.cameraDepth;
            var dest = RenderTargetHandle.CameraTarget;
            if (postData == null)
                return;
            postPass.Setup(evt, cameraColorTarget, cameraDepth, dest, postData);
            renderer.EnqueuePass(postPass);
        }

        public override void Create()
        {
            postPass = new AdditionPostProcessPass();
        }
    }
}
