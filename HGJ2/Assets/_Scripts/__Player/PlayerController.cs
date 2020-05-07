using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterControler
{
    protected EnemyTriggerCollector enemyTrigger;

    public float viewDistance = 10;

    private bool targeting;
    private float timeOfLastShootPress;
    private float timeOfLastTargetChange = -1;

    protected override void Awake()
    {
        base.Awake();

        enemyTrigger = GetComponentInChildren<EnemyTriggerCollector>();
    }

    protected void Update()
    {
        if (GameManager.GameStatus != GameStatus.Gameplay) return;

        Vector3 moveVector = new Vector3(PlayerInputManager.Values.mouseX.value, 0, 
            PlayerInputManager.Values.mouseY.value);
        Vector3 keyboardMoveVector = new Vector3(PlayerInputManager.Values.horizontal.value, 0,
            PlayerInputManager.Values.vertical.value);
        if (keyboardMoveVector.magnitude > 0) keyboardMoveVector.Normalize();
        moveVector += keyboardMoveVector * .5f;
        if (moveVector.magnitude > 1) moveVector.Normalize();

        if(moveVector.magnitude > 0 && !targeting)
            movement.Move(moveVector);

        moveVector *= PlayerSetings.mouseSensitivity;

        TimeManager.SetTimeScale(moveVector.magnitude);

        moveVector *= PlayerSetings.mouseSensitivity * 5;

        if (PlayerInputManager.Values.leftMouse.wasPressed)
        {
            timeOfLastShootPress = Time.realtimeSinceStartup;
        }
        else if (PlayerInputManager.Values.leftMouse.wasReleased)
        {
            TimeManager.TimeJump(.1f);
            inventory.Use();
        }
        else if (PlayerInputManager.Values.leftMouse.isPressed)
        {
            if (Time.realtimeSinceStartup > timeOfLastShootPress + .15f) targeting = true;
        }
        else targeting = false;
        if (PlayerInputManager.Values.rightMouse.wasPressed) inventory.Throw();

        CheckTarget();

        if(!targeting && target)
        {
            movement.LookAt(target.transform.position, true);
        }
        else if (moveVector.magnitude > 0)
            movement.LookAt(transform.position + moveVector);

        if (target)
        {
            if (TargetInMeleeDistance())
                (target as Enemy).OnBecomeClose();
            else
                (target as Enemy).OnBecomeDistant();

            if(Time.realtimeSinceStartup > timeOfLastTargetChange + .6f)
            {
                float scrollWheel = PlayerInputManager.Values.mouseScrollWheel.value;
                if(scrollWheel != 0)
                {
                    timeOfLastTargetChange = Time.realtimeSinceStartup;
                    if (scrollWheel > 0)
                        SetTarget(enemyTrigger.NextObjectInTrigger(target as Enemy));
                    else if (scrollWheel < 0)
                        SetTarget(enemyTrigger.PreviousObjectInTrigger(target as Enemy));
                }
            }
        }
    }

    void CheckTarget()
    {
        if (target)
        {
            if (!enemyTrigger.objectsInTrigger.Contains(target as Enemy) ||
                !ObjectInDirectRay(target.transform))
                SetTarget(null);
        }
        else
        {
            if (enemyTrigger.objectsInTrigger.Count == 0) return;

            int i = 0;
            do
            {
                Enemy potentialTarget = enemyTrigger.objectsInTrigger[i];
                if(potentialTarget)
                {
                    if(ObjectInDirectRay(potentialTarget.transform))
                        SetTarget(potentialTarget);
                }
                else
                    enemyTrigger.objectsInTrigger.RemoveAt(i);

                i++;
            }
            while (i < enemyTrigger.objectsInTrigger.Count && !target);
        }
    }

    private bool ObjectInDirectRay(Transform t)
    {
        if(t && GameManager.Layers)
        {
            Ray ray = new Ray(transform.position, t.transform.position - transform.position);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, enemyTrigger.radius,
                GameManager.Layers.TargetLayer) ||
                hit.transform != t)
                return false;
        }

        return true;
    }

    public override void SetTarget(Character newTarget)
    {
        if (target && target != newTarget)
            (target as Enemy).OnStopBeingPlayerTarget();

        base.SetTarget(newTarget);

        if (target) (target as Enemy).OnBecomePlayerTarget();
    }
}
