using System;
using System.Collections.Generic;
using System.Linq;

using Inventory;
using UnityEngine;

namespace FPS
{
    public enum LayerTag
    {
        Base,
        Overlay,
        Actions
    }

    [Serializable] public class AnimLayer
    {
        public LayerTag tag;
        public int fpsIndex;
        public int tpsIndex;
    }
    
    [AddComponentMenu("FPS/FPSAnimator")]
    [RequireComponent(typeof(FPSCharacter))]
    
    public class FPSAnimator : MonoBehaviour
    {
        [SerializeField] public List<AnimLayer> animLayers;

        [SerializeField, Range(1, 100)]
        public float smoothness = 0.2f;
        
        [SerializeField, Required("Required field")]
        private Animator handsAnimator;

        [SerializeField, Required("Required field")]
        private Animator bodyAnimator;

        [SerializeField, Required("Required field")]
        private RuntimeAnimatorController defaultHandsController;

        [SerializeField, Required("Required field")]
        private RuntimeAnimatorController defaultBodyController;
        
        private FPSCharacter character;
        private Dictionary<string, string> currentStates = new Dictionary<string, string>();
        
        private AnimState currentState;

        private Animator weaponAnimator;
        
        private float aiming = 0f;
        private Vector2 moveDir = Vector2.zero;
        private float speed = 0f;
        
        private bool CheckErrors()
        {
            string missing = "";
            if (bodyAnimator == null) missing = nameof(bodyAnimator);
            if (handsAnimator == null) missing = nameof(handsAnimator);
            if (defaultHandsController == null) missing = nameof(defaultHandsController);
            if (defaultBodyController == null) missing = nameof(defaultBodyController);
            
            if (!TryGetComponent(out character))
            {
                Debug.LogError("FPSAnimator Requires FPSCharacter class to function.");
                return true;
            }


            if (missing != "")
            {
                Debug.LogError($"FPSAnimator missing param {missing}, disabled!");
                return true;
            }

            return false;
        }
        

        private void Start()
        {
            if (CheckErrors())
            {
                enabled = false;
                return;
            }

            // todo: set up
            SetHandsController(defaultHandsController);
            SetBodyController(defaultBodyController);
        }

        private void Update()
        {
            // TODO: Update animations based on character speed etc...
            currentState = AnimState.Idle;
            if (character.IsMoving()) currentState = character.IsSprinting() ? AnimState.Run : AnimState.Walk;
            
            // Update Animations values
            Vector2 inputMove = character.GetMovementInput();
            aiming = Mathf.Lerp(aiming, character.IsAiming() ? 1f : 0f, Time.deltaTime * smoothness);
            moveDir = Vector2.Lerp(moveDir, inputMove, Time.deltaTime * smoothness);
            speed = Mathf.Lerp(speed, character.IsSprinting() ? 1f : 0f, Time.deltaTime * smoothness);
            
            SetAllFloat(AnimatorHashes.Movement, moveDir.magnitude);
            SetAllFloat(AnimatorHashes.Horizontal, moveDir.x);
            SetAllFloat(AnimatorHashes.Vertical, moveDir.y);
            SetAllFloat(AnimatorHashes.Aim, aiming);
            SetAllFloat(AnimatorHashes.Speed, speed);

            if (weaponAnimator && handsAnimator.gameObject.activeSelf)
            {
                weaponAnimator.SetFloat(AnimatorHashes.Aim, aiming);
            }
        }
        

        private void PlayBodyAnimation(string targetClip, float transitionTime = 0.3f, int layer = 0)
        {
            if (bodyAnimator.gameObject.activeSelf) CrossFadeAnimation(bodyAnimator, targetClip, transitionTime, layer);
        }
        
        private void PlayHandsAnimation(string targetClip, float transitionTime = 0.3f, int layer = 0)
        {
            if (handsAnimator.gameObject.activeSelf) CrossFadeAnimation(handsAnimator, targetClip, transitionTime, layer);
        }

        private void CrossFadeAnimation(Animator anim, string targetClip, float transitionTime = 0.3f, int layer = 0, bool isInstant = true)
        {
            AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(layer);
            string hash = $"{anim.name}-{layer}";
            
            if (currentStates.TryGetValue(hash, out string clip))
            {
                if (clip == targetClip) return;
                currentStates[hash] = targetClip;
            }
            else currentStates.Add(hash, targetClip);

            if (isInstant) anim.CrossFade(targetClip, transitionTime, layer, layer);
            
            else anim.CrossFade(targetClip, transitionTime, layer, animState.normalizedTime);
        }

        #region API Stuff
        
        public void PlayAnimation(string targetClip, float transitionTime = 0.3f, int layer = 0, bool updateCam = false)
        {
            PlayHandsAnimation(targetClip, transitionTime / 10f, layer);
            
            PlayBodyAnimation(targetClip, transitionTime, layer);
        }

        public void PlayActionAnimation(string targetClip, float transitionTime = 0.3f)
        {
            PlayHandsAnimation(targetClip, transitionTime / 10f, 3);
            
            PlayBodyAnimation(targetClip, transitionTime, 2);
        }
        
        public void PlayActionInstant(string targetClip, bool updateCam = false)
        {
            if (bodyAnimator.layerCount > 3) bodyAnimator.Play(targetClip, 3, 0.0f);
            if (handsAnimator.layerCount > 2) handsAnimator.Play(targetClip, 2, 0.0f);
        }
        
        public void PlayAnimationInstant(string targetClip, LayerTag layer = LayerTag.Actions, bool updateCam = false)
        {
            AnimLayer animLayer = animLayers.FirstOrDefault(x => x.tag == layer);
            if (animLayer == null)
            {
                Debug.LogWarning($"No Animation Layer set up with tag {layer.ToString()}");
                return;
            }
            
            if (bodyAnimator.layerCount > animLayer.tpsIndex) bodyAnimator.Play(targetClip, animLayer.tpsIndex, 0.0f);
            if (handsAnimator.layerCount > animLayer.fpsIndex) handsAnimator.Play(targetClip, animLayer.fpsIndex, 0.0f);
        }
        
        public void SetBodyController(RuntimeAnimatorController newController)
        {
            SetController(bodyAnimator, newController, defaultBodyController);
        }

        public void SetHandsController(RuntimeAnimatorController newController)
        {
            SetController(handsAnimator, newController, defaultHandsController);
        }

        private void SetController(Animator anim, RuntimeAnimatorController newController,
            RuntimeAnimatorController defaultController = null)
        {
            if (newController == null && defaultController != null) newController = defaultController;
            if (anim.runtimeAnimatorController == newController) return;
            anim.runtimeAnimatorController = newController;
        }
        
        private void SetController(Animator anim, AnimatorOverrideController newController,
            RuntimeAnimatorController defaultController = null)
        {
            if (newController == null && defaultController != null) anim.runtimeAnimatorController = defaultController;

            else
            {
                if (anim.runtimeAnimatorController == newController) return;
                anim.runtimeAnimatorController = newController;
            }
        }

        public void SetHandsActive(bool value)
        {
            handsAnimator.gameObject.SetActive(value);
             
            if (!character) return;
            // REFRESH THE ANIMATOR CONTROLLER
            if (character.FPSInventory)
            {
                InHandItem inHandItem = character.FPSInventory.CurrentInHand;
                if (inHandItem)
                {
                    AnimatorOverrideController hands = inHandItem.Item.handsAnimatorController;
                    AnimatorOverrideController body = inHandItem.Item.bodyAnimatorController;
                    
                    SetController(handsAnimator, hands,defaultHandsController);
                    SetController(bodyAnimator, body, defaultBodyController);
                }
            }
        }

        public void SetAllFloat(int hash, float value)
        {
            handsAnimator.SetFloat(hash, value);
            bodyAnimator.SetFloat(hash, value);
        }
        
        public void SetAllBool(int hash, bool value)
        {
            handsAnimator.SetBool(hash, value);
            bodyAnimator.SetBool(hash, value);
        }

        public void SetWeaponAnimator(Animator anim) => weaponAnimator = anim;

        public Animator GetWeaponAnimator() => weaponAnimator;

        public GameObject GetHands() => handsAnimator.gameObject;

        public GameObject GetBody() => bodyAnimator.gameObject;
        
        public AnimState GetCurrentState() => currentState;

        #endregion
    }
}
