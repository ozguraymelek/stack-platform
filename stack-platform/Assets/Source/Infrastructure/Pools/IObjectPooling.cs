using Source.Gameplay.Platform;
using Source.Gameplay.Platform.Wrappers;

namespace Source.Infrastructure.Pools
{
    public interface IObjectPool
    {
        IInteractable<Platform> Get();
        void Release(IInteractable<Platform> item);
    }
}