using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory
{
    public class RecoilEffect : WeaponEffect
    {
        private Transform motionHandler;
        private RecoilData recoilData;
        
        Vector3 currRotational;
        Vector3 targetRotation;
        
        private Vector3 Recoil => recoilData.recoilAmount;
        private Vector3 AimRecoil => recoilData.aimRecoilAmount;

        private bool isAim;

        protected override void Start()
        {
            base.Start();
            motionHandler = Weapon.GetInventory().GetCameraMotionHandler();
            recoilData = Weapon.WeaponItem.recoilData;
            
            Weapon.OnAim += WeaponOnOnAim;
            
        }

        private void WeaponOnOnAim(object sender, ButtonState e) => isAim = e == ButtonState.Pressed || e == ButtonState.Held;
        

        public override void Apply()
        {
            if (!motionHandler) return;

            Vector3 recoil = isAim ? AimRecoil : Recoil;
            
            targetRotation += new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y),
                Random.Range(-recoil.z, recoil.z));
        }

        public override void OnUpdate()
        {
            if (!motionHandler) return;
            targetRotation = Vector3.Lerp(targetRotation, Vector3.down, Time.deltaTime * recoilData.recoverySpeed);
            currRotational = Vector3.Slerp(currRotational, targetRotation, Time.fixedDeltaTime * recoilData.snappiness);
            motionHandler.localRotation = Quaternion.Euler(currRotational);
            base.OnUpdate();
        }
    }
}