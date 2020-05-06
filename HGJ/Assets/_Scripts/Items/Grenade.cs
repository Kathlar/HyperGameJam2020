using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : UseableItemObject
{
    public ParticleSystem thrownParticle;
    [BoxGroup("Explosion")]
    public ParticleSystem explosionParticle;

    [BoxGroup("Explosion")]
    public float timeToExplode = 1.5f, explosionPower = 1, explosionDistance = 3;
    [BoxGroup("Explosion")]
    public int explosionDamage = 1;

    protected override void Start()
    {
        base.Start();
        if (thrownParticle) thrownParticle.gameObject.SetActive(false);
        if (explosionParticle) explosionParticle.gameObject.SetActive(false);
    }

    protected override bool DoUse(Vector3 targetPoint)
    {
        useAudioSource.Play();
        StartCoroutine(UseCoroutine());
        return true;
    }

    private IEnumerator UseCoroutine()
    {
        itemObject.lastOwner.inventory.Throw();
        Destroy(itemObject);

        if (thrownParticle) thrownParticle.gameObject.SetActive(true);

        yield return new WaitForSeconds(timeToExplode);

        if (explosionParticle)
        {
            explosionParticle.gameObject.SetActive(true);
            explosionParticle.transform.SetParent(GameManager.orphanParent);
        }

        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosionDistance);
        foreach(Collider col in hitObjects)
        {
            if (col.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.GetDamage(explosionDamage);
                if (col.TryGetComponent<Character>(out Character hitChar))
                    hitChar.Stun();
            }
        }
        yield return null; 

        hitObjects = Physics.OverlapSphere(transform.position, explosionDistance);
        foreach (Collider col in hitObjects)
        {
            if (col.TryGetComponent<Rigidbody>(out Rigidbody hitRB))
                hitRB.AddExplosionForce(explosionPower, transform.position, explosionDistance);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionDistance);
    }
}
