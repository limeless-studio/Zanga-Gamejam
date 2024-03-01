using System.Globalization;
using TMPro;
using UnityEngine;

namespace UI
{
    public class AmmoElement : Element
    {
        #region FIELDS SERIALIZED
        [Header("References")]
        [SerializeField] private TMP_Text ammoText;
        [SerializeField] private TMP_Text maxAmmoText;
        
        [Header("Colors")]
        
        [Tooltip("Determines if the color of the text should changes as ammunition is fired.")]
        [SerializeField]
        private bool updateColor = true;
        
        [Tooltip("Determines how fast the color changes as the ammunition is fired.")]
        [SerializeField]
        private float emptySpeed = 1.5f;
        
        [Tooltip("Color used on this text when the player character has no ammunition.")]
        [SerializeField]
        private Color emptyColor = Color.red;
        
        #endregion

        protected override void Tick()
        {
            if (character && character.FPSInventory)
            {
                float current = 0;
                float total = 0;
                bool isValid = true;
                var weapon = character.FPSInventory.GetCurrentWeapon();
                if (weapon)
                {
                    current = weapon.GetAmmo();
                    total = weapon.GetMaxAmmo();
                } else if (character.FPSInventory.TryGetInHand(out var item) && item.InvItem.IsStackable)
                {
                    current = item.InvItem.Count;
                    total = item.InvItem.MaxStack;
                }
                else
                {
                    isValid = false;
                }
                
                ammoText.text = isValid ? current.ToString(CultureInfo.InvariantCulture) : "--";
                maxAmmoText.text = isValid ? total.ToString(CultureInfo.InvariantCulture) : "--";
                
                if (updateColor && isValid)
                {
                    float colorAlpha = (current / total) * emptySpeed;
                    ammoText.color = Color.Lerp(emptyColor, Color.white, colorAlpha);   
                }
                
            }
            base.Tick();
        }
    }
}