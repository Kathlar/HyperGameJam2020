using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UseableItemObject : MonoBehaviour
{
    protected ItemObject itemObject;
    public Collider mainCollider { get; protected set; }
    public AudioSource useAudioSource, refillAudioSource;

    public float cooldown = .5f;
    public float useTimeJump = .15f;
    private float timeOfLastUseage;

    public bool useOnThrow;

    protected virtual void Awake()
    {
        mainCollider = GetComponent<Collider>();
        itemObject = GetComponent<ItemObject>();
    }

    protected virtual void Start()
    {
        timeOfLastUseage = -cooldown;
    }

    public virtual bool Use(Vector3 targetPoint)
    {
        if (Time.timeSinceLevelLoad > timeOfLastUseage + cooldown)
        {
            timeOfLastUseage = Time.timeSinceLevelLoad;
            return DoUse(targetPoint);
        }
        return true;
    }

    protected abstract bool DoUse(Vector3 targetPoint);

    public virtual void Refill()
    {

    }

    public virtual void OnDrop()
    {

    }
}
