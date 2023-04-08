using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveManager : MonoSingleton<SaveManager>
{
    [SerializeField] public int qualityIndex;
    [SerializeField] public float masterVolume;
    public void Initialize()
    {
        if(!PlayerPrefs.HasKey("QualityLevel") || !PlayerPrefs.HasKey("MasterVolume"))
            SaveSettings();

        LoadSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualityLevel", GameManager.Instance.GetQualityIndex());
        PlayerPrefs.SetFloat("MasterVolume", AudioManager.Instance.GetMasterVolume());
    }

    public void LoadSettings()
    {
        GameManager.Instance.SetQuality(qualityIndex = PlayerPrefs.GetInt("QualityLevel"));
        AudioManager.Instance.SetMasterVolume(masterVolume = PlayerPrefs.GetFloat("MasterVolume"));
    }

    public bool isSettingsSaved()
    {
        return PlayerPrefs.GetInt("QualityLevel") == qualityIndex && PlayerPrefs.GetFloat("MasterVolume") == masterVolume;
    }
}
