using UnityEngine;

namespace FPS
{
    public class BobMotion : Motion
    {
        [SerializeField] private float speed = 1f;
        [SerializeField, InLineEditor] private BobCurves bobCurves;

        private Vector3 m_position = Vector3.zero;
        
        public override void Tick()
        {
            if (motionApplier.GetCharacter().IsGrounded())
            {
                AnimState animState = motionApplier.GetCharacter().FPSAnimator
                    ? motionApplier.GetCharacter().FPSAnimator.GetCurrentState()
                    : AnimState.Idle;
                m_position.y = Mathf.Lerp(m_position.y, bobCurves.GetCurveVertical(animState).Evaluate(Time.time * speed),
                    Time.deltaTime * 10);
                
                m_position.x = Mathf.Lerp(m_position.x, bobCurves.GetCurveHorizontal(animState).Evaluate(Time.time * speed),
                    Time.deltaTime * 10);
            }else
            {
                m_position.y = 0;
                m_position.x = 0;
            }
        }

        public override Vector3 GetLocation() => m_position;

        public override Vector3 GetEulerAngles() => Vector3.zero;
    }
}