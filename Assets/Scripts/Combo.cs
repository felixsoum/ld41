using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{
    public Text text;
    public float scaleFactor = 2;
    Vector3 initialScale;

    void Awake()
    {
        initialScale = transform.localScale;
    }

    void Start()
    {
        text.text = "0";
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, initialScale, 10 * Time.deltaTime);
    }

    public void SetCombo(int n)
    {
        text.text = n.ToString();
        transform.localScale = initialScale * scaleFactor;
    }


}
