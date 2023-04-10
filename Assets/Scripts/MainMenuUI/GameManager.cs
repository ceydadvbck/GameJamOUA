using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    public bool useAlternativeNames = true;
    public string[] QualitySettingsNamesAlternative;
    [NonSerialized] public string[] QualitySettingsNames;
    [NonSerialized] public int CurrentQualityLevel;
    void Awake()
    {
        QualitySettingsNames = QualitySettings.names;
        CurrentQualityLevel = QualitySettings.GetQualityLevel();
        SaveManager.Instance.Initialize();
    }

    void Start()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = 0;
            StartCoroutine(AudioFadeIn(audioSource, 1));
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(CurrentQualityLevel = qualityIndex);
    }

    /*public string[] GetQualityNames()
    {
        return useAlternativeNames ? QualitySettingsNamesAlternative : QualitySettingsNames;
    }*/
    public int GetQualityIndex()
    {
        return CurrentQualityLevel;
    }

    public string GetQualityName()
    {
        return useAlternativeNames ? QualitySettingsNamesAlternative[CurrentQualityLevel] : QualitySettingsNames[CurrentQualityLevel];
    }

    public void LoadScene(int index)
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = 0;
            StartCoroutine(AudioFadeOut(audioSource, 1, index));
        }
    }

    public int GetScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }

    IEnumerator AudioFadeIn(AudioSource audioSource, float FadeTime)
    {
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }

    IEnumerator AudioFadeOut(AudioSource audioSource, float FadeTime, int index = 0)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }
}
