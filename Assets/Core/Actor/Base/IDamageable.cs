using UnityEngine;

namespace Core
{
    public interface IDamageable
    {
        public void TakeDamage(float damage);
        public void TakeDamage(float damage, DamageCause cause);
        
        
        public void Heal(float amount);
        
        public void Die();
        
        public void SetHealth(float amount);
        
        public float GetHealth();
        
        public Transform GetTransform();
    }
}