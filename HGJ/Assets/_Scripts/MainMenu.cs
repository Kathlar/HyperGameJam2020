using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public enum MenuPart { Main, Options }
    protected MenuPart currentMenuPart = MenuPart.Main;
    private bool changingMenuPart;

    public GameObject mainPanel, optionsPanel;

    private void Start()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
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
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
}
