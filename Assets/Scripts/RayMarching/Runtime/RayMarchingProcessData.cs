using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
#endif
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace UnityEngine.Rendering.Universal.RayMarching
{
    [Serializable]
    public class RayMarchingProcessData : ScriptableObject
    {

#if UNITY_EDITOR
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812")]

        [MenuItem("Assets/Create/Rendering/Universal Render Pipeline/Raymarching Process Data", priority = CoreUtils.assetCreateMenuPriority3 + 1)]
        static void CreateRayMarchingProcessData()
        {
            //ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreatePostProcessDataAsset>(), "CustomPostProcessData.asset", null, null);
            var instance = CreateInstance<RayMarchingProcessData>();
            AssetDatabase.CreateAsset(instance, string.Format("Assets/Settings/{0}.asset", typeof(RayMarchingProcessData).Name));
            Selection.activeObject = instance;
        }
#endif
        [Serializable]
        public sealed class Shaders
        {
            public Shader raymarch = Shader.Find("Custom/RayMarching/shadertoy");
        }
        public Shaders shaders;
    }
}
