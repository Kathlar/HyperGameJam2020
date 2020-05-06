using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static MainCameraController mainCamera { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        mainCamera = FindObjectOfType<MainCameraController>();
    }
}
