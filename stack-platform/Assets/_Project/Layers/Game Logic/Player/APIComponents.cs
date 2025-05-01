using System;
using _Project.Layers.Data.Entities;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Player
{
    [Serializable]
    public struct APIComponents
    {
        public Rigidbody Rigidbody;
        public Collider Collider;
        public Animator Animator;
    }
}
