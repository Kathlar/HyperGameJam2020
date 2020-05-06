using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : CharacterMovement
{
    protected float startSpeed;

    private void Start()
    {
        startSpeed = moveSpeed;
        agent.speed = moveSpeed;
    }

    public override void Move(Vector3 direction, float moveSpeedPercent = 1)
    {
        agent.speed = startSpeed * moveSpeedPercent;
        agent.SetDestination(transform.position + direction);
    }

    public override Vector3 DestinedPosition()
    {
        return transform.position;
    }
}
