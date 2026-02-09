using System.Collections;
using System.Collections.Generic;

public static class DestroySystem
{
    public static IEnumerator RemoveWithAnimation(Board board, List<Cell> cells)
    {
        int remaining = 0;

        foreach (var cell in cells)
        {
            if (cell.Element == null) continue;
            if (cell.Element.View == null) continue;

            remaining++;

            ElementView view = cell.Element.View;

            void Handler(ElementView elementView)
            {
                elementView.OnDestroyAnimationFinished -= Handler;
                remaining--;
            }

            view.OnDestroyAnimationFinished += Handler;
            view.PlayDestroyAnimation();
        }

        if (remaining == 0)
            yield break;

        while (remaining > 0)
            yield return null;

        foreach (var cell in cells)
        {
            if (board.GetCell(cell.Position).Element != null)
                board.RemoveElement(cell.Position);
        }
    }
}
