using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    //public AudioSource musicSource;
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public float GetMasterVolume()
    {
        return AudioListener.volume;
    }

    /* public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    } */
}
