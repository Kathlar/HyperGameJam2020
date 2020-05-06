using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashTextManager : Singleton<FlashTextManager>
{
    public Text flashText { get; protected set; }
    public AudioSource audioSource { get; protected set; }

    private List<string> textToShow = new List<string>();

    private Coroutine currentCoroutine;

    protected override void Awake()
    {
        base.Awake();
        flashText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    public static void FlashText(string text, float duration = 1)
    {
        if (Instance.currentCoroutine != null)
        {
            Instance.textToShow.Add(text);
            return;
        }
        Instance.currentCoroutine = 
            Instance.StartCoroutine(Instance.FlashTextCoroutine(text, duration));
    }

    private IEnumerator FlashTextCoroutine(string text, float duration = 1)
    {
        flashText.text = text;
        audioSource.Play();

        yield return new WaitForSecondsRealtime(duration);

        if(Instance.textToShow.Count > 0)
        {
            string nextText = Instance.textToShow[0];
            Instance.textToShow.RemoveAt(0);
            currentCoroutine = StartCoroutine(FlashTextCoroutine(nextText, duration));
        }
        else
        {
            flashText.text = "";
            currentCoroutine = null;
        }
    }

    public static void ClearTexts()
    {
        if (Instance.currentCoroutine != null) Instance.StopCoroutine(Instance.currentCoroutine);
        Instance.currentCoroutine = null;
        Instance.textToShow.Clear();
        Instance.flashText.text = "";
    }
}
