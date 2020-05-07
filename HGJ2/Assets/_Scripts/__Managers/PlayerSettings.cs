using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "_Kathlar/_Databases/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    public float mouseSensitivity = 1;

    public void LoadSettings()
    {
        if(PlayerPrefs.GetInt("PrefsSet") == 0)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("PrefsSet", 1);
            SaveSettings();
        }
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
        PlayerPrefs.Save();
    }
}
