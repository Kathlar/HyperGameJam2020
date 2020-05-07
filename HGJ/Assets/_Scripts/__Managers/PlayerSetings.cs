using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetings : Singleton<PlayerSetings>
{
    //SOUND
    public static bool soundOn { get; private set; } = true;
    public static float soundVolume { get; private set; } = 1;

    //QUALITY
    public static int qualitySetting { get; private set; } = 2;

    public bool clearPlayerPrefs;

    public static float mouseSensitivity = 1f;
    public static float roundedMouseSensitivity
    {
         get
        {
            return Mathf.Round(mouseSensitivity * mouseSensitivity * 10f) / 10;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        if (clearPlayerPrefs)
        {
            ClearPrefs();
        }
        if (PlayerPrefs.GetInt("SettingsSet") == 0) SaveSettings();
        LoadSettings();
    }

    [Sirenix.OdinInspector.Button("Clear")]
    void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private static void LoadSettings()
    {
        soundVolume = PlayerPrefs.GetFloat("SoundVolume");
        SetSound(soundVolume);
        soundOn = PlayerPrefs.GetInt("SoundOn") == 1;
        SetSound(soundOn);

        qualitySetting = PlayerPrefs.GetInt("Quality");

        mouseSensitivity = PlayerPrefs.GetFloat("Mouse_Sensitivity");
        SetMouseSensitivity(mouseSensitivity);
    }

    private static void SaveSettings()
    {
        PlayerPrefs.SetInt("SoundOn", soundOn ? 1 : 0);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);

        PlayerPrefs.SetInt("Quality", qualitySetting);

        PlayerPrefs.SetInt("SettingsSet", 1);

        PlayerPrefs.SetFloat("Mouse_Sensitivity", mouseSensitivity);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        SaveSettings();
    }

    public static void SetSound()
    {
        SetSound(!soundOn);
    }

    public static void SetSound(bool on)
    {
        soundOn = on;
        AudioListener.volume = soundOn ? soundVolume : 0;
    }

    public static void SetSound(float volume)
    {
        soundVolume = volume;
        AudioListener.volume = soundVolume;
        SetSound(volume > 0);
    }

    public static void SetQualitySetting(int value)
    {
        qualitySetting = value;
    }

    public static void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }
}
