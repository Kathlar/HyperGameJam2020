using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public Camera mainCamera { get; protected set; }

    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }
}
