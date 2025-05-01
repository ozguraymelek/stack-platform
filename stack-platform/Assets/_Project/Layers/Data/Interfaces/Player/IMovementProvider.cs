using UnityEngine;

namespace _Project.Layers.Data.Interfaces.Player
{
    public interface IMovementProvider
    {
        IMovement Current { get; set; }
        void Set(IMovement movement);
    }
}
