using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Rendering.Universal.RayMarching
{
    public class MaterialLibrary
    {
        public readonly Material rayMarching;

        public MaterialLibrary(RayMarchingProcessData data)
        {
            rayMarching = Load(Shader.Find("Custom/RayMarching/shadertoy"));
        }

        Material Load(Shader shader)
        {
            if (shader == null)
            {
                Debug.LogErrorFormat($"Missing shader. {GetType().DeclaringType.Name} render pass will not execute. Check for missing reference in the renderer resources.");
                return null;
            }
            else if (!shader.isSupported)
            {
                return null;
            }

            return CoreUtils.CreateEngineMaterial(shader);

        }
        internal void Cleanup()
        {

        }
    } 
}
