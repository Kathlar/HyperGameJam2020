using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class QualityManager : Singleton<QualityManager>
{
    protected void Start()
    {
        GameManager.mainCameraController.ppVolume.enabled = true;
    }

    public static void UpdateQualitySettings()
    {
        GameManager.mainCameraController.ppVolume.profile.TryGetSettings<SCPE.Fog>(out SCPE.Fog fog);
        GameManager.mainCameraController.mainCamera.backgroundColor = fog.fogColor;
        if (QualitySettings.GetQualityLevel() == 0)
        {
            GameManager.mainCameraController.ppVolume.profile.TryGetSettings<AmbientOcclusion>(out AmbientOcclusion ao);
            ao.enabled.value = false;
            fog.enabled.value = false;
        }
    }

    public static void SetProfile(LevelTheme theme)
    {
        SetProfile(GameManager.Themes.ThemeInfo(theme).postProcessProfile);
    }

    public static void SetProfile(PostProcessProfile profile)
    {
        GameManager.mainCameraController.ppVolume.profile = Instantiate(profile);
        UpdateQualitySettings();
    }
}
