using UnityEngine;

namespace FPS
{
    public class TPAnimationEvents : MonoBehaviour
    {

        #region UNITY

        private void Awake()
        {
        }

        #endregion
		
        #region ANIMATION

        private void OnAmmunitionFill(int amount = 0)
        {
        }

        private void OnGrenade()
        {
        }
        private void OnSetActiveMagazine(int active)
        {
        }
		
        private void OnAnimationEndedBolt()
        {
        }
        private void OnAnimationEndedReload()
        {
        }

        private void OnAnimationEndedGrenadeThrow()
        {
        }
        private void OnAnimationEndedMelee()
        {
        }

        private void OnAnimationEndedInspect()
        {
        }
        private void OnAnimationEndedHolster()
        {
        }
		
        private void OnEjectCasing()
        {
        }

        private void OnSlideBack()
        {
        }

        private void OnSetActiveKnife()
        {
        }

        /// <summary>
        /// Spawns a magazine! This function is called from an Animation Event.
        /// </summary>
        private void OnDropMagazine(int drop = 0)
        {
            //todo: Drop the magazine.
        }

        #endregion
    }   
}