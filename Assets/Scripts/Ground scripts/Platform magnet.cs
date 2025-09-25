using UnityEngine;

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlatformMojo : MonoBehaviour
{
    private Transform characterOnPlatform;
    private bool isCharacterOnPlatform = false;

    public Vector2 horizontalLimits = new Vector2(-2f, 2f);
    public Vector2 verticalLimits = new Vector2(-0.4f, 0.4f);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsHeroPlayerEnemyTag(other.tag))
        {
            characterOnPlatform = other.transform;
            isCharacterOnPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == characterOnPlatform)
        {
            isCharacterOnPlatform = false;
            characterOnPlatform = null;
        }
    }

    private void LateUpdate()
    {
        if (!isCharacterOnPlatform || characterOnPlatform == null)
            return;

        Vector3 pos = characterOnPlatform.localPosition;

        // Приводим позицию к диапазонам относительно платформы
        pos.x = Mathf.Clamp(pos.x, horizontalLimits.x, horizontalLimits.y);
        pos.y = Mathf.Clamp(pos.y, verticalLimits.x, verticalLimits.y);

        characterOnPlatform.localPosition = pos;
    }

    private bool IsHeroPlayerEnemyTag(string tagName)
    {
        return tagName == "hero" || tagName == "player" || tagName == "enemy";
    }

}
