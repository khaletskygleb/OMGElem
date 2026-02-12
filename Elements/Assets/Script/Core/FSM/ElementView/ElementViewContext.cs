using DG.Tweening;
using System;
using UnityEngine;

namespace ElementGame.Core
{
    public class ElementViewContext
    {
        public ElementView View;

        public Animator Animator;
        public float MoveSpeed;

        public Tween MoveTween;
        public Action Callback;

        public Element Element;
        public Board Board;
        public Func<Vector2Int, Vector3> GridToWorld;
        public bool IsDying;

        public void ClearCallback()
        {
            Callback = null;
        }
    }
}