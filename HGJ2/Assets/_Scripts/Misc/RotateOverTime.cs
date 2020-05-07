using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public Vector3 rotationVector;
    public bool localRotation;

    private void Update()
    {
        if (localRotation) transform.localEulerAngles += rotationVector * Time.deltaTime;
        else transform.eulerAngles += rotationVector * Time.deltaTime;
    }
}
