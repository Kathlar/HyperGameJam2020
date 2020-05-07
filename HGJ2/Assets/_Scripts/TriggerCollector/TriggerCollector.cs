using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerCollector<T> : MonoBehaviour where T : MonoBehaviour
{
    protected SphereCollider mainCollider;

    public List<T> objectsInTrigger = new List<T>();

    protected ITriggerCollectorReaction<T> triggerReaction;

    public float radius { get { return mainCollider.radius; } }

    private void Awake()
    {
        mainCollider = GetComponent<SphereCollider>();
    }

    public void SetUp(ITriggerCollectorReaction<T> t)
    {
        triggerReaction = t;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<T>(out T t))
        {
            OnTriggerEnterT(t);
        }
    }

    protected virtual void OnTriggerEnterT(T t)
    {
        if (triggerReaction != null)
        {
            triggerReaction.OnTriggerEnterReaction(t);
            if (objectsInTrigger.Count > 0)
                triggerReaction.OnTriggerExitReaction(objectsInTrigger[objectsInTrigger.Count - 1]);
        }
        objectsInTrigger.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<T>(out T t) && objectsInTrigger.Contains(t))
        {
            OnTriggerExitT(t);
        }
    }

    protected virtual void OnTriggerExitT(T t)
    {
        objectsInTrigger.Remove(t);
        if (triggerReaction != null)
        {
            triggerReaction.OnTriggerExitReaction(t);
            if (objectsInTrigger.Count > 0)
                triggerReaction.OnTriggerEnterReaction(objectsInTrigger[objectsInTrigger.Count - 1]);
        }
    }

    public T LastObjectInTrigger()
    {
        if (objectsInTrigger.Count > 0) return objectsInTrigger[objectsInTrigger.Count - 1];
        return null;
    }

    public void RemoveFromTrigger(T t)
    {
        if (objectsInTrigger.Contains(t))
        {
            objectsInTrigger.Remove(t);
        }
    }

    public T NextObjectInTrigger(T t)
    {
        int numberOfT = objectsInTrigger.FindIndexNumber<T>(t) + 1;
        if (numberOfT >= objectsInTrigger.Count) numberOfT = 0;
        return objectsInTrigger[numberOfT];
    }

    public T PreviousObjectInTrigger(T t)
    {
        int numberOfT = objectsInTrigger.FindIndexNumber<T>(t) - 1;
        if (numberOfT < 0) numberOfT = objectsInTrigger.Count - 1;
        return objectsInTrigger[numberOfT];
    }
}

public interface ITriggerCollectorReaction<T> where T : MonoBehaviour
{
    void OnTriggerEnterReaction(T t);
    void OnTriggerExitReaction(T t);
}