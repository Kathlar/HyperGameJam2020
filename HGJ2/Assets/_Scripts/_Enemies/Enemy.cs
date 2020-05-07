using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    protected Knife.PostProcessing.OutlineRegister outlineClose;

    public GameObject isTargetIcon;

    protected override void Awake()
    {
        base.Awake();
        if(mainMesh)
        {
            outlineClose = mainMesh.gameObject.AddComponent<Knife.PostProcessing.OutlineRegister>();
            if (outlineClose)
                outlineClose.enabled = false;
        }
    }

    protected override void Start()
    {
        base.Start();
        if(outlineClose)
            outlineClose.OutlineTint = GameManager.Colors.enemyColorClose;

        isTargetIcon.SetActive(false);
    }

    //private void FixedUpdate()
    //{
    //    if (Time.timeSinceLevelLoad < timeOfStunEnd) controller.movement.Move(
    //         transform.position + hitDirection * Time.fixedDeltaTime, .1f);
    //}

    protected override void CreateHealthPanel()
    {
        GameObject healthPanelObject = Instantiate(GameManager.Prefabs.ui.healthPanel);

        healthPanelObject.GetComponent<UIFollowTarget>().SetUp(transform);
        healthPanel = healthPanelObject.GetComponent<UIHealthPanel>();
        base.CreateHealthPanel();
        healthPanel.UpdateSize();
    }

    public override void GetDamage(int power)
    {
        base.GetDamage(power);
        controller.SetTarget(GameManager.playerController.character);
        healthPanel.UpdateSize();
    }

    protected override void Die()
    {
        LevelFlowControl.NotifyOnChange<Enemy>(this);
        Destroy(healthPanel.gameObject);
        base.Die();
    }

    public void OnBecomePlayerTarget()
    {
        isTargetIcon.SetActive(true);
    }

    public void OnStopBeingPlayerTarget()
    {
        isTargetIcon.SetActive(false);
        outlineClose.enabled = false;
    }

    public void OnBecomeClose()
    {
        if(outlineClose)
            outlineClose.enabled = true;
    }

    public void OnBecomeDistant()
    {
        if (outlineClose)
            outlineClose.enabled = false;
    }

    public override void Stun(bool melee = false)
    {
        base.Stun();
        if (melee) inventory.Drop();
        else inventory.DropStrong();
        controller.movement.Idle();
        timeOfStunEnd = Mathf.Clamp(Time.timeSinceLevelLoad + 1f,
             Time.timeSinceLevelLoad, Time.timeSinceLevelLoad + 2);

        GameObject stunEffect = Instantiate(GameManager.Prefabs.particles.stunEffect, transform);
        stunEffect.transform.localPosition = Vector3.zero;
        stunEffect.transform.rotation = Quaternion.identity;
        Destroy(stunEffect, stunEffect.GetComponent<ParticleSystem>().main.duration);
    }
}
