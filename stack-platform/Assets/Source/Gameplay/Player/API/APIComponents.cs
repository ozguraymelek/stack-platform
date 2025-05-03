using System;
using UnityEngine;

namespace Source.Gameplay.Player.API
{
    [Serializable]
    public struct APIComponents
    {
        public Rigidbody Rigidbody;
        public Collider Collider;
        public Animator Animator;
    }
}