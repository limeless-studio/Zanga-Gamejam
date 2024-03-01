using System;
using System.Linq;

using Inventory;
using Inventory.Attachments;
using UnityEngine;

/*[Serializable] public abstract class AttachmentBase {
    [EnumFlags] public WeaponType weaponType;
    public SpriteData[] sprites;

    public Sprite GetSprite(WeaponType type)
    {
        foreach (var data in sprites)
        {
            if (data.weaponType == type) return data.sprite;
        }

        return null;
    }

    public Sprite GetSprite(WeaponType type, int typeNumber)
    {
        foreach (var data in sprites)
        {
            if (data.weaponType == type && data.typeNumber == typeNumber) return data.sprite;
        }

        return null;
    }
}

[Serializable] public class MuzzleAttachment : AttachmentBase {
    public Muzzle muzzle;
}

[Serializable] public class ScopeAttachment : AttachmentBase {
    public Scope scope;
}

[Serializable] public class MagazineAttachment : AttachmentBase {
    public Magazine magazine;
}

[Serializable] public class GripAttachment : AttachmentBase {
    public Grip grip;
}

[Serializable] public class LaserAttachment : AttachmentBase {
    public Laser laser;
}*/

/*[Serializable] public class AttachmentSprite
{
    public int id = 0;
    [SerializeField] SpriteData[] sprites;
}*/

[Serializable]
public class SpriteData
{
    public WeaponType weaponType;
    [Min(1)] public WeaponSubType weaponSubType;
    [AssetPreview] public Sprite sprite;
}

[Serializable]
public class AttachmentSerialized<T>
{
    public int id;
    public WeaponType weaponTypes;
    public WeaponSubType weaponSubTypes;
    [SerializeReference] public T attachment;
    [SerializeField] private SpriteData[] sprites;

    public Sprite GetSprite(WeaponType type, WeaponSubType subType)
    {
        if ((weaponTypes & type) == 0 || (weaponSubTypes & subType) == 0) return null;
        // filter sprites by type and number
        return sprites.FirstOrDefault(x => (x.weaponType & type) != 0 && (x.weaponSubType & subType) != 0)?.sprite;
    }
}

namespace Inventory
{
    [CreateAssetMenu(fileName = "Attachments", menuName = "Snowy/FPS/Weapons Attachments Data")]
    public class AllAttachments : ScriptableObject
    {
        [Help("The first item in any list of a type is considered the default attachment for that type")]
        [Header("Attachments")]
        [ReorderableList] public AttachmentSerialized<Muzzle>[] muzzles;
        [ReorderableList] public AttachmentSerialized<Magazine>[] magazines;
        [ReorderableList] public AttachmentSerialized<Scope>[] scopes;
        [ReorderableList] public AttachmentSerialized<Laser>[] laser;
        [ReorderableList] public AttachmentSerialized<Grip>[] grips;

        public AttachmentSerialized<T> GetAttachment<T>(AttachmentSerialized<T>[] array, WeaponType type, WeaponSubType subType, int id = -1)
        {
            if (array.Length <= 0) return null;

            // filter for type.
            array = array.Where(x => (x.weaponTypes & type) != 0 && (x.weaponSubTypes & subType) != 0).ToArray();

            if (array.Length <= 0) return null;
            
            // filter for id if found or return 0;
            if (id == -1) return array[0];
            
            return array.FirstOrDefault(x => x.id == id);
        }
    }
}

static class Extension
{
    
}