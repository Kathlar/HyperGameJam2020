using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public UnityEngine.UI.Slider sensitivitySlider;
    public Text sensitivityValue;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        sensitivitySlider.value = GameManager.Settings.mouseSensitivity;
    }

    private void Update()
    {
        sensitivityValue.text = sensitivitySlider.value.ToString();
    }

    public void ResumeGame()
    {
        GameManager.SetGameStatus(GameStatus.Gameplay);
    }

    public void SavePlayerPrefs()
    {
        GameManager.Settings.mouseSensitivity = sensitivitySlider.value;
    }
}
