using Core;
using FPS;
using Inventory.Attachments;
using UnityEngine;
using Utilities;

namespace Inventory
{
    [RequireComponent(typeof(Weapon))]
    public class WeaponAttachmentManager : MonoBehaviour
    {
        [Header("Sockets")]
        [SerializeField] Transform scopeSocket;
        [SerializeField] Transform muzzleSocket;
        [SerializeField] Transform laserSocket;
        [SerializeField] Transform gripSocket;
        [SerializeField] Transform magazineSocket;

        [SerializeField] private bool defaultScope = false;
        [SerializeField] private bool defaultLaser = false;
        [SerializeField] private bool defaultGrip = false;

        private Magazine magazine;
        private Muzzle muzzle;
        private Laser laser;
        private Scope scope;
        private Grip grip;
        
        private Weapon weapon;
        
        private void Start()
        {
            weapon = GetComponent<Weapon>();

            InitAttachment();
        }

        private void InitAttachment()
        {
            WeaponType type = weapon.WeaponItem.weaponType;
            WeaponSubType subType = weapon.WeaponItem.subType;

            AttachmentSerialized<Magazine> magazineAttachment = GlobalSettings.Instance.GetMagazine(type, subType);
            AttachmentSerialized<Scope> scopeAttachment = GlobalSettings.Instance.GetScope(type, subType);
            AttachmentSerialized<Muzzle> muzzleAttachment = GlobalSettings.Instance.GetMuzzle(type, subType);
            AttachmentSerialized<Laser> laserAttachment = GlobalSettings.Instance.GetLaser(type, subType);
            AttachmentSerialized<Grip> gripAttachment = GlobalSettings.Instance.GetGrip(type, subType);

            /*
             * - Instantiate
             * - Reset Transform
             * - Set the sprite
            */
            
            if (magazineAttachment != null)
            {
                magazine = Instantiate(magazineAttachment.attachment, magazineSocket);
                magazine.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                magazine.SetSprite(magazineAttachment.GetSprite(type, subType));

                if (magazine)
                {
                    weapon.SetMaxAmmo(magazine.GetAmmunitionTotal());
                    weapon.RefillAmmo();
                }
            }

            if (muzzleAttachment != null)
            {
                muzzle = Instantiate(muzzleAttachment.attachment, muzzleSocket);
                muzzle.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                muzzle.SetSprite(muzzleAttachment.GetSprite(type, subType));
                
                if (muzzle) weapon.onShoot.AddListener(muzzle.Run);
            }
            
            if(scopeAttachment != null && defaultScope)
            {
                scope = Instantiate(scopeAttachment.attachment, scopeSocket);
                scope.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                scope.SetSprite(scopeAttachment.GetSprite(type, subType));

                weapon.OnAim += OnAim;
            }
            
            if (laserAttachment != null && defaultLaser)
            {
                laser = Instantiate(laserAttachment.attachment, laserSocket);
                laser.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                laser.SetSprite(laserAttachment.GetSprite(type, subType));
            }
            
            if (gripAttachment != null && defaultGrip)
            {
                grip = Instantiate(gripAttachment.attachment, gripSocket);
                grip.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                grip.SetSprite(gripAttachment.GetSprite(type, subType));
            }
            
        }

        public void OnAim(object sender, ButtonState input)
        {
            FPSCamera cam = weapon.GetFPSCamera();
            if (!scope || !cam) return;
            
            if (input == ButtonState.None || input == ButtonState.Released)
                cam.SetFOVMultiplier(1f);
            
            else
                cam.SetFOVMultiplier(scope.GetFieldOfViewMultiplierAim());
            
        }
        
        #region Getters

        public Muzzle GetMuzzle() => muzzle;
        public Laser GetLaser() => laser;
        public Magazine GetMagazine() => magazine;
        public Scope GetScope() => scope;
        public Grip GetGrip() => grip;

        #endregion
    }
}