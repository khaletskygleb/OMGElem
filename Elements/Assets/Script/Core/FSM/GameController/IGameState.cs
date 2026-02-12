using System.Collections;

namespace ElementGame.Core
{
    public interface IGameState
    {
        IEnumerator Enter();
    }
}