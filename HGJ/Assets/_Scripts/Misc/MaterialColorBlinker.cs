using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorBlinker : MonoBehaviour
{
    protected Material material;

    protected Color regularColor;
    private Color targetColor;

    private bool goingRegular;

    public float red;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        targetColor = regularColor = material.color;
    }

    private void Update()
    {
        material.color = Color.Lerp(material.color, targetColor, 3 * Time.unscaledDeltaTime / Mathf.Abs(material.color.CombinedValue() - targetColor.CombinedValue()));
        if(material.color == targetColor)
        {
            goingRegular = !goingRegular;
            if (goingRegular) targetColor = regularColor;
            else targetColor = Color.red;
            red = material.color.r;
        }
    }
}
