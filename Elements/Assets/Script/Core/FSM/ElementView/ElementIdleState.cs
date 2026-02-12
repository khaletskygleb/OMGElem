namespace ElementGame.Core
{
    public class ElementIdleState : ElementViewState
    {
        public ElementIdleState(ElementViewContext context, ElementViewStateMachine fsm)
            : base(context, fsm) { }

        public override void Enter()
        {
            float randomTime = UnityEngine.Random.value;
            Context.Animator.Play(0, 0, randomTime);
            Context.Animator.Update(0);
        }
    }
}