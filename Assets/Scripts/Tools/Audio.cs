using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewAudio",menuName = "AudioInfo")]
public class AudioInfo:ScriptableObject
{
    public AudioClip clip;
    public bool isLoop;
    public float volume = 1;
}
