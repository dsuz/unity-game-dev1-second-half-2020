using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem[] m_particles = null;

    public void Play()
    {
        foreach(var p in m_particles)
        {
            p.Play();
        }
    }

    public void Stop()
    {
        foreach (var p in m_particles)
        {
            p.Stop();
        }
    }
}
