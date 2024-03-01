
using UnityEngine;

namespace FPS
{
    [CreateAssetMenu(fileName = "Motion Offset Set", menuName = "Snowy/FPS/Motion Camera Offset Curves")]
    public class MotionOffsetCurveSet : ScriptableObject
    {
        [InLineEditor] public OffsetCurve idleCurve, walkCurve, runCurve, aimCurve;

        public OffsetCurve GetCurve(AnimState state)
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
    }
}