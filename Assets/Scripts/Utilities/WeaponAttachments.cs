
using Inventory;
using UnityEngine;

namespace Utilities
{
    public class WeaponAttachment
    {
        public WeaponType weaponType;
        public GameObject Attachment;
    }
    
    [CreateAssetMenu(fileName = "Weapon Attachments", menuName = "Snowy/Global/Weapon Attachments", order = 0)]
    public class WeaponAttachments : ScriptableObject
    {
        
    }
}