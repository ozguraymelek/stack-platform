using Source.Core.Utilities.External;
using Source.Infrastructure.Signals;
using Source.Systems.GameFlow;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject;

namespace Source.UI.Managers
{
    public class UIManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        private GameManager _gameManager;
        private LevelManager _levelManager;
        
        [SerializeField] private GameObject levelTransitionPanel;
        [SerializeField] private GameObject gameFailedPanel;
        
        [SerializeField] private TMP_Text currentLevelText;
        
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
            levelTransitionPanel.SetActive(true);
        }
        private void OnGameFailed()
        {
            gameFailedPanel.SetActive(true);
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
            _levelManager.NextLevel(currentLevelText);
        }
    }
}