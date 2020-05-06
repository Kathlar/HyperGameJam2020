using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    public EnemyController controller { get; private set; }

    public enum EnemyBehaviourType { Regular, Triggered }
    public abstract EnemyBehaviourType behaviourType { get; }

    protected virtual void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    public abstract void OnBehaviourStart();
    public abstract void OnBehaviourUpdate();
    public abstract void OnBehaviourEnd();
}
