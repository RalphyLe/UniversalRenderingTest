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
        public const string root = "D:/unityproject/urp test/Assets";
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
        [Serializable, ReloadGroup]
        public sealed class ShaderResources
        {
            [Reload(root + "/Shader/RayMarching/shadertoy.shader", ReloadAttribute.Package.Builtin)]
            public Shader raymarch;
        }

        public ShaderResources shaders;
    }
}
