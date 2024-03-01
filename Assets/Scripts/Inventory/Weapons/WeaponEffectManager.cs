using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Inventory
{
    [RequireComponent(typeof(Weapon))]
    public class WeaponEffectManager : MonoBehaviour
    {
        [SerializeField] List<WeaponEffect> effects;
        private Weapon weapon;
        
        public event Action OnUpdate;
        public event Action OnLateUpdate;

        private void Start()
        {
            weapon = GetComponent<Weapon>();
            weapon.onShoot.AddListener(OnShoot);
        }

        void OnShoot()
        {
            foreach (var effect in effects)
            {
                effect.Apply();
            }
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        public void RegisterEffect(WeaponEffect effect)
        {
            if (!effects.Contains(effect))
            {
                effects.Add(effect);
                OnUpdate += effect.OnUpdate;
                OnLateUpdate += effect.OnLateUpdate;
            }
        }
        
        public void UnregisterEffect(WeaponEffect effect)
        {
            if (effects.Contains(effect))
            {
                effects.Remove(effect);
                OnUpdate -= effect.OnUpdate;
                OnLateUpdate -= effect.OnLateUpdate;
            }
        }
    }
}