using UnityEngine.Rendering;

using UnityEngine.Rendering.Universal;



namespace UnityEngine.Experiemntal.Rendering.Universal

{

    public class Blur : ScriptableRendererFeature

    {

        [System.Serializable]

        public class BlurSettings

        {

            public RenderPassEvent Event = RenderPassEvent.AfterRenderingTransparents;



            public Material blurMaterial = null;

            public int blurMaterialPassIndex = -1;



            [Range(1, 4)]

            public int blurCount = 1;

            [Range(1, 4)]

            public int downSample = 2;

            public Target destination = Target.Color;

            [Range(0f, 20f)]

            public float indensity;

            public string textureId = "_BlurPassTexture";

        }



        public enum Target

        {

            Color,

            Texture

        }



        public BlurSettings settings = new BlurSettings();

        RenderTargetHandle m_RenderTextureHandle;



        BlurPass blurPass;



        public override void Create()

        {

            var passIndex = settings.blurMaterial != null ? settings.blurMaterial.passCount - 1 : 1;

            settings.blurMaterialPassIndex = Mathf.Clamp(settings.blurMaterialPassIndex, -1, passIndex);

            blurPass = new BlurPass(settings.Event, settings.blurMaterial, settings.blurMaterialPassIndex, name, settings.downSample, settings.blurCount, settings.indensity);

            m_RenderTextureHandle.Init(settings.textureId);

        }



        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)

        {

            var src = renderer.cameraColorTarget;

            var dest = (settings.destination == Target.Color) ? RenderTargetHandle.CameraTarget : m_RenderTextureHandle;



            if (settings.blurMaterial == null)

            {

                Debug.LogWarningFormat("Missing Blur Material. {0} blur pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);

                return;

            }



            blurPass.Setup(src, dest);

            renderer.EnqueuePass(blurPass);

        }

    }
}