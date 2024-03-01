using System;

using UnityEngine;

// ignore never used warning
#pragma warning disable 67

namespace FPS
{
    [AddComponentMenu("FPS/FPSMovement")]
    [RequireComponent(typeof(FPSCharacter), typeof(CharacterController))]
    public class FPSMovement : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private FPSCharacter character;
        [SerializeField] private CharacterController cc;
        
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float runSpeed = 5f;

        [Header("Air & Gravity")] 
        [SerializeField] private float maxJumpHeight = 2f;
        [SerializeField] private float airSpeedMultiplier = 1f;
        [SerializeField] private float gravityMultiplier = 1f;

        [Header("Ground Checker")] 
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float radius = .5f;
        [SerializeField] private float distance = 1f;
        
        [Space]
        [Line]
        [SerializeField, Disable] public Vector3 velocity;
        
        private new Collider collider;
        
        private Vector3 moveDir;
        private bool jump;
        private bool sprint;
        private bool crouch;

        private bool wasGrounded;
        private bool isGrounded;
        
        private float currentGravityMultiplier;

        public event Action OnJump;
        public event Action OnLanded;

        private void Start()
        {
            if (!character)
                character = GetComponent<FPSCharacter>();

            if (!cc)
                cc = GetComponent<CharacterController>();

            collider = cc;

            if (!character)
            {
                Debug.LogError($"No FPSCharacter found on the Movement GameObject {name}");
                enabled = false;
            }
            else
            {
                character.OnInputUpdated += OnInputUpdate;
            }
            
            currentGravityMultiplier = gravityMultiplier;
        }

        private void Update()
        {
            CheckGrounded();
            
            Move();
            Jump();
            ApplyGravity();

            ApplyVelocity();
        }

        private void CheckGrounded()
        {
            isGrounded = Physics.CheckSphere(transform.position + Vector3.down * distance, radius, groundLayer);
        }
        
        private void Move()
        {
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f * currentGravityMultiplier;
            }
            float speed = isGrounded ? (sprint ? runSpeed : walkSpeed) : walkSpeed * airSpeedMultiplier;

            velocity = (speed * moveDir) + Vector3.up * velocity.y;
        }
        
        private void ApplyGravity()
        {
            velocity.y += Physics.gravity.y * Time.deltaTime * currentGravityMultiplier;
        }


        private void Jump()
        {
            if (jump && isGrounded)
            {
                velocity.y += Mathf.Sqrt(maxJumpHeight * -2 * Physics.gravity.y);
            }
        }
        
        private void ApplyVelocity()
        {
            if (currentGravityMultiplier == 0)
            {
                velocity.y = 0;
            }
            cc.Move(velocity * Time.deltaTime);
        }

        #region Events 

        private void OnInputUpdate(ref PlayerInput input)
        {
            Transform transform1 = this.transform;
            moveDir = transform1.right * input.MoveDir.x + transform1.forward * input.MoveDir.y;

            jump = input.Jump;
            sprint = input.Sprint;
            crouch = input.CrouchDown;
        }
        
        #endregion

        public Vector3 GetVelocity() => cc.velocity;
        
        public bool IsGrounded => isGrounded;
        
        public void SetGravityMultiplier(float multiplier)
        {
            currentGravityMultiplier = multiplier;
        }
        
        public void ResetGravityMultiplier()
        {
            currentGravityMultiplier = gravityMultiplier;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * distance, radius);
        }
#endif
    }
}

#pragma warning restore 67