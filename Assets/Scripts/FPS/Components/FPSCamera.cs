using System.Linq;
using UnityEngine;

namespace FPS
{
    [AddComponentMenu("FPS/FPSCamera")]
    [RequireComponent(typeof(FPSCharacter))]
    public class FPSCamera : Motion
    {
        [SerializeField] private Transform body;
        [SerializeField, Tooltip("The object to rotate!")] private Transform camParent;
        [SerializeField, Tooltip("The object to apply motion effect on")] private Transform motionHandler;
        [SerializeField, Tooltip("The player main camera")] private Camera cam;
        [SerializeField, Range(0, 90f)] private float clampAngle;
        [SerializeField, Range(20, 120f)] private float fov = 70f;
        [SerializeField, Range(0, 2)] private float fovMultiplier = 1;
        [SerializeField, Range(0, 2)] private float runMultiplier = 1;
        [SerializeField] private float sensitivity = 2f;
        
        private FPSCharacter character;
        private Vector2 lookInput;

        private static readonly int MaxRotCache = 3;
        private float[] rotArrayHor = new float[MaxRotCache];
        private float[] rotArrayVert = new float[MaxRotCache];
        private int rotCacheIndex;
        
        private float xRot;

        private void Start()
        {
            body = transform;
            character = GetComponent<FPSCharacter>();
            if (!character)
            {
                Debug.LogError($"No FPSCharacter found on the Movement GameObject {name}");
                enabled = false;
            }
            else
            {
                character.OnInputUpdated += OnInputUpdated;
            }

            if (!cam)
            {
                if (!camParent.TryGetComponent(out cam))
                {
                    cam = GetComponent<Camera>();
                }
            }
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public override void Tick()
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov * fovMultiplier, Time.deltaTime * 10f) * runMultiplier;
            
            // Normal camera Movement
            xRot -= (lookInput.y) * Time.deltaTime;
            xRot = Mathf.Clamp(xRot, -clampAngle, clampAngle);
            
            body.Rotate(Vector3.up * (lookInput.x * Time.deltaTime));
        }
        
        private void OnInputUpdated(ref PlayerInput input)
        {
            // Smoothing the input using the average frame solution.
            float x = GetAverageHorizontal(input.LookDir.x);
            float y = GetAverageVertical(input.LookDir.y);
            IncreaseRotCacheIndex();
            
            lookInput = new Vector2(x, y) * sensitivity;
        }

        private float GetAverageHorizontal(float h)
        {
            rotArrayHor[rotCacheIndex] = h;
            return rotArrayHor.Average();
        }
        
        private float GetAverageVertical(float v)
        {
            rotArrayVert[rotCacheIndex] = v;
            return rotArrayVert.Average();
        }

        private void IncreaseRotCacheIndex()
        {
            rotCacheIndex++;
            rotCacheIndex %= MaxRotCache;
        }
        
        public float GetRotProgress() => xRot / clampAngle;

        public Camera GetCamera() => cam;
        
        public Transform GetCameraParent() => camParent;
        public Transform GetCameraMotionHandler() => motionHandler;

        public void SetFOVMultiplier(float m) => fovMultiplier = m;

        public override Vector3 GetLocation()
        {
            return new Vector3(0, 0, 0);
        }

        public override Vector3 GetEulerAngles()
        {
            return new Vector3(xRot, 0f, 0f);
        }
    }
}