using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : Beat
{
    public delegate void SliderHandler(Vector3 slidePosition, Vector3 direction);
    public event SliderHandler OnSlide;

    public SpriteRenderer startRenderer;
    public SliderCollider sliderCollider;
    public LineRenderer lineRenderer;
    public GameObject endObject;
    public double TimingEnd { get; set; }

    const float maxLength = 4;
    bool isSliding;
    public Vector3 slideDirection;
    bool isStartFadingOut;

    public override void Start()
    {
        base.Start();
        sliderCollider.OnSliderStart += SliderCollider_OnSliderStart;
        sliderCollider.OnSliderFinish += SliderCollider_OnSliderFinish;
        isColliderStationary = false;
    }

    public void Init()
    {
        endObject.transform.position = GetEndPosition();
        slideDirection = endObject.transform.position - transform.position;
        slideDirection.Normalize();
    }

    public Vector3 GetEndPosition()
    {
        Vector3 endPos = endObject.transform.position;
        endPos.x = -transform.position.x;
        endPos.y = -transform.position.y;

        if (Random.Range(0, 2) == 0)
        {
            endPos.x *= -1;
        }
        else
        {
            endPos.y *= -1;
        }

        Vector3 dir = endPos - transform.position;
        dir.Normalize();
        endPos = transform.position + dir * maxLength;

        return endPos;
    }

    private void SliderCollider_OnSliderStart()
    {
        bool isSuccess = Mathf.Abs((float)(lastTime - Timing)) <= ErrorTreshhold;
        if (!isSuccess)
        {
            IsDone = true;
            Fail();
            collider.enabled = false;
        }
        else
        {
            isSliding = true;
        }
        isStartFadingOut = true;
        Invoke("HideStartBeat", 0.2f);
    }

    private void SliderCollider_OnSliderFinish()
    {
        if (!IsDone)
        {
            IsDone = true;
            bool isSuccess = Mathf.Abs((float)(lastTime - TimingEnd)) <= ErrorTreshhold;
            if (isSuccess)
            {
                stabAudio.Play();
                DoBeatDone(isSuccess);
            }
            else
            {
                Fail();
            }
            collider.enabled = false;
            isFadingOut = true;
        }
    }

    protected override void Update()
    {
        base.Update();
        lineRenderer.SetPosition(0, timingRenderer.transform.position);
        lineRenderer.SetPosition(1, endObject.transform.position);
        if (isStartFadingOut)
        {
            startRenderer.transform.localScale = Vector3.Lerp(startRenderer.transform.localScale, Vector3.one * 1.5f, FadeSpeed * Time.deltaTime);
            Color color = startRenderer.color;
            color.a = 0.25f;
            startRenderer.color = Color.Lerp(startRenderer.color, color, FadeSpeed * Time.deltaTime);
        }
    }

    public override void UpdateTime(double time)
    {
        if (isSliding)
        {
            double timingRatio = 1 - (TimingEnd - time) / (TimingEnd - Timing);
            timingRenderer.transform.position = Vector3.Lerp(transform.position, endObject.transform.position, Mathf.Clamp01((float)timingRatio));
            collider.transform.position = timingRenderer.transform.position;
            if (victim)
            {
                var victimPos = victim.transform.position;
                victimPos.x = timingRenderer.transform.position.x;
                victimPos.y = timingRenderer.transform.position.y;
                victim.transform.position = victimPos;
            }
            OnSlide(timingRenderer.transform.position, slideDirection);
        }
        base.UpdateTime(time);
    }

    public override void CheckDoneCondition(double time)
    {
        if (!IsDone)
        {
            if (!isSliding)
            {
                IsDone = time > Timing + DelayTime;
                if (IsDone)
                {
                    DoBeatDone(false);
                    victim.Remove();
                    victim = null;
                }
            }
            else
            {
                IsDone = time > TimingEnd + DelayTime;
                if (IsDone)
                {
                    DoBeatDone(false);
                    victim.Remove();
                    victim = null;
                }
            }
        }
    }

    public override double GetEndTiming()
    {
        return TimingEnd;
    }

    public override Vector3 GetPosForPlayer()
    {
        return timingRenderer.transform.position;
    }

    void HideStartBeat()
    {
        startRenderer.enabled = false;
    }
}
