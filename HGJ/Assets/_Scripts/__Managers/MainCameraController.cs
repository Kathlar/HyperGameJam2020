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

    const float barsStartValue = .1f;

    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
        ppVolume = GetComponentInChildren<PostProcessVolume>();
        ppVolume.profile = Instantiate(ppVolume.profile);
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
            bars.size.value = 1;
            do
            {
                ppVolume.profile.TryGetSettings(out bars);
                bars.size.value = Mathf.Clamp(bars.size.value - Time.deltaTime * 2f, 
                    0, 1);
                yield return null;
            }
            while (bars.size.value > barsStartValue);
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
                    bars.size.value += Time.deltaTime * 2.5f;
                    yield return null;
                }
                while (bars.size.value < 1);
            }
            fadingIn = false;
        }
    }
    #endregion
}
