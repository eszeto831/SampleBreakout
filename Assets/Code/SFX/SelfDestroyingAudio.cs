using UnityEngine;
using System.Collections;

public class SelfDestroyingAudio : MonoBehaviour
{
    public AudioSource Audio;
    
    public void Init(AudioClip clip)
    {
        Audio.clip = clip;
        Audio.Play();
    }
}