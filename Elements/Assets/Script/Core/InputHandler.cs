using UnityEngine;

namespace ElementGame.Core
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private GameController _gameController;
        [SerializeField, Min(0)] private float _swipeThreshold = 0.25f;

        private Vector2 _startWorldPos;
        private ElementView _startElement;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
                _startElement = RaycastElement(_startWorldPos);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_startElement == null) return;

                Vector2 endWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 delta = endWorldPos - _startWorldPos;

                if (delta.magnitude < _swipeThreshold)
                {
                    _startElement = null;
                    return;
                }

                Vector2Int dir = GetSwipeDirection(delta);
                Vector2Int from = _startElement.Element.Position;
                Vector2Int to = from + dir;

                _gameController.TryPlayerMove(from, to);

                _startElement = null;
            }
        }

        private ElementView RaycastElement(Vector2 worldPos)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (!hit) return null;
            return hit.collider.GetComponent<ElementView>();
        }

        private Vector2Int GetSwipeDirection(Vector2 delta)
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                return delta.x > 0 ? Vector2Int.right : Vector2Int.left;
            else
                return delta.y > 0 ? Vector2Int.up : Vector2Int.down;
        }
    }
}