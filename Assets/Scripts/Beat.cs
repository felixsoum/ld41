using System;
using UnityEngine;

public class Beat : MonoBehaviour
{
    public GameObject timingObject;
    public SpriteRenderer timingRenderer;
    public SpriteRenderer beatRenderer;
    float timingObjectInitialScale = 4;

    public const double ShrinkTime = 1f;
    public const double DelayTime = 0.5f;

    public double Timing { get; set; }
    public bool IsDone { get; set; }

    public void UpdateTime(double time)
    {
        double timingRatio = 1 - (Timing - time) / ShrinkTime;
        timingObject.transform.localScale = Vector3.Lerp(timingObjectInitialScale * Vector3.one, Vector3.one, Mathf.Clamp01((float)timingRatio));

        if (!IsDone)
        {
            IsDone = time > Timing + DelayTime;
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        IsDone = true;
        beatRenderer.enabled = false;
        timingRenderer.enabled = false;
    }
}
