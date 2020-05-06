using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTriggerCollector : TriggerCollector<ItemObject>
{
    protected override void OnTriggerEnterT(ItemObject t)
    {
        if (t.grabbed || t.thrown) return;
        base.OnTriggerEnterT(t);
    }
}
