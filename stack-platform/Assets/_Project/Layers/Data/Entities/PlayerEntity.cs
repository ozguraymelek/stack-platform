using UnityEngine;

namespace _Project.Layers.Data.Entities
{
    public class PlayerEntity
    {
        public float Speed { get; set; }
        public Vector3 Position { get; set; }
        public bool IsMovementEnable { get; set; }
        
        public PlayerEntity(float initialSpeed)
        {
            Speed = initialSpeed;
        }
        public void SetSpeed(float newSpeed) => Speed = newSpeed;
    }
}
