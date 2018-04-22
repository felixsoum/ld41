using System;
using UnityEngine;

public class Beat : MonoBehaviour
{
    public SpriteRenderer timingRenderer;
    public SpriteRenderer beatRenderer;
    public new Collider2D collider;
    public AudioSource stabAudio;

    public GameObject victimPrefab;
    protected Victim victim;

    public const float timingObjectInitialScale = 4;

    public const double ShrinkTime = 1;
    public const double DelayTime = 0.5f;
    public const float ErrorTreshhold = 0.25f;

    public double Timing { get; set; }
    public bool IsDone { get; set; }

    public delegate void BeatHandler(Beat beat, bool b);
    public event BeatHandler OnBeatDone;

    protected double lastTime;
    protected bool isFadingOut;
    protected const float FadeSpeed = 15;
    protected bool isColliderStationary = true;

    public virtual void Start()
    {
        Vector3 victimPos = transform.position;
        victim = Instantiate(victimPrefab, victimPos, Quaternion.identity).GetComponent<Victim>();

        var timingPos = timingRenderer.transform.position;
        timingPos.z = 0;
        timingRenderer.transform.position = timingPos;
    }

    protected virtual void Update()
    {
        if (isFadingOut)
        {
            beatRenderer.transform.localScale = Vector3.Lerp(beatRenderer.transform.localScale, Vector3.one * 1.5f, FadeSpeed * Time.deltaTime);
            Color color = beatRenderer.color;
            color.a = 0.25f;
            beatRenderer.color = Color.Lerp(beatRenderer.color, color, FadeSpeed * Time.deltaTime);
        }
    }

    public virtual void UpdateTime(double time)
    {
        lastTime = time;
        double timingRatio = 1 - (Timing - time) / ShrinkTime;
        timingRenderer.transform.localScale = Vector3.Lerp(timingObjectInitialScale * Vector3.one, Vector3.one, Mathf.Clamp01((float)timingRatio));

        Color color = timingRenderer.color;
        color.a = (float)timingRatio;
        timingRenderer.color = color;

        CheckDoneCondition(time);
    }

    public virtual double GetEndTiming()
    {
        return Timing;
    }

    public virtual void CheckDoneCondition(double time)
    {
        if (!IsDone)
        {
            IsDone = time > Timing + DelayTime;
            if (IsDone)
            {
                Fail();
            }
        }
    }

    public void Kill(bool isPlayerLeft = false)
    {
        if (victim)
        {
            victim.Kill(isPlayerLeft);
        }
        collider.enabled = false;
        timingRenderer.enabled = false;
        isFadingOut = true;
        Invoke("Remove", 0.2f);
    }

    void OnMouseDown()
    {
        if (isColliderStationary)
        {
            IsDone = true;
            bool isSuccess = Mathf.Abs((float)(lastTime - Timing)) <= ErrorTreshhold;
            if (isSuccess)
            {
                stabAudio.Play();
            }
            OnBeatDone(this, isSuccess);
            collider.enabled = false;
            isFadingOut = true;
        }
    }

    void Remove()
    {
        Destroy(gameObject);
    }

    protected void DoBeatDone(bool isSuccess)
    {
        OnBeatDone(this, isSuccess);
    }

    protected void Fail()
    {
        OnBeatDone(this, false);
        victim.Remove();
        victim = null;
    }

    public virtual Vector3 GetPosForPlayer()
    {
        return transform.position;
    }
}
