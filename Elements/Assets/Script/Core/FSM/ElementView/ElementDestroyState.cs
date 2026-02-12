namespace ElementGame.Core
{
    public class ElementDestroyState : ElementViewState
    {
        public ElementDestroyState(ElementViewContext context, ElementViewStateMachine fsm)
            : base(context, fsm) { }

        public override void Enter()
        {
            Context.Animator.ResetTrigger("Death");
            Context.Animator.SetTrigger("Death");
        }
    }
}