using System;
using UnityEngine;

namespace Inventory
{
    public abstract class WeaponEffect : MonoBehaviour
    {
        protected Weapon Weapon;
        protected WeaponEffectManager EffectManager;

        protected virtual void Start() { }
        
        public abstract void Apply();
        public virtual void OnUpdate(){}
        public virtual void OnLateUpdate(){}

        public void OnEnable()
        {
            Weapon = GetComponentInParent<Weapon>();
            EffectManager = GetComponentInParent<WeaponEffectManager>();
            
            EffectManager.RegisterEffect(this);
        }

        public void OnDisable()
        {
            EffectManager.UnregisterEffect(this);
        }
    }
}