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
}
