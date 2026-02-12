using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;
using System.Collections;
using ElementGame.Core;

namespace ElementGame.UI
{
    public class GameUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _nextButton;

        private GameController _gameController;

        [Inject]
        public void Construct(GameController controller)
        {
            _gameController = controller;
        }

        private void Awake()
        {
            _restartButton.onClick.AddListener(RestartLevel);
            _nextButton.onClick.AddListener(NextLevel);
        }

        private void RestartLevel()
        {
            _gameController.RestartLevel();
        }

        private void NextLevel()
        {
            _gameController.LoadNextLevel();
        }
    }
}