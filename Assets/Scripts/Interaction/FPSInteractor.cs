using System;
using Core;
using FPS;
using Inventory;
using UI;
using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(FPSCharacter))]
    public class FPSInteractor : MonoBehaviour
    {
        [SerializeField] bool isPlayer = true;
        [SerializeField] bool canInteract = true;
        public Interactable interactable;
        public FPSCharacter character;

        private void Start()
        {
            character = GetComponent<FPSCharacter>();
            character.OnInputUpdated += OnInputUpdated;
        }

        private void OnInputUpdated(ref PlayerInput input)
        {
            if (!canInteract || interactable == null) return;

            if (input.Interact == ButtonState.Pressed)
            {
                interactable.Interact(this);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Interactable newInteractable))
            {
                CheckInteractable(newInteractable);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Interactable newInteractable))
            {
                RemoveInteractable(newInteractable);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Interactable newInteractable))
            {
                CheckInteractable(newInteractable);
            }
        }

        private void CheckInteractable(Interactable newInteractable)
        {
            if (newInteractable.activateWithoutInput)
            {
                newInteractable.Interact(this);
            }else if (newInteractable == interactable && isPlayer)
            {
                if (!NotificationManager.Instance.IsShowingPopup())
                {
                    NotificationManager.Instance.ShowNotification(NotificationType.Popup, interactable.promptText, 9999f);
                }
            }
            else
            {
                if (interactable)
                {
                    float distance = Vector3.Distance(transform.position, newInteractable.transform.position);
                    float oldDistance = Vector3.Distance(transform.position, interactable.transform.position);
                    if (distance >= oldDistance) return;
                }
                
                // Remove the old notification
                if (interactable) RemoveInteractable(interactable);
                if (newInteractable.CanInteract(this)) interactable = newInteractable;
            }
        }

        public bool IsPlayer() => isPlayer;
        
        public void RemoveInteractable(Interactable inter)
        {
            if (interactable == inter) interactable = null;
            if (isPlayer) NotificationManager.Instance.HideNotification(NotificationType.Popup);
        }
    }
}