using UnityEngine;

namespace FPS
{
    [CreateAssetMenu(fileName = "BobCurves", menuName = "Snowy/FPS/BobCurves", order = 0)]
    public class BobCurves : ScriptableObject
    {
        [Header("Vertical Bobbing Curves")] public AnimationCurve idleCurve;
        public AnimationCurve walkCurve, runCurve, aimCurve;

        [Header("Horizontal Bobbing Curves")] public AnimationCurve idleCurveH;
        public AnimationCurve walkCurveH, runCurveH, aimCurveH;
        
        public AnimationCurve GetCurveVertical(AnimState state)
        {
            switch (state)
            {
                case AnimState.Idle:
                default:
                    return idleCurve;
                
                case AnimState.Walk:
                    return walkCurve;
                
                case AnimState.Run:
                    return runCurve;
                
                case AnimState.Aim:
                    return aimCurve;
            }
        }
        
        public AnimationCurve GetCurveHorizontal(AnimState state)
        {
            switch (state)
            {
                case AnimState.Idle:
                default:
                    return idleCurveH;
                
                case AnimState.Walk:
                    return walkCurveH;
                
                case AnimState.Run:
                    return runCurveH;
                
                case AnimState.Aim:
                    return aimCurveH;
            }
        }
    }
}