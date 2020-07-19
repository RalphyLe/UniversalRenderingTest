﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

namespace UnityEngine.Experiemntal.Rendering.Universal
{
    public enum GaussianBlurTarget
    {
        Color,
        Texture
    }

    [Serializable,VolumeComponentMenu("Addition-post-processing/GaussianBlur")]
    public class GaussianBlur : VolumeComponent, IPostProcessComponent
    {
        public GaussianBlurTargetParameter destination = new GaussianBlurTargetParameter(GaussianBlurTarget.Color);
        public GaussianFilerModeParameter filterMode = new GaussianFilerModeParameter(FilterMode.Bilinear);
        public ClampedIntParameter blurCount = new ClampedIntParameter(1, 1, 4);
        public ClampedIntParameter downSample = new ClampedIntParameter(1, 1, 4);
        public ClampedFloatParameter indensity = new ClampedFloatParameter(0f, 0, 20);
        //public 

        public bool IsActive()
        {
            return active && indensity.value != 0;
        }

        public bool IsTileCompatible()
        {
            return false;
        }


    }

    [Serializable]
    public sealed class GaussianBlurTargetParameter : VolumeParameter<GaussianBlurTarget> { public GaussianBlurTargetParameter(GaussianBlurTarget value, bool overrideState = false) : base(value, overrideState) { } }

    [Serializable]
    public sealed class GaussianFilerModeParameter : VolumeParameter<FilterMode> { public GaussianFilerModeParameter(FilterMode value, bool overrideState = false) : base(value, overrideState) { } }
}
