using UnityEngine;

namespace Interaction
{
    public interface IInteractAction
    {
        Interactable Interactable { get; set; }

        void SetInteractable(Interactable interactable)
        {
            Interactable = interactable;
        }

        void Interact(FPSInteractor fpsInteractor);
        
        bool CanInteract();
    }
}