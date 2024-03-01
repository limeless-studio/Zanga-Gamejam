using System;

using Inventory;
using UnityEngine;

namespace FPS
{
    public class OffsetMotion : Motion
    {
        [SerializeField, Min(0.1f)] private float offsetChangeSpeed = 10f;
        [SerializeField, InLineEditor]
        private MotionOffsetCurveSet defaultCurvesSet;
        [SerializeField, Disable]
        private OffsetCurve currentOffsetCurve;
        
        [SerializeField, Disable]
        private MotionOffsetCurveSet currentCurvesSet;
        
        private FPSCamera fpsCamera;
        private FPSAnimator animator;
        private FPSCharacter character;

        private Vector3 pos;
        private Vector3 rot;

        private void Start()
        {
            currentCurvesSet = defaultCurvesSet;
            currentOffsetCurve = currentCurvesSet.idleCurve;

            fpsCamera = motionApplier.GetCharacter().FPSCamera;
            animator = motionApplier.GetCharacter().FPSAnimator;
            character = motionApplier.GetCharacter();
        }

        public override void Tick()
        {
            // ignore
            if (fpsCamera == null)
            {
                fpsCamera = motionApplier.GetCharacter().FPSCamera;
                return;
            }
            
            if (animator == null) animator = motionApplier.GetCharacter().FPSAnimator;
            else UpdateCurve(animator.GetCurrentState());
            
            pos = Vector3.Lerp(pos, currentOffsetCurve.GetPosition(fpsCamera.GetRotProgress()), Time.deltaTime * offsetChangeSpeed);
            rot = Vector3.Lerp(rot, currentOffsetCurve.GetEulerAngles(fpsCamera.GetRotProgress()), Time.deltaTime * offsetChangeSpeed);
        }
        
        public void SetOffsetCurveSet(MotionOffsetCurveSet set) => currentCurvesSet = set;

        public void ResetOffsetCurvesSet() => currentCurvesSet = defaultCurvesSet;

        public void UpdateCurve(AnimState state)
        {
            OffsetCurve curve = currentCurvesSet.GetCurve(state);
            if (curve) currentOffsetCurve = curve;
        }

        public override Vector3 GetLocation() => pos;

        public override Vector3 GetEulerAngles() => rot;
    }
}