using Core;
using Game;
using UI;
using UnityEngine;

namespace FPS
{
    [AddComponentMenu("FPS/FPSCharacter")]
    public class FPSCharacter : Actor
    {
        public delegate void OnInputUpdatedD(ref PlayerInput input);
        public OnInputUpdatedD OnInputUpdated;

        private FPSMovement fpsMovement;
        private FPSCamera fpsCamera;
        private FixedCamera fixedCamera;
        private FPSAnimator fpsAnimator;
        private FPSInventory fpsInventory;
        private FPSMotionApplier fpsMotionApplier;
        private InterfaceManager interfaceManager;
        
        PlayerInput input;
        
        public FPSMovement FPSMovement
        {
            get => fpsMovement;
            set => fpsMovement = value;
        }

        public FPSCamera FPSCamera
        {
            get => fpsCamera;
            set => fpsCamera = value;
        }
        
        public FixedCamera FixedCamera
        {
            get => fixedCamera;
            set => fixedCamera = value;
        }
        
        public FPSAnimator FPSAnimator
        {
            get => fpsAnimator;
            set => fpsAnimator = value;
        }
        
        public FPSInventory FPSInventory
        {
            get
            {
                if (fpsInventory == null) fpsInventory = GetComponent<FPSInventory>();
                return fpsInventory;
            }
            set => fpsInventory = value;
        }
        
        public FPSMotionApplier FPSMotionApplier
        {
            get => fpsMotionApplier;
            set => fpsMotionApplier = value;
        }
        
        private bool _isImmobilized;
        private bool _isCameraLocked;
        
        protected override void Awake()
        {
            SetUpComponents();
            base.Awake();
        }

        public void SetUpComponents()
        {
            fpsMovement = GetComponent<FPSMovement>();
            fpsCamera = GetComponent<FPSCamera>();
            fixedCamera = GetComponent<FixedCamera>();
            fpsAnimator = GetComponent<FPSAnimator>();
            fpsInventory = GetComponent<FPSInventory>();
            fpsMotionApplier = GetComponent<FPSMotionApplier>();
            interfaceManager = GetComponent<InterfaceManager>();

            if (GameManager.Instance)
            {
                GameManager.Instance.SetPlayer(this);
            }
        }

        private void Update()
        {
            UpdateInput();
        }
        
        private void UpdateInput()
        {
            // todo: Update input
            if (InputManager.Instance)
            {
                input.MoveDir = _isImmobilized ? Vector2.zero : new Vector2(InputManager.Instance.Horizontal, InputManager.Instance.Vertical);
                input.LookDir = _isCameraLocked ? Vector2.zero : InputManager.Instance.Look;

                input.Jump = !_isImmobilized && InputManager.Instance.IsJump;
                input.Sprint = !_isImmobilized && InputManager.Instance.IsSprint;
                input.Crouch = !_isImmobilized && InputManager.Instance.IsCrouch;
                input.CrouchDown = !_isImmobilized && InputManager.Instance.Crouch == ButtonState.Pressed;
                input.Attack = _isCameraLocked ? ButtonState.None : InputManager.Instance.Attack;
                input.Aim = _isCameraLocked ? ButtonState.None : InputManager.Instance.Aim;
                input.MouseWheel = _isCameraLocked ? 0 : InputManager.Instance.MouseWheel;
                input.Interact = _isCameraLocked ? ButtonState.None : InputManager.Instance.Interact;
                input.Reload = _isCameraLocked ? ButtonState.None : InputManager.Instance.Reload;
                input.Inventory = InputManager.Instance.Inventory;
            }
            
            OnInputUpdated?.Invoke(ref input);
        }


        #region API

        public void SetImmobilized(bool state)
        {
            _isImmobilized = state;
        }
        
        public bool IsImmobilized() => _isImmobilized;
        
        public void SetCameraLocked(bool state)
        {
            _isCameraLocked = state;
            
            Cursor.visible = state;
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        }
        
        public bool IsCameraLocked() => _isCameraLocked;
        
        public bool IsMoving() => input.MoveDir != Vector2.zero;
        
        public bool IsAiming() => (input.Aim == ButtonState.Held || input.Aim == ButtonState.Pressed) &&
                                  fpsInventory != null && fpsInventory.CurrentInHand != null && fpsInventory.CurrentInHand.IsGun;
        public bool IsCrouching() => input.Crouch;
        
        public bool IsGrounded() => fpsMovement.IsGrounded;
        
        public Vector2 GetMovementInput() => input.MoveDir;
        
        public void SetGravityMultiplier(float multiplier)
        {
            if (fpsMovement) fpsMovement.SetGravityMultiplier(multiplier);
        }
        
        public void ResetGravityMultiplier()
        {
            if (fpsMovement) fpsMovement.ResetGravityMultiplier();
        }

        public Vector3 GetVelocity() => fpsMovement ? fpsMovement.GetVelocity() : Vector3.zero;
        
        public bool IsSprinting() => input.Sprint;

        public void TriggerHands(bool state)
        {
            if (fpsAnimator == null) return;
            fpsAnimator.SetHandsActive(state);
        }

        #endregion

    }
}