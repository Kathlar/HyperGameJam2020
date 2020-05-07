using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePause : MonoBehaviour
{
    private void Start()
    {
        var p = GetComponent<ParticleSystem>();
        p.Simulate(.3f);
        p.Pause();
    }
}
