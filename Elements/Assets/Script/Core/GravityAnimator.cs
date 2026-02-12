using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementGame.Core
{
    public class GravityAnimator
    {
        public IEnumerator Play(Board board, List<FallData> falls, Func<Vector2Int, Vector3> gridToWorld)
        {
            bool done = false;
            var tracker = new AnimationTracker();
            tracker.Setup(() => done = true);

            foreach (var fall in falls)
            {
                tracker.Register();

                ElementView view = fall.Element.View;
                Vector3 target = gridToWorld(fall.To);

                board.MoveElement(fall.From, fall.To);
                view.PlayMove(target, tracker.Complete);
            }

            yield return new WaitUntil(() => done);
        }

    }
}
