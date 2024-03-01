
using Inventory;
using UnityEngine;
using Utilities;

namespace FPS
{
    // ignore never used warning FOR float
    #pragma warning disable 414
    
    public class ItemOffset : Motion
    {
        [SerializeField, Min(0.1f)] private float offsetChangeSpeed = 10f;
        private FPSCamera fpsCamera;
        private FPSCharacter character;

        private Vector3 pos;
        private Vector3 rot;
        
        /// <summary>
        /// springLocation. Handles all location interpolation.
        /// </summary>
        private readonly Spring springLocation = new Spring();
        /// <summary>
        /// springRotation. Handles all rotation interpolation.
        /// </summary>
        private readonly Spring springRotation = new Spring();

        private void Start()
        {
            fpsCamera = motionApplier.GetCharacter().FPSCamera;
            character = motionApplier.GetCharacter();
        }

        public override void Tick()
        {
            // ignore
            if (fpsCamera == null)
            {
                fpsCamera = motionApplier.GetCharacter().FPSCamera;
                return;
            }
            
            if (!character.FPSInventory || !character.FPSInventory.TryGetInHand(out var item)) return;
            
            //Location.
            Vector3 location = default;
            //Rotation.
            Vector3 rotation = default;
            
            // Aim:
            if (character.IsAiming())
            {
                if (item.IsGun)
                {
                    // aim
                    var weapon = (Weapon)item;
                    if (!weapon.AttachmentManager) return;
                    var scope = weapon.AttachmentManager.GetScope();
                    if (scope)
                    {
                        location += scope.GetOffsetAimingLocation();
                        rotation += scope.GetOffsetAimingRotation();
                    }
                }
            }
            
            //Update End Values.
            springLocation.UpdateEndValue(location);
            springRotation.UpdateEndValue(rotation);
        }
        

        public override Vector3 GetLocation() => springLocation.Evaluate();

        public override Vector3 GetEulerAngles() => springRotation.Evaluate();
    }
    
}