using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    private const float xOff = 4.35f, yOff = 5f;

    public Vector3 cubeCoordinates;
    protected Vector2 offsetCoordinates;
    protected Vector2 axialCoordinates;
    protected Vector2 doubledCoordinates;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        transform.position -= Vector3.up * 20;
    }

    private void Update()
    {
        transform.position = Vector3.Slerp(transform.position, startPosition, Time.deltaTime * 3);
    }

    public void SetUpCoordinates()
    {
        offsetCoordinates = new Vector2(transform.position.x / xOff, 
            Mathf.Floor(-transform.position.z / yOff));

        cubeCoordinates = new Vector3(offsetCoordinates.x, 0,
            offsetCoordinates.y - (offsetCoordinates.x - (offsetCoordinates.x % 2)) / 2);
        cubeCoordinates.y = -cubeCoordinates.x - cubeCoordinates.z;

        axialCoordinates = new Vector2(cubeCoordinates.x, cubeCoordinates.z);

        doubledCoordinates = new Vector2(cubeCoordinates.x,
            2 * cubeCoordinates.z + cubeCoordinates.x);
    }
}
