﻿using System.Collections;
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
        public string textureId = "_BlurPassTexture";
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            var src = renderer.cameraColorTarget;
            var dest = RenderTargetHandle.CameraTarget;
            if (postData == null)
                return;
            postPass.Setup(src,postData, dest);
            renderer.EnqueuePass(postPass);
        }

        public override void Create()
        {
            postPass = new AdditionPostProcessPass(evt);
        }
    }
}
