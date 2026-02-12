using System.Collections;
using UnityEngine;

namespace ElementGame.Core
{
    public class GameStateMachine
    {
        private readonly MonoBehaviour _host;
        private IGameState _currentState;

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
            _currentState = state;
            yield return state.Enter();
        }

        public IEnumerator RunSubState(IGameState state)
        {
            yield return state.Enter();
        }
    }
}
