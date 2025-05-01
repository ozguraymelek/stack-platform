using _Project.Layers.Data.Interfaces.Player;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Player
{
    public class EndlessMovement : IMovement
    {
        public Vector3 GetDelta(float speed)
        {
            return Vector3.forward * speed;
        }
    }
}
