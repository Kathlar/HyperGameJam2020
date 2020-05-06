using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float timeOfLastMousePress;
    private bool moved = false;
    private Vector3 totalMoveVector;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            timeOfLastMousePress = Time.timeSinceLevelLoad;
            moved = false;
            totalMoveVector = Vector3.zero;
        }

        if(Input.GetMouseButton(0))
        {
            Vector3 moveVector = new Vector3(
                -Input.GetAxis("Mouse X"), 0, -Input.GetAxis("Mouse Y"));
            GameManager.mainCamera.transform.position += moveVector;

            totalMoveVector += moveVector;
            if (totalMoveVector.magnitude > 1) moved = true;
        }

        if(Input.GetMouseButtonUp(0) && Time.timeSinceLevelLoad < timeOfLastMousePress + .2f && 
            !moved)
        {
            Debug.Log("PRESSED");
        }
    }
}
