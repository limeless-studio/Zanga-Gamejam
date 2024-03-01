using Core;
using FPS;
using UnityEngine;

namespace Inventory
{
    public class InHandItem : MonoBehaviour
    {
        public InventoryItem InvItem;
        public Item Item => InvItem.Item;

        protected FPSInventory FPSInventory;

        public Sprite GetSprite() => Item.icon;

        public virtual void Init(InventoryItem item, FPSInventory fpsInv)
        {
            InvItem = item;
            FPSInventory = fpsInv;
        }

        public virtual void Use(ButtonState input) { }

        public FPSInventory GetInventory() => FPSInventory;

        public virtual bool IsGun => false;

        public virtual void Aim(ButtonState input) {}

        public virtual void Reload(string state, bool isReloading = false) { }
    }
}