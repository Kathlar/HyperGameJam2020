using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class KathlarMenu : EditorWindow
{
    [MenuItem("Window/Kathlar/Kathlar Menu")]
    public static void ShowWindow()
    {
        GetWindow<KathlarMenu>("KathlarMenu");
    }

    private void OnGUI()
    {
        GUILayout.Label("KATHLAR MENU", EditorStyles.boldLabel);

        if(GUILayout.Button("Create Scene Structure"))
        {
            CreateLevelStructure();
        }
    }

    [MenuItem("GameObject/_Kathlar/Level Structure", false, 2)]
    static void CreateLevelStructure()
    {
        GameObject levelObject = new GameObject("Level");
        levelObject.AddComponent<LevelFlowControl>();

        GameObject enviroObject = new GameObject("Enviro");
        enviroObject.transform.SetParent(levelObject.transform);
        GameObject textTriggersObject = new GameObject("TextTriggers");
        textTriggersObject.transform.SetParent(levelObject.transform);
        GameObject dynamicObject = new GameObject("Dynamic");
        dynamicObject.transform.SetParent(levelObject.transform);

        GameObject itemObject = new GameObject("Items");
        GameObject enemiesObject = new GameObject("Enemies");
        itemObject.transform.SetParent(dynamicObject.transform);
        enemiesObject.transform.SetParent(dynamicObject.transform);
    }
}
#endif