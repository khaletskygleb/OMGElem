using DG.Tweening;
using ElementGame.Core;
using ElementGame.Pool;
using System;
using UnityEngine;

namespace ElementGame.Core
{
    public class ElementView : APoolableObject
    {
        [SerializeField] private float _animMoveSpeed = 0.15f;
        [SerializeField] private Animator _animator;

        private ElementViewStateMachine _fsm;
        private ElementViewContext _context;

        public Element Element => _context.Element;

        private void Awake()
        {
            _fsm = new ElementViewStateMachine();

            _context = new ElementViewContext
            {
                View = this,
                Animator = _animator,
                MoveSpeed = _animMoveSpeed
            };
        }

        public override void OnGet()
        {
            base.OnGet();

            _context.Animator.Rebind();
            _context.Animator.Update(0f);

            _fsm.SetState(new ElementIdleState(_context, _fsm));
        }

        public override void OnReturnToPool()
        {
            transform.DOKill();
            _context.MoveTween?.Kill();
            _context.ClearCallback();

            _context.IsDying = false;
            _fsm.SetState(new ElementPooledState(_context, _fsm));
        }

        public void Initialize(Element element, Board board, Func<Vector2Int, Vector3> gridToWorld)
        {
            _context.Element = element;
            _context.Board = board;
            _context.GridToWorld = gridToWorld;

            transform.position = _context.GridToWorld(element.Position);
        }

        public void PlayMove(Vector3 target, Action onComplete)
        {
            _context.Callback = onComplete;
            _fsm.SetState(new ElementMoveState(_context, _fsm, target));
        }

        public void PlayDestroy(Action onComplete)
        {
            if (_context.IsDying) 
                return; 

            _context.IsDying = true;
            _context.Callback = onComplete;
            _fsm.SetState(new ElementDestroyState(_context, _fsm));
        }

        public void NotifyDeathFinished()
        {
            _context.Callback?.Invoke();
            _context.ClearCallback();
        }

        public Vector3 GetWorldPosition(Vector2Int gridPos) 
        { 
            return _context.GridToWorld(gridPos); 
        }
    }
}