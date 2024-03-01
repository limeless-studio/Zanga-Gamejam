using Game;
using Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Interaction_System.Interactables
{
    public struct StaticCamera : IInteractAction
    {
        public Transform target;
        [SerializeField] private float transitionTime;
        public UnityEvent onInteract;
        
        public Interactable Interactable { get; set; }


        public void Interact(FPSInteractor interactor)
        {
            if (GameManager.Instance == null) return;
            if (GameManager.Instance.GetPlayer() == null) return;
            if (GameManager.Instance.GetPlayer().FixedCamera == null) return;
            
            GameManager.Instance.GetPlayer().FixedCamera.SetFixedCamera(true, target, transitionTime);
            
            if (Interactable) interactor.RemoveInteractable( Interactable );
            
            onInteract?.Invoke();
        }

        public bool CanInteract()
        {
            if (GameManager.Instance == null) return false;
            if (GameManager.Instance.GetPlayer() == null) return false;
            if (GameManager.Instance.GetPlayer().FixedCamera == null) return false;
            return !GameManager.Instance.GetPlayer().FixedCamera.IsFixed();
        }
    }
}