using UnityEngine;

public class Victim : MonoBehaviour
{
    public SpriteRenderer aliveSprite;
    public SpriteRenderer deadSprite;

    const float TimeBeforeRemove = 2;

    public void Kill()
    {
        aliveSprite.enabled = false;
        deadSprite.enabled = true;
        Invoke("Remove", TimeBeforeRemove);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

}
