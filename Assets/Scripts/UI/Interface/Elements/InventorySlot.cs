using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TMP_Text amountText;
        
        [SerializeField] private InventoryItem item;
        public InventoryItem Item => item;
        
        public void SetItem(InventoryItem item)
        {
            this.item = item;
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            if (item == null)
            {
                icon.sprite = null;
                icon.enabled = false;
                amountText.text = "";
                //nameText.text = "";
                return;
            }
            
            icon.sprite = item.Item.icon;
            icon.enabled = true;
            amountText.text = item.Count.ToString();
            //nameText.text = item.Item.name;
        }
    }
}