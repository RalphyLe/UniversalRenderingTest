using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
#endif
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.IO;

namespace UnityEngine.Experiemntal.Rendering.Universal
{
    [Serializable]
    public class AdditionalPostProcessData : ScriptableObject
    {
        public const string root = "D:/unityproject/urp test/Assets";
#if UNITY_EDITOR
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812")]

        [MenuItem("Assets/Create/Rendering/Universal Render Pipeline/Additional Post-process Data", priority = CoreUtils.assetCreateMenuPriority3 + 1)]
        static void CreateAdditionalPostProcessData()
        {
            //ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreatePostProcessDataAsset>(), "CustomPostProcessData.asset", null, null);
            var instance = CreateInstance<AdditionalPostProcessData>();
            AssetDatabase.CreateAsset(instance, string.Format("Assets/Settings/{0}.asset", typeof(AdditionalPostProcessData).Name));
            Selection.activeObject = instance;
        }
#endif
        [Serializable, ReloadGroup]
        public sealed class ShaderResources
        {
            [Reload(root + "/Shader/GaussianBlur.shader",ReloadAttribute.Package.Builtin)]
            public Shader gaussianBlur;
        }
        public ShaderResources shaders;
    }
}
