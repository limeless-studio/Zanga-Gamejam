using Inventory;
using UI;
using UnityEngine;

namespace Interaction
{
    public enum AfterPickupAction
    {
        Destroy,
        DisableThePickup,
        Nothing
    }
    public class Pickup : IInteractAction
    {
        [SerializeField] Item itemScriptable;
        [SerializeField] int amount;
        [SerializeField] bool pickupOneAtATime;
        [SerializeField] AfterPickupAction afterPickupAction;
        private int left = -1;
        [SerializeField] bool canPickup = true;

        public Interactable Interactable { get; set; }
        
        public void Interact(FPSInteractor fpsInteractor)
        {
            if (fpsInteractor.character == null || fpsInteractor.character.FPSInventory == null) return;
            if (!canPickup) return;
            bool done = false;
            if (pickupOneAtATime)
            {
                if (left == -1) left = amount;
                if (left == 0)
                {
                    return;
                }
                left--;
                fpsInteractor.character.FPSInventory.AddItem(itemScriptable, 1);
                done = left == 0;
            }
            else
            {
                fpsInteractor.character.FPSInventory.AddItem(itemScriptable, amount);
                done = true;
            }

            if (done)
            {
                switch (afterPickupAction)
                {
                    case AfterPickupAction.Destroy:
                        Object.Destroy(Interactable.gameObject);
                        break;
                    case AfterPickupAction.DisableThePickup:
                        Interactable.isActive = false;
                        break;
                    case AfterPickupAction.Nothing:
                        break;
                }
            }
            
            fpsInteractor.RemoveInteractable(Interactable);
        }
        
        public bool CanInteract()
        {
            return canPickup;
        }
    }
}