using System;
using System.Linq;
using Inventory;
using Inventory.Attachments;
using JetBrains.Annotations;
using Scriptables;
using UnityEngine;

namespace Utilities
{
    public class GlobalSettings : MonoBehaviour
    {
        public static GlobalSettings Instance;
        
        [SerializeField] private GlobalMaterials globalMaterials;
        [SerializeField] private ShotImpacts shotImpacts;
        [SerializeField] private AllAttachments attachments;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        #region API

        public GlobalMaterials GetGlobalMaterials => globalMaterials;
        public ShotImpacts GetShotImpacts => shotImpacts;
        public AllAttachments Attachments => attachments;
        
        public NamedMaterial[] GetNamedMaterials() => globalMaterials ? globalMaterials.WeaponMaterials : Array.Empty<NamedMaterial>();
        public CharacterMaterial[] GetCharacterMaterials => globalMaterials ? globalMaterials.CharacterMaterials : Array.Empty<CharacterMaterial>();
        public CharacterMaterial GetDefaultMaterial() => globalMaterials ? globalMaterials.CharacterMaterials[0] : null;
        public Material GetInvisibleMat() => globalMaterials ? globalMaterials.invisibleMaterial : null;
        
        public GameObject GetImpact(GameObject target)
        {
            if (shotImpacts == null) return null;
            Impact imp = shotImpacts.impacts.FirstOrDefault(x => target.CompareTag(x.tag));
            return imp != null ? imp.impact : shotImpacts.impacts[0].impact;
        }

        [CanBeNull]
        public AttachmentSerialized<Magazine> GetMagazine(WeaponType type, WeaponSubType subType, int id = -1) =>
            attachments.GetAttachment(attachments.magazines, type, subType, id);

        [CanBeNull]
        public AttachmentSerialized<Muzzle> GetMuzzle(WeaponType type, WeaponSubType subType, int id = -1) =>
            attachments.GetAttachment(attachments.muzzles, type, subType, id);

        [CanBeNull]
        public AttachmentSerialized<Scope> GetScope(WeaponType type, WeaponSubType subType, int id = -1) =>
            attachments.GetAttachment(attachments.scopes, type, subType, id);

        [CanBeNull]
        public AttachmentSerialized<Laser> GetLaser(WeaponType type, WeaponSubType subType, int id = -1) =>
            attachments.GetAttachment(attachments.laser, type, subType, id);
        [CanBeNull] public AttachmentSerialized<Grip> GetGrip(WeaponType type, WeaponSubType subType, int id = -1) => attachments.GetAttachment(attachments.grips, type, subType, id);

        public object GetFromArray(int index, object[] array)
        {
            object obj = array[0];
            if (index > -1 && index < array.Length)
                obj = array[index];

            return obj;
        }
        
        #endregion
    }
}