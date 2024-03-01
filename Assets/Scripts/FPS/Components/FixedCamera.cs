using System;
using System.Collections;
using UnityEngine;

namespace FPS
{
    
    [RequireComponent(typeof(FPSCharacter))]    
    public class FixedCamera : MonoBehaviour
    {
        [SerializeField] private Transform mainCam;
        [SerializeField] private Camera fixedCam;
        [SerializeField] private GameObject[] visuals;
        
        private FPSCharacter _fpsCharacter;
        bool _isFixed;

        private void Start()
        {
            if (fixedCam) fixedCam.gameObject.SetActive(false);
            _fpsCharacter = GetComponent<FPSCharacter>();
        }
        
        public void SetFixedCamera(bool state)
        {
            if (fixedCam)
            {
                fixedCam.gameObject.SetActive(state);
                mainCam.gameObject.SetActive(!state);
                
                _fpsCharacter.SetImmobilized(state);
                _fpsCharacter.SetCameraLocked(state);
                
                SetVisuals(!state);
                
                _isFixed = state;
            }
        }
        
        public void SetFixedCamera(bool state, Transform target, float time)
        {
            StartCoroutine(LerpToFixedCamera(target, time, state, state));
        }
        
        public void SetFixedCameraInstant(bool state, Transform target)
        {
            _isFixed = state;
            SetFixedCamera(state);
            
            fixedCam.transform.position = target.position;
            fixedCam.transform.rotation = target.rotation;
        }
        
        public void SetVisuals(bool state)
        {
            foreach (var visual in visuals)
            {
                visual.SetActive(state);
            }
        }
        
        IEnumerator LerpToFixedCamera(Transform target, float time, bool state, bool before = false)
        {
            if (before)
            {
                _isFixed = state;
                SetFixedCamera(state);
            }
            
            var startPos = mainCam.position;
            var startRot = mainCam.rotation;
            var endPos = target.position;
            var endRot = target.rotation;
            var t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / time;

                fixedCam.transform.position = Vector3.Lerp(startPos, endPos, t);
                fixedCam.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
                yield return null;
            }
            
            SetFixedCamera(state);
            _isFixed = state;
        }
        
        public void GoBackToMainCamera()
        {
            SetFixedCameraInstant(false, mainCam);
        }

        public bool IsFixed() => _isFixed;

        public Camera GetFixedCamera() => fixedCam;
    }
}