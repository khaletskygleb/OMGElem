using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoScaleBackground : MonoBehaviour
{
    public void ScaleToCamera()
    {
        var spriteRenerer = GetComponent<SpriteRenderer>();

        var screenHeight = Camera.main.orthographicSize * 2f;
        var screenWidth = screenHeight * Screen.width / Screen.height;

        Vector2 spriteSize = spriteRenerer.sprite.bounds.size;

        transform.localScale = new Vector3(
            screenWidth / spriteSize.x,
            screenHeight / spriteSize.y,
            default
        );
    }
}
