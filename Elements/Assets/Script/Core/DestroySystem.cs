using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementGame.Pool;

namespace ElementGame.Core
{
    public class DestroySystem
    {
        private readonly ObjectPoolService _pool;

        public DestroySystem(ObjectPoolService pool)
        {
            _pool = pool;
        }

        public IEnumerator RemoveWithAnimation(Board board, List<Cell> cells)
        {
            bool done = false;
            var tracker = new AnimationTracker();
            tracker.Setup(() => done = true);

            foreach (var cell in cells)
            {
                Element element = cell.Element;
                ElementView view = element.View;

                tracker.Register();

                view.PlayDestroy(() =>
                {
                    ReturnImmediate(view);
                    tracker.Complete();
                });
            }

            yield return new WaitUntil(() => done);

            foreach (var cell in cells)
            {
                if (cell.Element != null)
                    cell.Element.View = null;

                board.ClearCell(cell.Position);
            }
        }

        public void ReturnImmediate(APoolableObject poolableObject)
        {
            _pool.Return(poolableObject);
        }
    }
}
