using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : Singleton<DebugManager>
{
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P)) Debug.Break();
#endif

        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
