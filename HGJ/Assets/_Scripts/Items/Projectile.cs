using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    protected static List<Projectile> projectiles;
    public Collider mainCollider { get; private set; }
    private Rigidbody rb;

    private TrailRenderer trail;

    public GameObject hitEffectPrefab;
    public Transform hitEffectPoint;

    public MovementType movementType;
    public float flightSpeed = 10;
    public int damagePower = 1;

    private void Awake()
    {
        mainCollider = GetComponent<Collider>();
        if (movementType == MovementType.Rigidbody) rb = GetComponent<Rigidbody>();

        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        if (projectiles == null) projectiles = new List<Projectile>();
        projectiles.Add(this);
    }

    private void OnDisable()
    {
        projectiles.Remove(this);
    }

    public void SetUp(Collider ignoreCollider)
    {
        Physics.IgnoreCollision(mainCollider, ignoreCollider);

        if (movementType == MovementType.Rigidbody) 
            rb.AddForce(transform.forward * flightSpeed / TimeManager.startFixedDeltaTime);
    }

    private void FixedUpdate()
    {
        if (movementType == MovementType.Transform)
            transform.position += transform.forward * flightSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger && !other.TryGetComponent<Projectile>(out Projectile p)) return;
        if (other.TryGetComponent<ItemObject>(out ItemObject item) && !item.thrown) return;
        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.GetDamage(damagePower);
        }

        OnHitEffect();
    }

    void OnHitEffect()
    {
        if (trail)
        {
            trail.transform.SetParent(GameManager.orphanParent);
            Destroy(trail.gameObject, trail.time);
        }

        ParticleSystem hitEffect = Instantiate(hitEffectPrefab, hitEffectPoint.position,
            transform.rotation).GetComponent<ParticleSystem>();
        hitEffect.transform.SetParent(GameManager.orphanParent);
        Destroy(hitEffect.gameObject, hitEffect.main.duration);

        Destroy(gameObject);
    }

    public static void DestroyAllProjectiles()
    {
        if (projectiles == null) projectiles = new List<Projectile>();
        if (projectiles.Count == 0) return;
        for(int i = projectiles.Count -1; i >= 0; i--)
        {
            projectiles[i].OnHitEffect();
        }
    }
}
