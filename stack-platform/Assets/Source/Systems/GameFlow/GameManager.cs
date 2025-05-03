using Source.Core.Utilities.External;
using Source.Gameplay.Player.Services;
using Source.Infrastructure.Signals;
using Zenject;

namespace Source.Systems.GameFlow
{
    public class GameManager
    {
        private readonly SignalBus _signalBus;
        public bool IsGameStarted { get; private set; } = false;
        
        public GameManager(SignalBus signalBus)
        {
            _signalBus = signalBus;

            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus)
            );
        }
        
        public void StartGame()
        {
            if (IsGameStarted)
                return;

            IsGameStarted = true;

            _signalBus.Fire<GameStartedSignal>();
        }
        
        public void ResetGame()
        {
            IsGameStarted = false;
        }
    }
}