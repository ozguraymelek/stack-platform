using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Effect;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Platform
{
    public interface IInteractable<T> : IPlatformData
    {
        T GetReference();
        EdgeOutline GetOutline();
    }
}
