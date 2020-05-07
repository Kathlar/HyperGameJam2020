using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterMovement : MonoBehaviour
{
    public NavMeshAgent agent { get; private set; }

    public float moveSpeed = 5, rotationSpeed = 5, rotationTowardsTargetMultiplier = 4;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public abstract void Move(Vector3 direction, float moveSpeedPercent = 1);

    public void Idle()
    {
        if(agent.isActiveAndEnabled)
            agent.SetDestination(transform.position);
    }

    public void LookAt(Vector3 point, bool towardsTarget = false)
    {
        point.y = transform.position.y;
        Quaternion lookAtQuaternion = Quaternion.LookRotation(point - transform.position);
        float actualRotationSpeed = rotationSpeed * Time.deltaTime;
        if (towardsTarget) actualRotationSpeed *= rotationTowardsTargetMultiplier;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtQuaternion,
            actualRotationSpeed);
    }

    public abstract Vector3 DestinedPosition();
}
