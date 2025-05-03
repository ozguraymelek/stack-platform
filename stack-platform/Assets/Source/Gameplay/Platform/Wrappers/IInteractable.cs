using Source.Data.Entities;
using Source.Systems.Effects;

namespace Source.Gameplay.Platform.Wrappers
{
    public interface IInteractable<T> : IPlatformData
    {
        T GetReference();
        EdgeOutline GetOutline();
    }
}