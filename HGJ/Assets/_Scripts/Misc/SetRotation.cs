using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRotation : MonoBehaviour
{
    public Vector3 targetRotation;

    private void Update()
    {
        transform.eulerAngles = targetRotation;
    }
}
