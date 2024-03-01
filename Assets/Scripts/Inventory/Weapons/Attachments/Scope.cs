using UnityEngine;

namespace Inventory.Attachments
{
    public class Scope : WeaponAttachment
    {
        [Header("Aiming Offset")]
        
        [Tooltip("Weapon bone location offset while aiming.")]
        [SerializeField]
        private Vector3 offsetAimingLocation;
        
        [Tooltip("Weapon bone rotation offset while aiming.")]
        [SerializeField]
        private Vector3 offsetAimingRotation;
        
        [Header("Field Of View")]

        [Tooltip("Field Of View Multiplier Aim.")]
        [SerializeField]
        private float fieldOfViewMultiplierAim = 0.9f;
        
        [Tooltip("Field Of View Multiplier Aim Weapon.")]
        [SerializeField]
        private float fieldOfViewMultiplierAimWeapon = 0.7f;
        
        /// <summary>
        /// GetOffsetAimingLocation.
        /// </summary>
        public Vector3 GetOffsetAimingLocation() => offsetAimingLocation;
        /// <summary>
        /// GetOffsetAimingRotation.
        /// </summary>
        public Vector3 GetOffsetAimingRotation() => offsetAimingRotation;
        
        /// <summary>
        /// GetFieldOfViewMultiplierAim.
        /// </summary>
        public float GetFieldOfViewMultiplierAim() => fieldOfViewMultiplierAim;
        /// <summary>
        /// GetFieldOfViewMultiplierAimWeapon.
        /// </summary>
        public float GetFieldOfViewMultiplierAimWeapon() => fieldOfViewMultiplierAimWeapon;
    }
}