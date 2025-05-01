using _Project.Layers.Game_Logic.Game_Flow;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Presentation
{
    public class UIManager : MonoBehaviour
    {
        [Inject] private GameManager _gameManager;

        public void PlayButtonOnClicked()
        {
            _gameManager.StartGame();
        }
    }
}
