using System.Collections;
using UnityEngine;

namespace ElementGame.Core
{
    public class GameStateMachine
    {
        private readonly MonoBehaviour _host;
        private IGameState _currentState;
        private bool _isBusy;

        public bool IsBusy => _isBusy;

        public GameStateMachine(MonoBehaviour host)
        {
            _host = host;
        }

        public void SetState(IGameState state)
        {
            _host.StartCoroutine(RunState(state));
        }

        private IEnumerator RunState(IGameState state)
        {
            _isBusy = true;
            _currentState = state;

            yield return state.Enter();

            _isBusy = false;
        }

        public IEnumerator RunSubState(IGameState state)
        {
            yield return state.Enter();
        }
    }
}