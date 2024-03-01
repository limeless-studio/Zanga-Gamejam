
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Snowy/Inventory/BaseItem")]
    public class Item : ScriptableObject
    {
        [Header("General Settings")]
        public int id = -1;
        public string itemName;
        [TextArea] public string description;
        [AssetPreview] public Sprite icon;
        
        public bool isStackable;
        [EnableIf(nameof(isStackable), true), Min(1)] public int maxStack = 1;

        public bool canHaveInHand;
        [AssetPreview, ShowIf(nameof(canHaveInHand), true)] public InHandItem inHandPrefab;
        
        [ShowIf(nameof(canHaveInHand), true)]
        public AnimatorOverrideController handsAnimatorController;
        [ShowIf(nameof(canHaveInHand), true)]
        public AnimatorOverrideController bodyAnimatorController;
    }
}