using UnityEngine;

namespace Source.Data.Entities
{
    public class PlayerEntity
    {
        public float Speed { get; set; }
        public bool IsMovementEnable { get; set; }
        
        public PlayerEntity(float initialSpeed)
        {
            Speed = initialSpeed;
        }
    }
}