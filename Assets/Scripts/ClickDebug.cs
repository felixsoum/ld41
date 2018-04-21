using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDebug : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log(gameObject.name);
    }
}
