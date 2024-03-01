using System;

using UnityEngine;

namespace Inventory
{
    [Flags]
    public enum WeaponType
    {
        None = 0,
        AR = 1 << 0,
        Handgun = 1 << 1,
        Launchers = 1 << 2,
        Shotgun = 1 << 3,
        Smg = 1 << 4,
        Snipers = 1 << 5
    }
    
    [Flags]
    public enum WeaponSubType
    {
        None = 0,
        First = 1 << 0,
        Second = 1 << 1,
        Third = 1 << 2,
        Forth = 1 << 3,
        Fifth = 1 << 4,
        Sixth = 1 << 5
    }
    
    [CreateAssetMenu(fileName = "Weapon Settings", menuName = "Snowy/Inventory/Weapon")]
    public class WeaponItem : Item
    {
        [Space] 
        [Header("Weapon Type:")]
        public WeaponType weaponType;
        public WeaponSubType subType;
        
        [Space] 
        [Header("Weapon Settings:")] 
        public bool isAutomatic;
        public bool autoReload;
        [Min(1)] public float damage = 10f;
        [Min(1)] public float force = 50f;
        [Min(0)] public float fireRate = 0.2f;
        [Min(0)] public float reloadTime = 1f;
        
        [SerializeField, InLineEditor] public RecoilData recoilData;
        
        
        [Header("References:")] 
        public AudioClip reload;
        public AudioClip reloadEmpty;
        
        [AssetPreview] public GameObject casingPrefab;
        [AssetPreview] public GameObject trailPrefab;
        //[AssetPreview] public GameObject bulletPrefab;
        [AssetPreview] public GameObject bulletPrefab;
    }
}