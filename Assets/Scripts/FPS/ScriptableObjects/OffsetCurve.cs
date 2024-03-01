using UnityEngine;

namespace FPS
{
    public enum AnimState
    {
        Idle,
        Walk,
        Run,
        Aim
    }
    
    [CreateAssetMenu(fileName = "Cam Offset", menuName = "Snowy/FPS/Camera Offset Curve")]
    public class OffsetCurve : ScriptableObject
    {
        public Vector3 defaultPos;
        public Vector3 offsetPos;
        
        public Vector3 defaultRot;
        public Vector3 offsetRot;

        public AnimationCurve lerpCurve;

        public Vector3 GetPosition(float t)
        {
            lerpCurve.preWrapMode = WrapMode.Clamp;
            lerpCurve.postWrapMode = WrapMode.Clamp;
            
            t = lerpCurve.Evaluate(t);
            return Vector3.Lerp(defaultPos, offsetPos, t);
        }
        
        public Vector3 GetEulerAngles(float t)
        {
            lerpCurve.preWrapMode = WrapMode.Clamp;
            lerpCurve.postWrapMode = WrapMode.Clamp;
            
            t = lerpCurve.Evaluate(t);
            return Vector3.Lerp(defaultRot, offsetRot, t);
        }
    }
    
}