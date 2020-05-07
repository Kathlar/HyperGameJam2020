using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugManager : Singleton<DebugManager>
{
    public bool debugOn;
    private bool wasDebugOn;
    public static bool DebugOn { get; private set; }
    private static bool DebugOnSet;

    public static bool godMode;

    protected override void Awake()
    {
        base.Awake();
        if (DebugOnSet) debugOn = DebugOn;
    }

    private void Update()
    {
        DebugOnSet = true;
        if (debugOn != wasDebugOn) DebugOn = debugOn;
        wasDebugOn = debugOn;

        if (!DebugOn || GameManager.LoadingLevel) return;

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Input.GetKeyDown(KeyCode.N))
            GameManager.LoadNextLevel();
        if (Input.GetKeyDown(KeyCode.G))
        {
            godMode = !godMode;
            FlashTextManager.FlashText("GOD MODE " + (godMode ? "ON" : "OFF"), .5f);
        }
        if (Input.GetKeyDown(KeyCode.K))
            EnemyController.KillAll();
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
            Debug.Break();
#endif

        if (Input.GetKeyDown(KeyCode.Comma)) GameManager.LoadNextLevel();
    }
}
