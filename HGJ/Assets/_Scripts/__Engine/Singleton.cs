using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class of any manager class. It contains static instance object, that
/// can be referenced from any script. 
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    protected static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
}
