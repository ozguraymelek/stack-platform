using _Project.Layers.Data.Entities;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Platform
{
    public interface IPlatformInteractable
    {
        PlatformEntity ToEntity();
        void ApplyEntity(PlatformEntity entity);
    }
}
