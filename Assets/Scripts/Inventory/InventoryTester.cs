
using UnityEngine;

namespace Inventory
{
    public class InventoryTester : MonoBehaviour
    {
        [SerializeField] private BaseInventory inventory;
        [SerializeField, InLineEditor] private Item[] items;
        [SerializeField, Min(1)] private int amount = 1;

        private void Start()
        {
            if (!inventory) inventory = GetComponentInChildren<BaseInventory>();
        }

        [Button("Add Random Item")]
        public void AddRandomItem()
        {
            if (items.Length == 0 || !inventory) return;
            Item item = items[Random.Range(0, items.Length)];
            if (item)
            {
                inventory.AddItem(item, amount);
            }
        }
    }
}