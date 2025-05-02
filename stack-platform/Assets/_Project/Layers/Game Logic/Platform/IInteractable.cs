using _Project.Layers.Data.Entities;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Platform
{
    public interface IInteractable<T> : IPlatformData
    {
        T GetReference();
    }
}
