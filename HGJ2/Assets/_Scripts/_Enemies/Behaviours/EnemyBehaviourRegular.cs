using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourRegular : EnemyBehaviour
{
    public override EnemyBehaviourType behaviourType { get { return EnemyBehaviourType.Regular; } }

    public override void OnBehaviourStart()
    {

    }

    public override void OnBehaviourUpdate()
    {
        controller.LookForTarget();
    }

    public override void OnBehaviourEnd()
    {

    }
}
