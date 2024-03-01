using System;

using UnityEngine;

namespace Scriptables
{
    [Serializable] public class NamedMaterial
    {
        public string name;
        public Material material;
    }

    [Serializable] public class CharacterMaterial
    {
        public string name = "";
        public Material bodyMaterial;
        public Material handsMaterial;
    }
    
    public enum MaterialType
    {
        AlwaysVisible,
        InvisibleInFirstPerson,
    }
    
    [Serializable] public class TpMaterial
    {
        public MaterialType materialType;
        [HideInInspector] public Material material;
    }
    
    
    [CreateAssetMenu(fileName = "Global Materials", menuName = "Snowy/Global/Materials", order = 0)]
    public class GlobalMaterials : ScriptableObject
    {
        [ReorderableList] public NamedMaterial[] WeaponMaterials;
        
        [ReorderableList] public CharacterMaterial[] CharacterMaterials;
        
        public Material invisibleMaterial;
    }
}