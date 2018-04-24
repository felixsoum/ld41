using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer idleRenderer;
    public SpriteRenderer stabRenderer;

    const float offset = 0.5f;

    void Update()
    {
        bool isMouseDown = Input.GetMouseButton(0);
        idleRenderer.enabled = !isMouseDown;
        stabRenderer.enabled = !idleRenderer.enabled;

        Vector3 pos = transform.position;
        pos.z = 9.999f + pos.y;
        transform.position = pos;

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(Mathf.Sign(transform.localScale.x) * 1, 1, 1), 2 * Time.deltaTime);
    }

    public void MoveTo(Vector3 pos)
    {
        bool isLeft = pos.x < transform.position.x;

        float currentOffset = offset;
        var scale = Vector3.one;
        if (!isLeft)
        {
            currentOffset *= -1;
            scale.x *= -1;
        }
        pos.x += currentOffset;

        transform.position = pos;
        transform.localScale = scale;
    }

    public void SlideTo(Vector3 pos, Vector3 dir)
    {
        bool isLeft = dir.x < 0;

        float currentOffset = offset;
        var scale = Vector3.one;
        if (!isLeft)
        {
            currentOffset *= -1;
            scale.x *= -1;
        }
        pos.x += currentOffset;

        transform.position = pos;
        transform.localScale = scale;
    }
}
