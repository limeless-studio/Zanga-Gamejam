using System;

using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class InventoryItem
    {
        [SerializeField, InLineEditor] public Item Item;
        [SerializeField, Disable] private int count = 1;

        #region Scriptable Getters

        public string Name => Item.itemName;
        public string Description => Item.description;
        public bool IsStackable => Item.isStackable;
        public int MaxStack => Item.isStackable ? Item.maxStack : 1;
        
        public Sprite Icon => Item.icon;
        public GameObject InHandItemObject => Item.inHandPrefab ? Item.inHandPrefab.gameObject : null;
        
        public InHandItem InHandItem => Item.inHandPrefab;
        #endregion

        public int Count
        {
            get => count;
            private set => count = value;
        }

        public InventoryItem(Item item)
        {
            Item = item;
        }
        
        public bool CanStack() => count < MaxStack && Item.isStackable;

        public int FreeSpace() => count - MaxStack;
        
        public int Stack(int amount)
        {
            if (!CanStack()) return amount;

            int canAdd = (MaxStack - count);
            int rest = 0;
            int toAdd = 0;
            if (amount <= canAdd) toAdd = amount;
            else
            {
                toAdd = canAdd;
                rest = amount - toAdd;
            }

            count = Mathf.Min(count + toAdd, MaxStack);
            return rest;
        }

        public int Remove(int amount)
        {
            int toRemove = Mathf.Min(count, amount);
            int rest = Mathf.Max(count - amount, 0);
            
            count = Mathf.Max(count - toRemove, 0);
            return rest;
        }

    }
}