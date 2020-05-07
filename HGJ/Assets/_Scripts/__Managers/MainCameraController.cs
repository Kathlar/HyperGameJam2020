using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Defines behaviour of main camera in game.
/// </summary>
public class MainCameraController : MonoBehaviour
{
    public Camera mainCamera { get; private set; }
    public PostProcessVolume ppVolume { get; private set; }

    public Transform target { get; private set; }
    private Vector3 offset;

    static float barsStartValue = .1f;

    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
        ppVolume = GetComponentInChildren<PostProcessVolume>();
    }

    private void OnDestroy()
    {
        ppVolume.profile.TryGetSettings(out SCPE.BlackBars bars);
        bars.maxSize.value = barsStartValue;
    }

    private void LateUpdate()
    {
        if (!target)
        {
            if(GameManager.playerController)
                SetTarget(GameManager.playerController.transform);
        }
        else
            transform.position = target.position + offset;
    }

    private void SetTarget(Transform target)
    {
        this.target = target;
        offset = transform.position - target.position;
    }

    #region Post Processing

    private bool fadingOut = false;
    private bool fadingIn = false;

    public IEnumerator FadeFromBlackCoroutine()
    {
        if (!fadingOut)
        {
            fadingOut = true;
            SCPE.BlackBars bars;
            ppVolume.profile.TryGetSettings(out bars);
            TimeManager.TimeJump(1);
            if(barsStartValue == -1)
                barsStartValue = bars.maxSize.value;
            bars.maxSize.value = 1;
            if (bars)
            {
                do
                {
                    bars.maxSize.value -= Time.deltaTime;
                    yield return null;
                }
                while (bars.maxSize > barsStartValue);
            }
            fadingOut = false;
        }
    }

    public IEnumerator FadeToBlackCoroutine()
    {
        if (!fadingIn)
        {
            fadingIn = true;
            SCPE.BlackBars bars;
            ppVolume.profile.TryGetSettings(out bars);
            TimeManager.TimeJump(1);
            if (bars)
            {
                do
                {
                    bars.maxSize.value += Time.deltaTime * 2.5f;
                    yield return null;
                }
                while (bars.maxSize < 1);
            }
            fadingIn = false;
        }
    }
    #endregion
}
