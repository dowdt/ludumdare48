using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{
    AudioSource Audio;
    public static SoundManager instance;
    void Awake()
    {
        instance = this;
        Audio = GetComponent<AudioSource>();
    }
    public void PlayOneShot(AudioClip clip){
        Audio.PlayOneShot(clip);

    }
    

    
}
