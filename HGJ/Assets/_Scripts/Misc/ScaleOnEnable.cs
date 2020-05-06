using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnEnable : MonoBehaviour
{
    Vector3 regularScale;

    private void Awake()
    {
        regularScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = regularScale * .5f;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, regularScale, 8 * Time.unscaledDeltaTime);
    }
}
