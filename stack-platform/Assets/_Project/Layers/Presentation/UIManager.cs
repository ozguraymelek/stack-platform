using _Project.Helper.Utils;
using _Project.Layers.Game_Logic.Game_Flow;
using _Project.Layers.Game_Logic.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Project.Layers.Presentation
{
    public class UIManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        public GameObject LevelTransitionPanel;
        public GameObject GameFailedPanel;
        
        private GameManager _gameManager;
        private LevelManager _levelManager;
        
        public TMP_Text CurrentLevelText;
        
        [Inject]
        public void Construct(SignalBus signalBus, GameManager gameManager, LevelManager levelManager)
        {
            _signalBus = signalBus;
            _gameManager = gameManager;
            _levelManager = levelManager;

            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus),
                (nameof(_gameManager), _gameManager),
                (nameof(_levelManager), _levelManager)
            );
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<LevelFinishedSignal>(OnLevelFinished);
            _signalBus.Subscribe<GameFailedSignal>(OnGameFailed);
        }
        private void OnDisable()
        {
            _signalBus.Unsubscribe<LevelFinishedSignal>(OnLevelFinished);
            _signalBus.Unsubscribe<GameFailedSignal>(OnGameFailed);
        }
        
        private void OnLevelFinished()
        {
            LevelTransitionPanel.SetActive(true);
        }
        private void OnGameFailed()
        {
            GameFailedPanel.SetActive(true);
        }
        public void PlayButtonOnClicked()
        {
            _gameManager.StartGame();
        }
        
        public void RestartButtonOnClicked()
        {
            if (!SceneManager.GetActiveScene().IsValid())
            {
                Debug.LogError("There is no active scene in build settings..");
                return;
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void NextLevelButtonOnClicked()
        {
            _levelManager.NextLevel(CurrentLevelText);
        }
    }
}