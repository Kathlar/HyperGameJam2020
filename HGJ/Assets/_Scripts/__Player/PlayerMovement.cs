using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    private Vector3 targetPosition;

    public override void Move(Vector3 direction, float moveSpeedPercent = 1)
    {
        targetPosition = transform.position + direction;
        agent.Move(direction * Time.deltaTime * moveSpeed * moveSpeedPercent);
    }

    public override Vector3 DestinedPosition()
    {
        return targetPosition;
    }
}
