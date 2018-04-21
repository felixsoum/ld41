using System;
using UnityEngine;

public class Beat : MonoBehaviour
{
    public SpriteRenderer timingRenderer;
    public SpriteRenderer beatRenderer;

    public GameObject victimPrefab;
    Victim victim;

    float timingObjectInitialScale = 4;

    public const double ShrinkTime = 1;
    public const double DelayTime = 0.5f;
    public const float ErrorTreshhold = 0.25f;

    public double Timing { get; set; }
    public bool IsDone { get; set; }

    public delegate void BeatHandler(bool b);
    public event BeatHandler OnBeatDone;

    double lastTime;

    void Start()
    {
        Vector3 victimPos = transform.position;
        victimPos.z = 10 + victimPos.y;
        victim = Instantiate(victimPrefab, victimPos, Quaternion.identity).GetComponent<Victim>();
    }

    public void UpdateTime(double time)
    {
        lastTime = time;
        double timingRatio = 1 - (Timing - time) / ShrinkTime;
        timingRenderer.transform.localScale = Vector3.Lerp(timingObjectInitialScale * Vector3.one, Vector3.one, Mathf.Clamp01((float)timingRatio));

        Color color = timingRenderer.color;
        color.a = (float)timingRatio;
        timingRenderer.color = color;

        if (!IsDone)
        {
            IsDone = time > Timing + DelayTime;
            if (IsDone)
            {
                OnBeatDone(false);
                victim.Remove();
                victim = null;
            }
        }
    }

    public void Kill()
    {
        if (victim)
        {
            victim.Kill();
        }
        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        IsDone = true;
        beatRenderer.enabled = false;
        timingRenderer.enabled = false;
        OnBeatDone(Mathf.Abs((float)(lastTime - Timing)) <= ErrorTreshhold);
    }
}
