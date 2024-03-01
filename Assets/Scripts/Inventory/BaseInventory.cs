using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Inventory
{
    public enum InventoryActions
    {
        AddItem,
        RemoveItem,
        StackItem,
        UnStackItem,
        Ready
    }
    
    public class BaseInventory : MonoBehaviour
    {
        [SerializeField, Min(0)] protected int size = 20;
        
        public Dictionary<int, InventoryItem> inventoryItems = new Dictionary<int, InventoryItem>();

        public event Action<InventoryActions, int, InventoryItem> OnInventoryChange;
        
        [ContextMenu("Init")]
        public void Init()
        {
            inventoryItems.Clear();
            Start();
        }
        protected virtual void Start()
        {
            // Instantiate the inventory slots;
            if (inventoryItems.Count < size)
            {
                inventoryItems.Clear();
                for (int i = 0; i < size; i++) inventoryItems.Add(i, null);
            }
        }

        public void AddItem(Item item, int amount = 1)
        {
            if (inventoryItems.Count < 1) Init();
            int toAdd = amount;
            if (item.isStackable)
            {
                InventoryItem[] allItems = GetAllItemsOfType(item);
                InventoryItem freeItem = GetMostEmpty(allItems);

                int safetyMaxTries = 50;
                int tries = 0;

                while (toAdd > 0 && tries < safetyMaxTries)
                {
                    if (freeItem == null)
                    {
                        CreateItem(item);
                        allItems = GetAllItemsOfType(item);
                        freeItem = GetMostEmpty(allItems);
                        if (freeItem == null)
                        {
                            Debug.LogError("Something went wrong while creating an Item, and we could get stuck in a loop!", this);
                            break;
                        }

                        toAdd--;
                    }
                    else
                    {
                        toAdd = freeItem.Stack(toAdd);
                        freeItem = GetMostEmpty(allItems);
                        int index = GetItemIndex(freeItem);
                        OnInventoryChange?.Invoke(InventoryActions.StackItem, index, freeItem);
                    }

                    tries++;
                }
                
                return;
            }
            
            // Create new Item
            for (int i = 0; i < amount; i++) CreateItem(item);
        }
        
        protected void Ready()
        {
            OnInventoryChange?.Invoke(InventoryActions.Ready, -1, null);
        }

        public void RemoveItem(InventoryItem item)
        {
            RemoveItem(item, item.Count);
        }

        public void RemoveItem(InventoryItem item, int amount)
        {
            if (item.Remove(amount) <= 0)
            {
                int index = GetItemIndex(item);
                OnInventoryChange?.Invoke(InventoryActions.RemoveItem, index, item);
                inventoryItems[index] = null;
            }
        }

        public void CreateItem(Item item)
        {
            // Check if there is free space in the inventory;
            if (IsFull())
            {
                Debug.LogError("Inventory is Full!");
                return;
            }

            int freeIndex = GetFirstFreeSlot();
            InventoryItem itemSc = new InventoryItem(item);
            inventoryItems[freeIndex] = itemSc;
            
            OnInventoryChange?.Invoke(InventoryActions.AddItem, freeIndex, itemSc);
        }

        /// <summary>
        /// Gets the first Item of a specific type 
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>InventoryItem</returns>
        [CanBeNull]
        public InventoryItem GetAnyItemOfType(Item item) => inventoryItems.FirstOrDefault(x => x.Value != null && x.Value.Item == item).Value;
        
        /// <summary>
        /// Returns a list of all the items of a specific type
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>InventoryItem[]</returns>
        [CanBeNull]
        public InventoryItem[] GetAllItemsOfType(Item item) => inventoryItems.Values.Where(x => x != null && x.Item == item).ToArray();

        /// <summary>
        /// Get the item with the most space to stack (Amount - MaxStack)
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        [CanBeNull]
        public InventoryItem GetMostEmpty(InventoryItem[] items)
        {
            InventoryItem mostEmpty = null;
            foreach (InventoryItem item in items)
            {
                if (item.CanStack() && (mostEmpty == null || item.FreeSpace() > mostEmpty.FreeSpace()))
                    mostEmpty = item;
            }
            return mostEmpty;
        }
        
        /// <summary>
        /// Gets Item by index
        /// </summary>
        /// <param name="index">int</param>
        /// <returns>InventoryItem</returns>
        [CanBeNull] public InventoryItem GetItemAt(int index) => inventoryItems[index];

        /// <summary>
        /// Gets Index by Item
        /// </summary>
        /// <param name="item">InventoryItem</param>
        /// <returns>int</returns>
        [CanBeNull]
        public int GetItemIndex(InventoryItem item) => inventoryItems.FirstOrDefault(x => x.Value == item).Key;

        /// <summary>
        /// Returns the first free slot index in the inventory
        /// </summary>
        /// <returns>int</returns>
        public int GetFirstFreeSlot() => inventoryItems.FirstOrDefault(x => x.Value == null).Key;

        /// <summary>
        /// Returns true if the Inventory is empty
        /// </summary>
        /// <returns>bool</returns>
        public bool IsEmpty() => inventoryItems.Values.All(x => x == null);

        /// <summary>
        /// Returns true if the inventory is Full
        /// </summary>
        /// <returns>bool</returns>
        public bool IsFull() => inventoryItems.Values.All(x => x != null);
        
        /// <summary>
        /// Returns a list of all items with thier indexes
        /// </summary>
        /// <returns>KeyValuePair&lt;int, InventoryItem&gt;[]</returns>
        public KeyValuePair<int, InventoryItem>[] GetInventoryItems()
        {
            return inventoryItems.Where(x => x.Value != null).ToArray();
        }
        
        /// <summary>
        /// Returns the size of the inventory
        /// </summary>
        /// <returns>int</returns>
        public int GetSize() => size;
    }
}