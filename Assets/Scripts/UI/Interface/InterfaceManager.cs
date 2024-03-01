using System;
using Core;
using FPS;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(FPSCharacter))]
    public class InterfaceManager : MonoBehaviour
    {
        [SerializeField] private GameObject inputElement;
        [HideInInspector] public FPSCharacter character;
        private event Action OnUpdate;

        private void Awake()
        {
            character = GetComponent<FPSCharacter>();
            if (!character) Debug.LogError("InterfaceManager: No FPSCharacter component found");
            if (!character.FPSMovement) character.SetUpComponents();
            
            character.OnInputUpdated += OnInputUpdated;
            
            inputElement.SetActive(false);
        }

        private void OnInputUpdated(ref PlayerInput input)
        {
            if (input.Inventory == ButtonState.Pressed)
            {
                inputElement.SetActive(!inputElement.activeSelf);
                character.SetImmobilized(inputElement.activeSelf);
                character.SetCameraLocked(inputElement.activeSelf);
            }
        }

        private void Start()
        {
            EnableAll();
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }
        
        public void EnableAll()
        {
            if (!character) character = GetComponent<FPSCharacter>();
            foreach (var element in GetComponentsInChildren<Element>(true))
            {
                element.Enable(this);
                OnUpdate += element.Run;
            }
        }

        public void UnRegisterElement(Element element)
        {
            OnUpdate -= element.Run;
        }
    }
}