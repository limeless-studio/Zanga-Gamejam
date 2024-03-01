using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public enum ApplyMode
    {
        Override,
        Add
    }
    
    public class FPSMotionApplier : MonoBehaviour
    {
        #region FIELDS SERIALIZED
        
        [Header("Settings")]

        [Tooltip("Determines the way this component applies the values for all subscribed Motion components.")]
        [SerializeField]
        private ApplyMode applyMode;
        
        [SerializeField] bool debug = true;
        [SerializeField] private Vector3 pos;
        
        #endregion
        
        #region FIELDS
        
        /// <summary>
        /// Subscribed Motions.
        /// </summary>
        private readonly List<Motion> motions = new List<Motion>();

        /// <summary>
        /// This Transform.
        /// </summary>
        private Transform thisTransform;
        
        private FPSCharacter character;

        #endregion
        
        #region METHODS

        /// <summary>
        /// Awake.
        /// </summary>
        private void Awake()
        {
            //Cache.
            thisTransform = transform;
        }
        /// <summary>
        /// LateUpdate.
        /// </summary>
        private void LateUpdate()
        {
            if (debug)
            {
                thisTransform.localPosition = pos;
                return;
            }
            //Final Location.
            Vector3 finalLocation = default;
            //Final Euler Angles.
            Vector3 finaEulerAngles = default;
            
            //ForEach Motion.
            motions.ForEach((motion =>
            {
                //Tick.
                motion.Tick();
                
                //Add Location.
                finalLocation += motion.GetLocation() * motion.Alpha;
                //Add Rotation.
                finaEulerAngles += motion.GetEulerAngles() * motion.Alpha;
            }));

            //Override Mode.
            if(applyMode == ApplyMode.Override)
            {
                //Set Location.
                thisTransform.localPosition = finalLocation;
                //Set Euler Angles.
                thisTransform.localEulerAngles = finaEulerAngles;
            }
            //Add Mode.
            else if (applyMode == ApplyMode.Add)
            {
                //Add Location.
                thisTransform.localPosition += finalLocation;
                //Add Euler Angles.
                thisTransform.localEulerAngles += finaEulerAngles;
            }
        }
        
        /// <summary>
        /// Subscribe a Motion to this MotionApplier. This means that the Motion's results every frame will be computed,
        /// and applied, by this MotionApplier.
        /// </summary>
        public void Subscribe(Motion motion) => motions.Add(motion);
        
        public FPSCharacter GetCharacter()
        {
            if (character == null)
                character = GetComponentInParent<FPSCharacter>();
            return character;
        }
        
        #endregion
    }
}