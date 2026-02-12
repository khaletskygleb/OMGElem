namespace ElementGame.Core
{
    public abstract class ElementViewState
    {
        protected ElementViewContext Context;
        protected ElementViewStateMachine Fsm;

        protected ElementViewState(ElementViewContext context, ElementViewStateMachine fsm)
        {
            Context = context;
            Fsm = fsm;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
    }
}