using _Project.Layers.Data.Interfaces.Player;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Player.Services
{
    public class MovementProvider : IMovementProvider, IInitializable
    {
        private readonly EndlessMovement _endless;
        // private readonly SwerveMovement _swerve;
        
        public IMovement Current { get; set; }

        public MovementProvider(EndlessMovement endless /*SwerveMovement */)
        {
            _endless = endless;
            // _swerve = swerve;
        }
        
        public void Set(IMovement movement)
        {
            Current = movement;
        }

        public void Initialize()
        {
            Current = _endless;
        }
    }
}
