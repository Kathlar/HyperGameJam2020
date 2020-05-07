using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthPanel : MonoBehaviour
{
    public RectTransform rectTransform { get; protected set; }

    public GameObject healthIconPrefab;
    protected List<Transform> healthIcons = new List<Transform>();

    public Character target { get; protected set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetUp(Character newTarget)
    {
        target = newTarget;
        UpdatePanel();
    }

    public void UpdateSize()
    {
        if (target)
        {
            Vector2 rect = rectTransform.sizeDelta;
            rect.x = target.currentHealth * 20 + 10;
            rectTransform.sizeDelta = rect;
        }
    }

    public void UpdatePanel()
    {
        if(target)
        {
            while(healthIcons.Count < target.currentHealth && healthIcons.Count < 10)
            {
                Transform healthIcon = Instantiate(healthIconPrefab, transform).transform;
                healthIcons.Add(healthIcon);
            }
            while(healthIcons.Count > target.currentHealth)
            {
                Destroy(healthIcons[healthIcons.Count-1].gameObject);
                healthIcons.RemoveAt(healthIcons.Count - 1);
            }
        }
    }
}
