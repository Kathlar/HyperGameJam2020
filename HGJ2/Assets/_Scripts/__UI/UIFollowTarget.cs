using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    public Transform target { get; protected set; }

    private void Start()
    {
        transform.SetParent(GameManager.MainCanvas.transform);
    }

    public void SetUp(Transform newTarget)
    {
        target = newTarget;
    }

    private void FixedUpdate()
    {
        transform.position = GameManager.mainCameraController.mainCamera.WorldToScreenPoint(
            target.transform.position + Vector3.up * 2);
    }
}
