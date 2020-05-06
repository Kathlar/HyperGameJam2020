using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : CharacterInventory
{
    protected override void Awake()
    {
        base.Awake();
        itemTriggerCollector.SetUp(this);
    }

    public override void Use()
    {
        if (grabbedItem)
            TimeManager.TimeJump(grabbedItem.useable ? grabbedItem.useable.useTimeJump : .1f);
        base.Use();
    }

    public override void Throw()
    {
        base.Throw();
        TimeManager.TimeJump(.15f);
    }

    public override void OnTriggerEnterReaction(ItemObject t)
    {
        base.OnTriggerEnterReaction(t);
        if(!grabbedItem)
            t.OnPotentialGrabEnter();
    }

    public override void OnTriggerExitReaction(ItemObject t)
    {
        base.OnTriggerExitReaction(t);
        t.OnPotentialGrabExit();
    }

    public override bool MeleeAttack()
    {
        if (!base.MeleeAttack())
            return false;

        TimeManager.TimeJump(.15f);
        return true;
    }
}
