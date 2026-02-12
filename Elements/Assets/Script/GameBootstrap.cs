using ElementGame.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElementGame
{
    public class GameBootstrap : MonoBehaviour
    {
        private string _gameSceneName = "Game";

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(_gameSceneName);
        }
    }
}
