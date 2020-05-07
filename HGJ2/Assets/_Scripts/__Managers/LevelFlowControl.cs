using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LevelFlowControl : Singleton<LevelFlowControl>
{
    public int tutorialLvlNumber = -1;

#if UNITY_EDITOR && ODIN_INSPECTOR
    private void UpdateThemeOnCurrentScene()
    {
        FindObjectOfType<UnityEngine.Rendering.PostProcessing.PostProcessVolume>().profile =
            FindObjectOfType<GameManager>().themes.ThemeInfo(levelTheme).postProcessProfile;
    }
    [Sirenix.OdinInspector.OnValueChanged("UpdateThemeOnCurrentScene")]
#endif
    public LevelTheme levelTheme;
    public LevelFinnish.LevelFinnishCondition finnishCondition;

    private void Start()
    {
        QualityManager.SetProfile(levelTheme);
    }

    public static void NotifyOnChange<T>(T t)
    {
        if (!Instance) return;
        if (t.GetType() == typeof(Enemy) && 
            Instance.finnishCondition == LevelFinnish.LevelFinnishCondition.KillAll)
        {
            if (EnemyController.Enemies.Count <= 1)
                NextLevel();
        }
    }

    public static void NextLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", Instance.tutorialLvlNumber);
        PlayerPrefs.Save();
        if (Instance.tutorialLvlNumber > -1 && Instance.tutorialLvlNumber < 4)
            GameManager.LoadNextLevel();
        else
            GameManager.LoadRandomLevel();
    }
}
