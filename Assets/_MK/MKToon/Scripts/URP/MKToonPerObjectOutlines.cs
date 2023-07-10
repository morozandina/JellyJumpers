#if MK_URP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MK.Toon.URP
{
    public class MKToonPerObjectOutlines : UnityEngine.Experimental.Rendering.Universal.RenderObjects
    {
        private const string _componentName = "MKToonPerObjectOutlines";
        private const string _shaderPassName = "MKToonOutline";

        public LayerMask _layerMask = -1;

        public override void Create()
        {
            settings.passTag = _componentName;
            name = _componentName;
            settings.filterSettings.LayerMask = _layerMask;
            settings.filterSettings.PassNames = new string[1] { _shaderPassName };
            base.Create();
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            settings.filterSettings.LayerMask = _layerMask;
            base.AddRenderPasses(renderer, ref renderingData);
        }
    }
}
#endif