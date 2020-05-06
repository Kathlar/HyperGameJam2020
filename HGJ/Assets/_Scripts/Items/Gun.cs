using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : UseableItemObject
{
    public GameObject projectilePrefab;

    public List<Transform> shootPoints = new List<Transform>();

    public int fullBulletCount = 3;
    protected int bulletCount;

    public ParticleSystem shootParticle;

    public int bulletsPerShoot = 1;

    private Coroutine lastUseCoroutine;

    protected override void Start()
    {
        base.Start();
        Refill();
    }

    protected override bool DoUse(Vector3 targetPoint)
    {
        if (bulletCount == 0) return false;
        lastUseCoroutine = StartCoroutine(DoUseCoroutine(targetPoint));
        return true;
    }

    private bool Shoot(Vector3 targetPoint)
    {
        if (bulletCount == 0) return false;
        bulletCount--;
        List<Projectile> projectiles = new List<Projectile>();
        foreach(Transform shootPoint in shootPoints)
        {
            Projectile projectile = Instantiate(projectilePrefab, shootPoint.position,
                shootPoint.rotation).GetComponent<Projectile>();
            projectile.transform.SetParent(GameManager.orphanParent);
            if (targetPoint != Vector3.zero)
            {
                Vector3 lookAtVec = transform.position + transform.forward;
                lookAtVec.y = targetPoint.y;
                projectile.transform.LookAt(lookAtVec);
            }
            projectile.SetUp(mainCollider);
            projectiles.Add(projectile);
        }
        useAudioSource.Play();
        refillAudioSource.Stop();

        for(int i = 0; i < projectiles.Count - 1; i++)
        {
            for(int j = i + 1; j < projectiles.Count; j++)
            {
                Physics.IgnoreCollision(projectiles[i].mainCollider,
                    projectiles[j].mainCollider);
            }
        }

        if (shootParticle) shootParticle.Play();

        Invoke("RefillSound", cooldown);
        return true;
    }

    private IEnumerator DoUseCoroutine(Vector3 targetPoint)
    {
        for (int i = 0; i < bulletsPerShoot; i++)
        {
            yield return new WaitForSeconds(.1f);
            if (!Shoot(Vector3.zero)) i = 5;
        }
    }

    public void RefillSound()
    {
        if(refillAudioSource && itemObject.grabbed && itemObject.lastOwner is Player)
            refillAudioSource.Play();
    }

    public override void Refill()
    {
        bulletCount = fullBulletCount;
    }

    public override void OnDrop()
    {
        if (lastUseCoroutine != null) StopCoroutine(lastUseCoroutine);
        base.OnDrop();
    }

    private void OnDrawGizmosSelected()
    {
        foreach(Transform p in shootPoints)
        {
            Gizmos.DrawLine(p.position, p.position + p.forward);
        }
    }
}
