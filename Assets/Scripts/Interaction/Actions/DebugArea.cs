using UnityEngine;

namespace Interaction
{
    public class DebugArea : IInteractAction
    {
        public string debugText = "This is a debug text";
        public Interactable Interactable { get; set; }
        public void Interact(FPSInteractor fpsInteractor)
        {
            Debug.Log(debugText);
        }
        
        public bool CanInteract()
        {
            return true;
        }
    }
}