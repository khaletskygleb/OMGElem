namespace ElementGame.Core
{
    public class ElementViewStateMachine
    {
        private ElementViewState _current;

        public void SetState(ElementViewState newState)
        {
            _current?.Exit();
            _current = newState;
            _current.Enter();
        }
    }
}