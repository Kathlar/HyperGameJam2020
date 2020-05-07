using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePause : MonoBehaviour
{
    public float stopMoment = .3f;
    private void Start()
    {
        var p = GetComponent<ParticleSystem>();
        p.Simulate(stopMoment);
        p.Pause();
    }
}
