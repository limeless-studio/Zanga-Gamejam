using System;
using UnityEngine;
using Utilities;

namespace Interaction
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        [SerializeField] public string promptText = "Press [E] to interact";
        [SerializeField] public bool isActive = true;
        public bool activateWithoutInput = false;
        [SerializeField] bool onlyPlayerCanInteract = true;

        [SerializeReference, ReferencePicker(TypeGrouping = TypeGrouping.ByFlatName)] public IInteractAction action;
        Collider _collider;

        /*#if UNITY_EDITOR
        private void OnValidate()
        {
            if (actionType == null || actionType == "") return;
            
            Type type = Utility.GetType(actionType);
            
            if (type == null)
            {
                Debug.LogError("Type " + actionType + " does not exist!");
                return;
            }
            
            if (type.GetInterface("IInteractAction") == null)
            {
                Debug.LogError("Type " + actionType + " does not implement IInteractAction!");
            }else if (action == null || action.GetType() != type)
            {
                // Create it as a subclass of IInteractAction
                action = (IInteractAction) Activator.CreateInstance(type);
                // change the action type to the type name
                action.SetInteractable(this);
            } else
            {
                if (action != null) action.SetInteractable(this);
                else
                {
                    Debug.LogError("Action is null!");
                }
            }
        }
        #endif*/

        private void Start()
        {
            if (!TryGetComponent(out _collider))
            {
                Debug.LogError("Interactable object " + gameObject.name + " does not have a collider!\nAdding a BoxCollider by default.");
                _collider = gameObject.AddComponent<BoxCollider>();
            }

            if (_collider.GetType() != typeof(MeshCollider))
            {
                _collider.isTrigger = true;
            }
            
            action.SetInteractable(this);
        }
        
        public void Interact(FPSInteractor fpsInteractor)
        {
            action.Interact(fpsInteractor);
        }

        public bool CanInteract(FPSInteractor fpsInteractor)
        {
            if (!isActive) return false;
            if (onlyPlayerCanInteract && !fpsInteractor.IsPlayer()) return false;
            return action.CanInteract();
        }
        
        public bool IsActive() => isActive;
    }
}