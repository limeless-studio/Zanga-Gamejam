using System.Collections;
using System.Collections.Generic;
using Core;

using Inventory;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(FPSCharacter))]
    public class FPSInventory : BaseInventory
    {
        public FPSCharacter character;
        [SerializeField, Required] public Transform itemsHolder;
        [SerializeField, Disable] private int currentIndex;
        
        public InHandItem CurrentInHand => inHandItems[currentIndex];
        
        private readonly Dictionary<int, InHandItem> inHandItems = new ();


        protected override void Start()
        {
            base.Start();

            if (inHandItems.Count < size)
            {
                inHandItems.Clear();
                for (int i = 0; i < size; i++)
                {
                    inHandItems.Add(i, null);
                }
                
                for (int i = 0; i < size; i++)
                {
                    if (inventoryItems[i] != null)
                    {
                        InventoryItem itemSc = new InventoryItem(inventoryItems[i].Item);
                        inventoryItems[i] = itemSc;
                    }
                }
                
            }

            OnInventoryChange += OnOnInventoryChange;

            character = GetComponent<FPSCharacter>();
            character.OnInputUpdated += OnInputUpdated;
            character.TriggerHands(CurrentInHand);
            
            // Invoke Ready event
            Ready();
        }

        private void UseCurrentItem(ButtonState state)
        {
            if (!CurrentInHand) return;
            CurrentInHand.Use(state);
        }
        
        private void AimCurrentItem(ButtonState state)
        {
            if (!CurrentInHand || !CurrentInHand.IsGun) return;
            CurrentInHand.Aim(state);
        }

        private void ReloadCurrentItem(ButtonState state)
        {
            if (state != ButtonState.Pressed) return;
            if (!CurrentInHand || !CurrentInHand.IsGun) return;
            
            //Get the name of the animation state to play, which depends on weapon settings, and ammunition!
            string stateName = ((Weapon)CurrentInHand).HasAmmo() ? "Reload" : "Reload Empty";
            if (character.FPSAnimator)
            {
                character.FPSAnimator.PlayAnimationInstant(stateName, LayerTag.Actions);
                // for cycled reload yet to be implemented
                character.FPSAnimator.SetAllBool(AnimatorHashes.Reloading, true);
            }
            
            CurrentInHand.Reload(stateName, false);
        }

        private void OnInputUpdated(ref PlayerInput input)
        {
            // Update scroll wheel :)
            if (input.Attack != ButtonState.None) UseCurrentItem(input.Attack);
            if (input.Aim != ButtonState.None) AimCurrentItem(input.Aim); 
            if (input.Reload != ButtonState.None) ReloadCurrentItem(input.Reload);
            
            float mouseWheel = input.MouseWheel;
            if (mouseWheel == 0) return;
            
            if (CurrentInHand) CurrentInHand.gameObject.SetActive(false);
            
            if (mouseWheel > 0)
            {
                if (currentIndex >= inHandItems.Count - 1) currentIndex = 0;
                else currentIndex++;
            }
            else
            {
                if (currentIndex <= 0) currentIndex = inHandItems.Count - 1;
                else currentIndex--;
            }
            
            if (CurrentInHand) CurrentInHand.gameObject.SetActive(true);
            
            character.TriggerHands(CurrentInHand);
        }

        private void OnOnInventoryChange(InventoryActions action, int index, InventoryItem invItem)
        {
            switch (action)
            {
                case InventoryActions.AddItem:
                    SpawnHandItem(invItem, index);
                    break;
                case InventoryActions.RemoveItem:
                    DestroyHandItem(invItem, index);
                    break;
                case InventoryActions.StackItem:
                    //todo
                    break;
                case InventoryActions.UnStackItem:
                    //todo
                    break;
            }
        }

        public void SpawnHandItem(InventoryItem invItem, int index)
        {
            if (invItem.InHandItemObject == null) return;
            InHandItem inHand = Instantiate(invItem.InHandItem, itemsHolder);
            var itemTransform = inHand.transform;
            itemTransform.localPosition = Vector3.zero;
            itemTransform.localRotation = Quaternion.identity;
            
            inHand.Init(invItem, this);
            inHandItems[index] = inHand;
            inHand.gameObject.SetActive(false);
            inHand.gameObject.SetActive(index == currentIndex);
            if (index == currentIndex) character.TriggerHands(true);
        }

        public void DestroyHandItem(InventoryItem invItem, int index)
        {
            if (inHandItems[index] == null) return;
            if (index == currentIndex) character.TriggerHands(false);

            GameObject inHandObj = inHandItems[index].gameObject;
#if UNITY_EDITOR
            if (Application.isPlaying) Destroy(inHandObj);
            else DestroyImmediate(inHandObj);
#else
            Destroy(inHandObj);
#endif
        }

        public GameObject GetHands()
        {
            if (!character.FPSAnimator) return null;
            return character.FPSAnimator.GetHands();
        }

        public Weapon GetCurrentWeapon()
        {
            if (CurrentInHand is Weapon) return (Weapon)CurrentInHand;
            return null; 
        }
        
        public bool TryGetInHand(out InHandItem item)
        {
            item = CurrentInHand;
            return item != null;
        }
        
        public Transform GetCameraMotionHandler()
        {
            if (!character.FPSCamera) return null;
            return character.FPSCamera.GetCameraMotionHandler();
        }
    }
}