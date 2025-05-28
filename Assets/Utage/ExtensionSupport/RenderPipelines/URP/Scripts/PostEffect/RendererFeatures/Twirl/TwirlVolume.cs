#if UTAGE_URP
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Utage.RenderPipeline.Urp
{
    //Twirl用のVolumeComponent
    [Serializable,VolumeComponentMenu("Utage/Twirl Volume")]
    public class TwirlVolume : VolumeComponent, IPostProcessComponent
    {
        public float Angle => angle.value;
        [SerializeField] FloatParameter angle = new(0);
        
        public Vector2 Radius => radius.value;
        [SerializeField] NoInterpVector2Parameter radius = new (new Vector2(0.3f,0.3f));
        
        public Vector2 Center => center.value;
        [SerializeField] NoInterpVector2Parameter center = new (new Vector2(0.5f,0.5f));
        
        public bool IsActive()
        {
            return Angle > 0 || Angle < 0;
        }

        public bool IsTileCompatible()
        {
            return false;
        }
    }
}
#endif
