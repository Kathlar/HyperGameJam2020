using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour, IDamageable
{
    public AudioSource destroySound;

    protected List<Rigidbody> children = new List<Rigidbody>();

    private void Awake()
    {
        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            if(rb.gameObject != gameObject)
                children.Add(rb);
        }
    }

    private void Start()
    {
        foreach(Rigidbody rb in children)
        {
            rb.gameObject.SetActive(false);
        }
    }

    public void GetDamage(int power)
    {
        GetDestroyed();
    }

    public virtual void GetDestroyed()
    {
        if (destroySound)
        {
            destroySound.Play();
            destroySound.transform.SetParent(GameManager.orphanParent);
            Destroy(destroySound.gameObject, destroySound.clip.length);
        }

        foreach(Rigidbody rb in children)
        {
            rb.gameObject.SetActive(true);
            rb.transform.SetParent(GameManager.orphanParent);
            rb.AddExplosionForce(200, transform.position, 1);
        }

        Destroy(gameObject);
    }
}
