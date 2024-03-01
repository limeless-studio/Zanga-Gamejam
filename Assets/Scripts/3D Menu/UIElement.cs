using UnityEngine;

namespace Systems._3D_Menu.Elements
{
    public class UIElement : MonoBehaviour
    {
        protected virtual void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("3DUI");
        }
        
        public virtual void SetUIElement(object obj)
        {
            
        }
        
        public virtual void OnHover()
        {
            
        }
        
        public virtual void OnClick()
        {
            
        }
        
        public virtual void OnExit()
        {
            
        }
    }
}