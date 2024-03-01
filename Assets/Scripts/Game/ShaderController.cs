using UnityEngine;

namespace Game
{
    
    [ExecuteInEditMode]
    public class ShaderController: MonoBehaviour
    {
        [Header("Invisible shader")]
        [SerializeField] private float radius = 1f;
        [SerializeField] private Transform visualSphere;
        
        [Header("Glitch effect")]
        [SerializeField] float maxGlitch = 0.1f;
        
        float nextGlitch;
        private static readonly int glitchStrength = Shader.PropertyToID("_GlitchStrength");
        
        private void LateUpdate()
        {
            // Glitching
            if (Time.time > nextGlitch)
            {
                nextGlitch = Time.time + UnityEngine.Random.Range(0.1f, 0.3f);
                Shader.SetGlobalFloat(glitchStrength, UnityEngine.Random.Range(0, maxGlitch));
            }
            
            Shader.SetGlobalFloat("_InvisibleRadius", radius);
            Shader.SetGlobalVector("_InvisiblePos", visualSphere.position);
            
            if (visualSphere) visualSphere.localScale = Vector3.one * radius * 2;
        }
        
        public void SetInvinsibleRadius(float newRadius)
        {
            radius = newRadius;
        }
        
        public Transform GetVisualSphere()
        {
            return visualSphere;
        }
    }
}