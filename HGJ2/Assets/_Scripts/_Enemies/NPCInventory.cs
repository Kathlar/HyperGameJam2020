using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventory : CharacterInventory
{
    public override void Use()
    {
        if (Time.timeSinceLevelLoad < timeOfLastItemGrab + .5f) return;
        base.Use();
        if (grabbedItem && grabbedItem.useable) grabbedItem.useable.Refill();
    }
}
