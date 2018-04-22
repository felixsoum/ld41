using UnityEngine;

public class SliderCollider : MonoBehaviour
{
    public delegate void SliderHandler();
    public event SliderHandler OnSliderStart;
    public event SliderHandler OnSliderFinish;

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
        if (isSliderStarted && !isSliderFinished)
        {
            isSliderFinished = true;
            OnSliderFinish();
        }
    }
}
