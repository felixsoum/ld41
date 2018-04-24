using UnityEngine;

public class SliderCollider : MonoBehaviour
{
    public delegate void SliderColliderHandler();
    public event SliderColliderHandler OnSliderStart;
    public event SliderColliderHandler OnSliderFinish;

    bool isSliderStarted;
    bool isSliderFinished;

    void OnMouseDown()
    {
        isSliderStarted = true;
        OnSliderStart();
    }

    void OnMouseUp()
    {
        if (isSliderStarted && !isSliderFinished)
        {
            isSliderFinished = true;
            OnSliderFinish();
        }
    }

    void OnMouseExit()
    {
        //if (isSliderStarted && !isSliderFinished)
        //{
        //    isSliderFinished = true;
        //    OnSliderFinish();
        //}
    }
}
