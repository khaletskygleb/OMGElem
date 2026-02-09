using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DynamicIsoSorting : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;

    private Vector3 _lastPos;

    private const int PRECISION = 100;

    private void Start()
    {
        _lastPos = transform.position;
        UpdateSorting();
    }

    private void LateUpdate()
    {
        if (transform.position != _lastPos)
        {
            UpdateSorting();
            _lastPos = transform.position;
        }
    }

    private void UpdateSorting()
    {
        float y = _sprite.bounds.min.y;
        float x = transform.position.x;
        float depth = x + y;

        _sprite.sortingOrder = Mathf.RoundToInt(depth * PRECISION);
    }
}

