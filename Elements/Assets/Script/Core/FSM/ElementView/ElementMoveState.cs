using DG.Tweening;
using UnityEngine;

namespace ElementGame.Core
{
    public class ElementMoveState : ElementViewState
    {
        private Vector3 _target;

        public ElementMoveState(ElementViewContext context, ElementViewStateMachine fsm, Vector3 target)
            : base(context, fsm)
        {
            _target = target;
        }

        public override void Enter()
        {
            Context.MoveTween?.Kill();

            Context.MoveTween = Context.View.transform.DOMove(_target, Context.MoveSpeed)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    Context.Callback?.Invoke();
                    Context.ClearCallback();
                    Fsm.SetState(new ElementIdleState(Context, Fsm));
                });
        }

        public override void Exit()
        {
            Context.MoveTween?.Kill();
        }
    }
}