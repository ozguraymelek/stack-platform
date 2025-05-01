using _Project.Layers.Game_Logic.Player;
using _Project.Layers.Game_Logic.Signals;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Game_Flow
{
    public class GameManager
    {
        private readonly PlayerMovement _playerMovement;
        private readonly SignalBus _signalBus;
        public bool IsGameStarted { get; private set; } = false;
        
        public GameManager(PlayerMovement playerMovement, SignalBus signalBus)
        {
            _playerMovement = playerMovement;
            _signalBus = signalBus;
        }
        
        public void StartGame()
        {
            if (IsGameStarted)
                return;

            IsGameStarted = true;

            // _playerMovement.EnableMovement();
            _signalBus.Fire<GameStartedSignal>();
        }
        
        public void ResetGame()
        {
            IsGameStarted = false;
            // _playerMovement.DisableMovement();
        }
    }
}