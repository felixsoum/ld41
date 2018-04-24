using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image image;

    void Update()
    {
        Color color = image.color;
        color.a = 1;
        image.color = Color.Lerp(image.color, color, 2 * Time.deltaTime);
    }

}
