using UnityEngine;

namespace FPS
{
    public abstract class AnimatorHashes
    {
        public static readonly int Movement = Animator.StringToHash("Movement");
        public static readonly int Horizontal = Animator.StringToHash("Horizontal");
        public static readonly int Vertical = Animator.StringToHash("Vertical");
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int Aim = Animator.StringToHash("Aim");
        public static readonly int Reloading = Animator.StringToHash("Reloading");
    }
}