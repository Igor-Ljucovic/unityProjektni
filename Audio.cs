using System;
using UnityEngine;

[Serializable]
public class Audio  // MonoBehaviour - mora ovo da se disable-uje da bi moglo lepo da se iskoristi kao mini-objekat
{
    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector] public AudioSource audioSource;
}
