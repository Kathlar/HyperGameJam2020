using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    protected override void CreateHealthPanel()
    {
        healthPanel = GameManager.PlayerHealthPanel;
        base.CreateHealthPanel();
    }

    public override void GetDamage(int power)
    {
        if (DebugManager.DebugOn && DebugManager.godMode) return;
        base.GetDamage(power);
    }

    protected override void Die()
    {
        GameManager.ReloadLevel();
    }
}
