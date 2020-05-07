using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelTheme { None, Western, Cyberpunk, Dream }

[CreateAssetMenu(menuName = "_Kathlar/_Databases/Themes")]
public class GameManagerDatabaseThemes : GameManagerDatabase
{
    [System.Serializable]
    public class GMThemeInfo
    {
        public LevelTheme theme;
        public UnityEngine.Rendering.PostProcessing.PostProcessProfile postProcessProfile;
        public AudioClip musicClip;
    }

    public GMThemeInfo emptyTheme;
    public List<GMThemeInfo> themes = new List<GMThemeInfo>();

    public GMThemeInfo ThemeInfo(LevelTheme theme)
    {
        foreach (GMThemeInfo info in themes)
            if (info.theme == theme) return info;
        return emptyTheme;
    }
}
