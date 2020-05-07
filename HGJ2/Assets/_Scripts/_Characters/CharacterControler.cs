using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    public Character character { get; private set; }
    public CharacterMovement movement { get; private set; }
    public CharacterInventory inventory { get; private set; }

    [HideInInspector]
    public Character target;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
        movement = GetComponent<CharacterMovement>();
        inventory = GetComponent<CharacterInventory>();
    }

    protected virtual void FixedUpdate()
    {
        if (Time.timeSinceLevelLoad > character.timeOfStunEnd) character.gotHit = false;
        if (character.gotHit)
            movement.Move(character.hitDirection * Time.fixedDeltaTime);
    }

    public virtual void SetTarget(Character newTarget)
    {
        target = newTarget;
    }

    public float DistanceToTarget()
    {
        if (target) return Vector3.Distance(transform.position, target.transform.position);
        return -1;
    }

    public bool TargetInMeleeDistance()
    {
        float distance = DistanceToTarget();
        if (distance > 0 && distance < 1.8f) return true;
        return false;
    }

    public float AngleToTarget()
    {
        return Vector3.Angle(transform.forward, target.transform.position - transform.position);
    }
}
