using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sounds", menuName = "ScriptableObjects/Sounds", order = 0)]
public class Sounds : ScriptableObject
{
    [SerializeField] private AudioClip music;
    public AudioClip SoundClip => music;
}
