using UnityEngine;

namespace FPS
{
    public abstract class Motion : MonoBehaviour
    {
        #region PROPERTIES
        
        /// <summary>
        /// Alpha.
        /// </summary>
        public float Alpha => alpha;
        
        #endregion
        
        #region FIELDS SERIALIZED
        
        [Header("Motion")]
        
        [Tooltip("The Motion's alpha. Used to more easily control how much of the motion is applied.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float alpha = 1.0f;

        [Header("References")]
        
        [Tooltip("The MotionApplier that will apply this Motion's values.")]
        [SerializeField]
        protected FPSMotionApplier motionApplier;
        
        #endregion
        
        #region METHODS
        
        /// <summary>
        /// Awake.
        /// </summary>
        protected virtual void Awake()
        {
            //Try to get the applier if we haven't assigned it.
            if (motionApplier == null)
                motionApplier = GetComponentInParent<FPSMotionApplier>();
            
            //Subscribe.
            if(motionApplier != null)
                motionApplier.Subscribe(this);
        }

        /// <summary>
        /// Tick.
        /// </summary>
        public abstract void Tick();
        
        #endregion
        
        #region FUNCTIONS
        
        /// <summary>
        /// GetLocation.
        /// </summary>
        public abstract Vector3 GetLocation();
        /// <summary>
        /// GetEulerAngles.
        /// </summary>
        public abstract Vector3 GetEulerAngles();
        
        #endregion
    }
}