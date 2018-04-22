using UnityEngine;

public class Victim : MonoBehaviour
{
    public SpriteRenderer aliveSprite;
    public SpriteRenderer deadSprite;
    public SpriteRenderer bloodSprite;

    public Sprite[] sprites;

    const float TimeBeforeRemove = 30;

    bool isKilled;
    Vector3 killOffsetPos = new Vector3(1, -0.5f, 0);
    float killFallAnimSpeed = 10;
    float killFadeAnimSpeed = 1;
    float targetKillRotZ = -90;
    float currentKillRotZ;
    Vector3 deadSpawnPos;

    void Start()
    {
        int victimIndex = Random.Range(0, sprites.Length);
        aliveSprite.sprite = sprites[victimIndex];
        deadSprite.sprite = sprites[victimIndex];
        deadSpawnPos = deadSprite.transform.localPosition;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.z = 10 + pos.y;
        transform.position = pos;

        if (isKilled)
        {
            deadSprite.transform.localPosition = Vector3.Lerp(deadSprite.transform.localPosition,
                deadSpawnPos + killOffsetPos, killFallAnimSpeed * Time.deltaTime);

            currentKillRotZ = Mathf.Lerp(currentKillRotZ, targetKillRotZ, killFallAnimSpeed * Time.deltaTime);
            deadSprite.transform.eulerAngles = new Vector3(0, 0, currentKillRotZ);

            Color color = deadSprite.color;
            color.a = 0;
            deadSprite.color = Color.Lerp(deadSprite.color, color, killFadeAnimSpeed * Time.deltaTime);
        }
    }

    public void Kill(bool isPlayerLeft = false)
    {
        aliveSprite.enabled = false;
        deadSprite.enabled = true;
        bloodSprite.enabled = true;
        isKilled = true;
        Invoke("Remove", TimeBeforeRemove);
        if (!isPlayerLeft)
        {
            killOffsetPos.x *= -1;
            targetKillRotZ *= -1;
        }
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

}
