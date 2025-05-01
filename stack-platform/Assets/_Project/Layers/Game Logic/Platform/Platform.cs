using _Project.Layers.Data.Entities;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Platform
{
    public class Platform : MonoBehaviour, IPlatformInteractable
    {
        public PlatformEntity ToEntity()
        {
            return new PlatformEntity();
        }

        public void ApplyEntity(PlatformEntity entity)
        {
            
        }
    }
}
