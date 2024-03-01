using System;
using Core;
using FPS;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Inventory
{
    public class Weapon : InHandItem
    {
        [SerializeField] private Animator animator;
        public UnityEvent onShoot;
        public event EventHandler<ButtonState> OnAim;

        public WeaponItem WeaponItem => (WeaponItem) Item;

        private WeaponAttachmentManager attachmentManager;
        
        private float lastShot;

        private int ammo;
        private int maxAmmo;
        private int shotsFired;
        
        private bool canShoot = true;
        private bool manualShot;
        RaycastHit hit;
        private Transform cam;

        private FPSAnimator characterAnimator;
        private FPSCharacter character;
        private FPSCamera fpsCamera;
        private AudioSource audioSource;
        
        private float aiming = 0f;
        
        public int GetShotsFired() => shotsFired;

        private void Start()
        {
            attachmentManager = GetComponentInChildren<WeaponAttachmentManager>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        }
        
        private void OnEnable()
        {
            if (characterAnimator) characterAnimator.SetWeaponAnimator(animator);
        }

        private void OnDisable()
        {
            if (characterAnimator && characterAnimator.GetWeaponAnimator() == animator)
            {
                characterAnimator.SetWeaponAnimator(null);
            }
        }


        public override void Init(InventoryItem item, FPSInventory fpsInv)
        {
            if (fpsInv.character && fpsInv.character.FPSCamera)
            {
                character = fpsInv.character;
                fpsCamera = character.FPSCamera;
                cam = fpsCamera.GetCamera()?.transform;
                characterAnimator = character.FPSAnimator;
            }
            
            if (!animator) animator = GetComponent<Animator>();
            
            base.Init(item, fpsInv);
        }

        public override void Use(ButtonState input)
        {
            if (input == ButtonState.Pressed || input == ButtonState.Released) shotsFired = 0;

            if (!canShoot) return;
            if (Time.time - lastShot < WeaponItem.fireRate) return;
            
            if (!WeaponItem.isAutomatic)
            {
                if (manualShot && input == ButtonState.Released)
                {
                    manualShot = false;
                    return;
                }
                if (manualShot) return;
                manualShot = true;
            }
            else
            {
                if (input != ButtonState.Held) return;
            }
            
            Shoot();

        }

        public override void Aim(ButtonState input)
        {
            OnAim?.Invoke(this, input);
        }
        
        public override void Reload(string stateName, bool isReloading = false)
        {
            AudioClip clip = stateName == "Reload" ? WeaponItem.reload : WeaponItem.reloadEmpty;
            audioSource.clip = clip;
            audioSource.Play();
            
            animator.Play(stateName, 0, 0.0f);
        }

        private void Shoot()
        {
            if (!cam)
            {
                Debug.LogWarning("Cannot Shoot, Camera is not assigned in this weapon");
                return;
            }

            if (ammo <= 0)
            {
                // todo check if auto reload to reload or shot empty shot :)
                if (WeaponItem.autoReload)
                {
                    // todo auto reload
                    // todo Reload in general :skull:
                }
                
                // shot empty shot
                return;
            }

            ammo = Mathf.Clamp(ammo - 1, 0, maxAmmo);
            shotsFired++;
            
            lastShot = Time.time;
            
            Ray ray = new Ray(cam.position, cam.forward);
            bool didHit = Physics.Raycast(cam.position, cam.forward, out hit);

            animator.Play("Fire", 0);
            characterAnimator.PlayAnimationInstant("Fire", LayerTag.Overlay);
            onShoot?.Invoke();
            
            if (didHit)
            {
                //todo did hit yay!
                GameObject impactPrefab = GlobalSettings.Instance.GetImpact(hit.transform.gameObject);
                if (impactPrefab)
                {
                    Instantiate(impactPrefab, hit.point,
                        Quaternion.LookRotation(hit.normal), hit.transform);
                }

                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForceAtPosition(-hit.normal * WeaponItem.force, hit.point);
                }

                // Check if has rigidbody to throw
                if (hit.transform.gameObject.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(WeaponItem.damage, DamageCause.DamagedByActor);
                }
            }
            else
            {
                // todo well umm.. do something for sure ;__;
            }

        }

        public override bool IsGun => true;

        public void SetMaxAmmo(int mAmmo)
        {
            maxAmmo = mAmmo;
        }
        
        public int GetAmmo() => ammo;
        public int GetMaxAmmo() => maxAmmo;

        public void RefillAmmo() => ammo = maxAmmo;
        
        public WeaponAttachmentManager AttachmentManager => attachmentManager;

        public FPSCamera GetFPSCamera() => fpsCamera;

        public bool HasAmmo() => ammo > 0;
        
        public float GetAiming() => aiming;
    }
}