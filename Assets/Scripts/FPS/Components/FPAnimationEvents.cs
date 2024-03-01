using System;
using Inventory;
using UnityEngine;

namespace FPS
{
	/// <summary>
	/// Handles all the animation events that come from the character in the asset.
	/// </summary>
	public class FPAnimationEvents : MonoBehaviour
	{
		private FPSCharacter character;
		private FPSInventory inventory;

		private void Awake()
		{
			character = GetComponentInParent<FPSCharacter>();
			inventory = character.FPSInventory ? character.FPSInventory : GetComponentInParent<FPSInventory>();
		}

		#region ANIMATION

		/// <summary>
		/// Ejects a casing from the character's equipped weapon. This function is called from an Animation Event.
		/// </summary>
		private void OnEjectCasing()
		{
			//todo: Notify the character.
		}

		/// <summary>
		/// Fills the character's equipped weapon's ammunition by a certain amount, or fully if set to 0. This function is called
		/// from a Animation Event.
		/// </summary>
		private void OnAmmunitionFill(int amount = 0)
		{
			//todo: Notify the character.
			if (inventory)
			{
				Weapon weapon = inventory.GetCurrentWeapon();
				if (!weapon) return;
				
				weapon.RefillAmmo();
			}
		}

		/// <summary>
		/// Sets the character's knife active value. This function is called from an Animation Event.
		/// </summary>
		private void OnSetActiveKnife(int active)
		{
			//todo: Notify the character.
		}

		/// <summary>
		/// Spawns a grenade at the correct location. This function is called from an Animation Event.
		/// </summary>
		private void OnGrenade()
		{
			//todo: Notify the character.
		}

		/// <summary>
		/// Sets the equipped weapon's magazine to be active or inactive! This function is called from an Animation Event.
		/// </summary>
		private void OnSetActiveMagazine(int active)
		{
			//todo: Notify the character.
		}

		/// <summary>
		/// Bolt Animation Ended. This function is called from an Animation Event.
		/// </summary>
		private void OnAnimationEndedBolt()
		{
			//todo: Notify the character.
		}

		/// <summary>
		/// Reload Animation Ended. This function is called from an Animation Event.
		/// </summary>
		private void OnAnimationEndedReload()
		{
			//todo: Notify the character.
		}

		/// <summary>
		/// Grenade Throw Animation Ended. This function is called from an Animation Event.
		/// </summary>
		private void OnAnimationEndedGrenadeThrow()
		{
			//todo: Notify the character.
		}

		/// <summary>
		/// Melee Animation Ended. This function is called from an Animation Event.
		/// </summary>
		private void OnAnimationEndedMelee()
		{
			//todo: Notify the character.
		}

		/// <summary>
		/// Inspect Animation Ended. This function is called from an Animation Event.
		/// </summary>
		private void OnAnimationEndedInspect()
		{
			//todo: Notify the character.
		}

		/// <summary>
		/// Holster Animation Ended. This function is called from an Animation Event.
		/// </summary>
		private void OnAnimationEndedHolster()
		{
			//todo: Notify the character.
		}

		/// <summary>
		/// Sets the character's equipped weapon's slide back pose. This function is called from an Animation Event.
		/// </summary>
		private void OnSlideBack(int back)
		{
			//todo: Notify the character.
		}

		#endregion
	}

}