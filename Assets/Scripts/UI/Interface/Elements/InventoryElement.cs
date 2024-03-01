using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace UI
{
    public class InventoryElement : Element
    {
        [SerializeField] private InventorySlot slotPrefab;
        Dictionary<int, InventorySlot> slots = new();
        
        protected override void Enabled()
        {
            // TODO: Add inventory logic here
            if (character.FPSInventory == null) return;
            character.FPSInventory.OnInventoryChange += OnInventoryChange;
            if (IsInventoryReady()) Init();
        }

        void Init()
        {
            // Setup inventory slots
            for (int i = 0; i < character.FPSInventory.GetSize(); i++)
            {
                InventorySlot slot = Instantiate(slotPrefab, transform);
                slots.Add(i, slot);
                slot.SetItem(character.FPSInventory.GetItemAt(i));
                slot.gameObject.name = $"Slot {i}";
            }
           
            base.Enabled();
        }

        bool IsInventoryReady()
        {
            return character.FPSInventory.inventoryItems.Count == character.FPSInventory.GetSize();
        }

        private void OnInventoryChange(InventoryActions action, int index, InventoryItem item)
        {
            switch (action)
            {
                default:
                case InventoryActions.AddItem:
                case InventoryActions.StackItem:
                case InventoryActions.UnStackItem:
                    slots[index].SetItem(item);
                    break;
                case InventoryActions.RemoveItem:
                    slots[index].SetItem(null);
                    break;
                case InventoryActions.Ready:
                    Init();
                    break;
            }
        }

        
    }
}