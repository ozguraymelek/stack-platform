using _Project.Layers.Game_Logic.Game_Flow;
using _Project.Layers.Game_Logic.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Presentation
{
    public class UIManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        public GameObject LevelTransitionPanel;
        
        private GameManager _gameManager;
        private LevelManager _levelManager;
        
        public TMP_Text CurrentLevelText;
        
        [Inject]
        public void Construct(SignalBus signalBus, GameManager gameManager, LevelManager levelManager)
        {
            _signalBus = signalBus;
            _gameManager = gameManager;
            _levelManager = levelManager;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<LevelFinishedSignal>(OnLevelFinished);
        }
        private void OnDisable()
        {
            _signalBus.Unsubscribe<LevelFinishedSignal>(OnLevelFinished);
        }
        

        private void OnLevelFinished()
        {
            LevelTransitionPanel.SetActive(true);
        }
        
        public void PlayButtonOnClicked()
        {
            _gameManager.StartGame();
        }
        public void NextLevelButtonOnClicked()
        {
            _levelManager.NextLevel(CurrentLevelText);
        }
    }
}