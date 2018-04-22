using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : Beat
{
    public SliderCollider sliderCollider;
    public LineRenderer lineRenderer;
    public GameObject endObject;
    public double TimingEnd { get; set; }

    const float maxLength = 4;
    bool isSliding;

    public override void Start()
    {
        base.Start();
        sliderCollider.OnSliderStart += SliderCollider_OnSliderStart;
        sliderCollider.OnSliderFinish += SliderCollider_OnSliderFinish;
        isColliderStationary = false;
        endObject.transform.position = GetEndPosition();
    }

    public Vector3 GetEndPosition()
    {
        Vector3 endPos = endObject.transform.position;
        endPos.x = -transform.position.x;
        endPos.y = -transform.position.y;

        if (new Vector2(transform.position.x, transform.position.y).magnitude * 2 > maxLength)
        {
            Vector3 dir = endPos - transform.position;
            dir.Normalize();
            endPos = transform.position + dir * maxLength;
        }

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
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endObject.transform.position);
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
}
