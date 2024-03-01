using Core;
using UnityEngine;

namespace FPS
{
    public struct PlayerInput
    {
        public Vector2 MoveDir;
        public Vector2 LookDir;
        public bool Sprint;
        public bool Jump;
        public bool CrouchDown;
        public bool Crouch;
        public float MouseWheel;
        public ButtonState Attack;
        public ButtonState Aim;
        public ButtonState Interact;
        public ButtonState Reload;
        public ButtonState Inventory;
    }
}