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
            int remaining = 0;

            foreach (var cell in cells)
            {
                Element element = cell.Element;
                if (element == null) continue;

                ElementView view = element.View;

                element.View = null;
                board.ClearCell(cell.Position);

                remaining++;

                view.PlayDestroy(() =>
                {
                    _pool.Return(view);
                    remaining--;
                });
            }

            while (remaining > 0)
                yield return null;
        }


        public void ReturnImmediate(APoolableObject poolableObject)
        {
            _pool.Return(poolableObject);
        }
    }
}
