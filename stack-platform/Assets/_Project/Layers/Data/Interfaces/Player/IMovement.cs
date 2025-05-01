using UnityEngine;

namespace _Project.Layers.Data.Interfaces.Player
{
    public interface IMovement
    {
        Vector3 GetDelta(float speed);
    }
}
