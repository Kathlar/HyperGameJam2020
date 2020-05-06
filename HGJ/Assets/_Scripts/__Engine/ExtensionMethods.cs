using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contains all extension methods, which exclude classes built in Unity
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Returns int value of a LayerMask
    /// </summary>
    public static int LayerMaskToLayer(this LayerMask mask)
    {
        int layerNumber = 0;
        int layer = mask.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber;
    }

    /// <summary>
    /// Resets local values of position, rotation and scale of a transform
    /// </summary>
    public static void ResetLocal(this Transform transform, bool resetScale = true)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        if(resetScale)
            transform.localScale = Vector3.one;
    }

    public static float CombinedValue(this Color color)
    {
        return color.r + color.g + color.b + color.a;
    }

    public static int FindIndexNumber<T>(this List<T> list, T obj)
    {
        return list.FindIndex(a => a.Equals(obj));
    }

    public static Rigidbody Reset(this Rigidbody rigidbody)
    {
        rigidbody.velocity = Vector3.zero;
        return rigidbody;
    }

    public static Rigidbody TurnOff(this Rigidbody rigidbody)
    {
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        return rigidbody.Reset();
    }

    public static Rigidbody TurnOn(this Rigidbody rigidbody)
    {
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        return rigidbody;
    }

    public static Transform ClearParent(this Transform t)
    {
        t.SetParent(GameManager.orphanParent);
        return t;
    }
}