using FPS;
using UnityEngine;

namespace UI
{
    public class Element : MonoBehaviour
    {
        protected InterfaceManager interfaceManager;
        protected FPSCharacter character;

        protected virtual void Awake() { }
        protected virtual void Start() { }

        private void OnDisable()
        {
            if (interfaceManager) interfaceManager.UnRegisterElement(this);
        }

        public void Enable(InterfaceManager intManager)
        {
            interfaceManager = intManager;
            character = interfaceManager.character;
            Enabled();
        }

        public void Run()
        {
            Tick();
        }
        
        protected virtual void Enabled() {}

        protected virtual void Tick(){}
    }
}