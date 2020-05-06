using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourTriggeredChase : EnemyBehaviourTriggered
{
    public override void OnBehaviourUpdate()
    {
        base.OnBehaviourUpdate();
        if (!controller.target)
        {
            controller.SetBehaviour(EnemyBehaviourType.Regular);
            return;
        }

        float stopDistance = controller.inventory.grabbedItem ? 3f : 1;
        if (controller.DistanceToTarget() > stopDistance)
            controller.movement.Move(controller.target.transform.position - transform.position);
        else controller.movement.Idle();
        controller.movement.LookAt(controller.target.transform.position, true);
        if (Time.timeSinceLevelLoad > 1 && controller.AngleToTarget() < 5)
            controller.inventory.Use();
    }
}
