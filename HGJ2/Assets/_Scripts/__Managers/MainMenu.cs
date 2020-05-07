using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu>
{
    public enum MenuPart { Main, Options }
    protected MenuPart currentMenuPart = MenuPart.Main;
    private bool changingMenuPart;

    [SerializeField]
    public GameManagerDatabaseLevels levels;

    public Transform mainCamera;
    public float rotationSpeed = 10;

    public RectTransform mainMenu;
    [Header("Menu Panels")]
    public GameObject mainPanel;
    public GameObject optionsPanel;

    [Header("Menu Items")]
    public GameObject soundCrossedOutIcon;
    public Michsky.UI.ModernUIPack.CustomDropdown qualityDropdown;
    public Michsky.UI.ModernUIPack.SliderManager volumeSlider, sensitivitySlider;
    public Text sensitivityText;

    private void Start()
    {
        if (GameManager.MusicAudioSource) Destroy(GameManager.MusicAudioSource.gameObject);

        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);

        soundCrossedOutIcon.SetActive(!PlayerSetings.soundOn);

        SetQuality(PlayerSetings.qualitySetting);
        qualityDropdown.selectedItemIndex = PlayerSetings.qualitySetting;
        volumeSlider.GetComponent<Slider>().value = PlayerSetings.soundVolume;
        sensitivitySlider.GetComponent<Slider>().value = PlayerSetings.mouseSensitivity;
        sensitivityText.text = PlayerSetings.roundedMouseSensitivity.ToString();

        PlayerInputManager.SetCursor(true);
    }

    private void Update()
    {
        mainCamera.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * rotationSpeed);
    }

    public void SetMenuPart(MenuPart newPart)
    {
        if (newPart == currentMenuPart) return;
        if (changingMenuPart) return;

        StartCoroutine(SetMenuPartCoroutine(newPart));
    }

    private IEnumerator SetMenuPartCoroutine(MenuPart newPart)
    {
        changingMenuPart = true;
        currentMenuPart = newPart;

        yield return new WaitForSeconds(.2f);

        if (newPart == MenuPart.Main)
        {
            mainPanel.SetActive(true);
            optionsPanel.SetActive(false);
        }
        else if (newPart == MenuPart.Options)
        {
            mainPanel.SetActive(false);
            optionsPanel.SetActive(true);
        }

        changingMenuPart = false;
    }

    public void PlayGameButton()
    {
        int levelFinnishedNumber = PlayerPrefs.GetInt("CurrentLevel");
        if (levelFinnishedNumber == 0) levelFinnishedNumber = 1;
        if (levelFinnishedNumber < 4 && levelFinnishedNumber > -1)
            SceneManager.LoadScene(levels.tutorialLevelNames[levelFinnishedNumber - 1]);
        else
            SceneManager.LoadScene(levels.randomLevelNames[
                UnityEngine.Random.Range(0, levels.randomLevelNames.Count)]);
            
    }

    public void OptionsButton()
    {
        SetMenuPart(MenuPart.Options);
    }

    public void GoBackToMainMenuButton()
    {
        SetMenuPart(MenuPart.Main);
    }

    public void SetQuality(int value)
    {
        switch(value)
        {
            case 0:
                QualitySettings.SetQualityLevel(0);
                break;
            case 1:
                QualitySettings.SetQualityLevel(1);
                break;
            case 2:
                QualitySettings.SetQualityLevel(2);
                break;
        }
        PlayerSetings.SetQualitySetting(value);
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    public void SetSound()
    {
        PlayerSetings.SetSound();
        soundCrossedOutIcon.SetActive(!PlayerSetings.soundOn);
    }

    public void SetSoundVolume()
    {
        PlayerSetings.SetSound(volumeSlider.GetComponent<Slider>().value);
        soundCrossedOutIcon.SetActive(!PlayerSetings.soundOn);
    }

    public void SetMouseSensitivity()
    {
        PlayerSetings.SetMouseSensitivity(sensitivitySlider.GetComponent<Slider>().value);
        sensitivityText.text = PlayerSetings.roundedMouseSensitivity.ToString();
    }
}
