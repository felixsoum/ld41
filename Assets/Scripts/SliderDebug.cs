using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderDebug : MonoBehaviour
{
    void OnMouseExit()
    {
        Debug.Log("exit");
    }

    void OnMouseUp()
    {
        Debug.Log("up");
    }
}
