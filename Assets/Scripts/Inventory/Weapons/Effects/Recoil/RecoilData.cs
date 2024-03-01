using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Recoil Profile", menuName = "Snowy/FPS/Weapon/Recoil Profile")]
    public class RecoilData : ScriptableObject
    {
        public Vector3 recoilAmount;
        public Vector3 aimRecoilAmount;
        public float snappiness;
        public float recoverySpeed;
    }
}