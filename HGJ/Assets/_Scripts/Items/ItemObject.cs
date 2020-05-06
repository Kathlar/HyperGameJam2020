using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemObject : MonoBehaviour
{
    public Rigidbody rb { get; private set; }
    public Collider mainCollider;
    public UseableItemObject useable { get; private set; }
    public Destroyable destroyable { get; private set; }
    public RotateOverTime rotateOverTime { get; private set; }

    public MeshRenderer mainMesh;
    protected Knife.PostProcessing.OutlineRegister outline;

    public bool grabbed { get; private set; }
    public Character lastOwner { get; private set; }
    public bool thrown { get; protected set; }
    private Vector3 throwPoint;
    public bool randomThrowRotation = true;
    [HideIf("randomThrowRotation")]
    public Vector3 throwRotation;

    [FoldoutGroup("Hit Character Interactions")][Tooltip("Stays as unusable in hit object's body")]
    public bool stayInHitObject;
    [FoldoutGroup("Hit Character Interactions")][ShowIf("stayInHitObject")]
    public bool unusableAfterStay;
    [FoldoutGroup("Hit Character Interactions")]
    public bool stunOnHit = true, damageOnHit;
    [FoldoutGroup("Hit Character Interactions")][ShowIf("damageOnHit")]
    public int hitDamage = 1;
    [FoldoutGroup("Hit Character Interactions")]
    public Collider triggerCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        useable = GetComponent<UseableItemObject>();
        destroyable = GetComponent<Destroyable>();
        rotateOverTime = GetComponent<RotateOverTime>();
        if(!mainCollider) mainCollider = GetComponent<Collider>();
        if (triggerCollider) triggerCollider.enabled = false;

        if (!mainMesh) mainMesh = GetComponentInChildren<MeshRenderer>();
        if (mainMesh)
        {
            outline = mainMesh.gameObject.
                AddComponent<Knife.PostProcessing.OutlineRegister>();
            outline.enabled = false;
        }
    }

    private void Start()
    {
        if(rotateOverTime)
            rotateOverTime.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || !thrown ||
            (lastOwner && lastOwner.transform == other.transform))
            return;

        SetThrown(false);
        if (other.TryGetComponent<Character>(out Character hitChar))
        {
            if (stunOnHit)
                hitChar.Stun();
            if (damageOnHit)
                hitChar.GetDamage(hitDamage);
        }
        else if (other.TryGetComponent<Destroyable>(out Destroyable hitDestroyable))
            hitDestroyable.GetDamage(1);

        if (stayInHitObject)
        {
            if (!hitChar)
                transform.SetParent(other.transform);
            else
                transform.SetParent(hitChar.mainMesh.transform);
            rb.TurnOff();

            mainCollider.isTrigger = true;
            if (unusableAfterStay)
                Destroy(this);
        }
        else if (destroyable) destroyable.GetDestroyed();
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = rb.velocity;
        velocity.y = thrown ? 0 : Mathf.Clamp(velocity.y, -.5f, .5f);
        rb.velocity = velocity;
        if (thrown && Vector3.Distance(transform.position, throwPoint) > 15)
            SetThrown(false);
    }

    public void OnPotentialGrabEnter()
    {
        if(outline) outline.enabled = true;
    }

    public void OnPotentialGrabExit()
    {
        if(outline) outline.enabled = false;
    }

    public void OnGrab(Character lastOwner)
    {
        this.lastOwner = lastOwner;
        grabbed = true;
        rb.TurnOff();
        mainCollider.enabled = false;
        outline.enabled = false;
    }

    public void OnDrop()
    {
        grabbed = false;
        transform.ClearParent();
        rb.TurnOn().Reset();
        mainCollider.enabled = true;
        mainCollider.isTrigger = false;
        if (useable) useable.OnDrop();
    }

    public void OnThrow()
    {
        if (rotateOverTime) rotateOverTime.enabled = true;
        throwPoint = transform.position;
        OnDrop();
        ThrowWithPower(1);
        Vector3 throwVector = randomThrowRotation ? new Vector3(Random.Range(-60, 60), Random.Range(-60, 60),
            Random.Range(-60, 60)) : throwRotation;
        rb.angularVelocity = throwVector;
        SetThrown(true);

        if (useable && useable.useOnThrow) useable.Use(Vector3.zero);
    }

    public void ThrowWithPower(float percent)
    {
        rb.Reset();
        rb.AddForce(transform.forward * 20 * percent * rb.mass / TimeManager.startFixedDeltaTime);
    }

    private void SetThrown(bool on)
    {
        thrown = on;
        if(thrown)
        {
            if(rotateOverTime) rotateOverTime.enabled = true;
            if (triggerCollider)
            {
                mainCollider.enabled = false;
                triggerCollider.enabled = true;
            }
        }
        else
        {
            if (rotateOverTime) rotateOverTime.enabled = true;
            if (triggerCollider) triggerCollider.enabled = false;
            mainCollider.enabled = true;
        }
    }

    public bool TryUse(Vector3 targetPoint)
    {
        if (useable)
        {
            bool canBeUsed = useable.Use(targetPoint);
            if (canBeUsed)
                return true;
            else
                OnThrow();
        }
        else
            OnThrow();
        return false;
    }
}
