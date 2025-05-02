using _Project.Layers.Game_Logic.Platform;
using UnityEngine;

namespace _Project.Layers.Infrastructure.Pools
{
    public interface IObjectPool
    {
        IInteractable<Platform> Get();
        void Release(IInteractable<Platform> item);
    }
}
