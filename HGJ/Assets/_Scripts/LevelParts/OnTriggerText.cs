using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OnTriggerText : MonoBehaviour
{
    public List<string> texts = new List<string>();
    public float duration = 1;
    public bool clearEarlier = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Player>()) return;

        if (clearEarlier) FlashTextManager.ClearTexts();
        foreach (string t in texts)
            FlashTextManager.FlashText(t, duration);

        Destroy(gameObject);
    }
}

#if UNITY_EDITOR
static class OnTriggerTextBuilder
{
    [MenuItem("GameObject/_Kathlar/TextTrigger", false, 2)]
    static void CreateTextTrigger()
    {
        GameObject textTriggerObject = new GameObject("TextTrigger");
        textTriggerObject.AddComponent<BoxCollider>().isTrigger = true;
        textTriggerObject.AddComponent<OnTriggerText>();
    }
}
#endif