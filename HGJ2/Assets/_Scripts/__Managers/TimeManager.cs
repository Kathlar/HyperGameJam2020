using UnityEngine;
using Sirenix.OdinInspector;

public class TimeManager : Singleton<TimeManager>
{
    public static float startFixedDeltaTime { get; private set; }

    [HideInPlayMode]
    public float minTimeScale = .02f, maxTimeScale = 1f;
    [HideInEditorMode][DisableInPlayMode]
    public float timeScale, fixedDeltaTime;

    private static float timeOfTimeJumpEnd;
    private static bool regularTimeOn;

    public bool realTime;
    private bool gamePaused;

    protected override void Awake()
    {
        base.Awake();
        startFixedDeltaTime = .5f / Screen.currentResolution.refreshRate;

        timeOfTimeJumpEnd = 0;
        regularTimeOn = false;
    }

    public static void SetTimeScale(float timeScale)
    {
        if(Instance)
        {
            if (Instance.gamePaused) return;
            if (Instance.realTime || regularTimeOn) timeScale = 1;
            if (timeOfTimeJumpEnd > Time.timeSinceLevelLoad || regularTimeOn) return;
        }

        Time.timeScale = Mathf.Clamp(timeScale, Instance.minTimeScale, Instance.maxTimeScale);
        Time.fixedDeltaTime = startFixedDeltaTime * Time.timeScale;

        if(Instance)
        {
            Instance.timeScale = Time.timeScale;
            Instance.fixedDeltaTime = Time.fixedDeltaTime;
        }
    }

    public static void TimeJump(float duration)
    {
        if (timeOfTimeJumpEnd < Time.timeSinceLevelLoad) timeOfTimeJumpEnd = Time.timeSinceLevelLoad;
        SetTimeScale(1);
        timeOfTimeJumpEnd = Mathf.Clamp(timeOfTimeJumpEnd + duration, timeOfTimeJumpEnd,
            timeOfTimeJumpEnd + 1);
    }

    public static void RegularTime(bool on)
    {
        if (on) SetTimeScale(1);
        regularTimeOn = on;
    }

    public static void PauseGame(bool on)
    {
        Instance.gamePaused = on;
        if (on) Time.timeScale = 0;
        else Time.timeScale = Instance.timeScale;
    }
}
