using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main game class, defining references to objects of player, camera, etc.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [Header("Databases")]
    [SerializeField]
    public GameManagerDatabasePrefab prefabs;
    public static GameManagerDatabasePrefab Prefabs { get { return Instance.prefabs; } }
    [SerializeField]
    public GameManagerDatabaseLayers layers;
    public static GameManagerDatabaseLayers Layers { get { return Instance.layers; } }
    [SerializeField]
    public GameManagerDatabaseColors colors;
    public static GameManagerDatabaseColors Colors { get { return Instance.colors; } }
    [SerializeField]
    public GameManagerDatabaseThemes themes;
    public static GameManagerDatabaseThemes Themes { get { return Instance.themes; } }
    [SerializeField]
    public GameManagerDatabaseLevels levels;
    public static GameManagerDatabaseLevels Levels { get { return Instance.levels; } }

    [SerializeField]
    public PlayerSettings settings;
    public static PlayerSettings Settings { get { return Instance.settings; } }

    public static MainCameraController mainCameraController { get; private set; }
    public static PlayerController playerController { get; private set; }

    [Header("Variables")]
    public Canvas mainCanvas;
    public Canvas playerCanvas;
    public static Canvas MainCanvas { get { return Instance.mainCanvas; } }
    public static Canvas PlayerCanvas { get { return Instance.playerCanvas; } }

    public UIHealthPanel playerHealthPanel;
    public static UIHealthPanel PlayerHealthPanel { get { return Instance.playerHealthPanel; } }

    public AudioSource musicAudioSource;
    public static AudioSource MusicAudioSource;

    public GameObject pauseMenuObject;

    public static Transform orphanParent { get; protected set; }

    public bool loadingNextNevel { get; protected set; }
    public static bool LoadingLevel { get { return Instance && Instance.loadingNextNevel; } }
    public GameStatus gameStatus = GameStatus.Gameplay;
    public static GameStatus GameStatus { get { return Instance.gameStatus; } }

    protected override void Awake()
    {
        base.Awake();

        mainCameraController = FindObjectOfType<MainCameraController>();
        playerController = FindObjectOfType<PlayerController>();

        orphanParent = new GameObject("OrphanParent").transform;

        settings.LoadSettings();
    }

    private void Start()
    {
        LevelTheme theme = LevelFlowControl.Instance ? LevelFlowControl.Instance.levelTheme :
            Themes.emptyTheme.theme;
        if(!MusicAudioSource)
        {
            Instance.musicAudioSource.clip = Themes.ThemeInfo(theme).musicClip;
            Instance.musicAudioSource.Play();
            MusicAudioSource = musicAudioSource;
            musicAudioSource.transform.SetParent(null);
            DontDestroyOnLoad(musicAudioSource.gameObject);
        }
        StartCoroutine(OnLevelStartCoroutine());
    }

    private void Update()
    {
        if (LoadingLevel) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerController.character.immortal = true;
            LoadLevel("MainMenu");
        }
    }

    private void OnDisable()
    {
        settings.SaveSettings();
    }

    public static IEnumerator OnLevelStartCoroutine()
    {
        Instance.loadingNextNevel = true;
        Instance.SetUI(false);
        playerController.enabled = false;

        yield return Instance.StartCoroutine(mainCameraController.FadeFromBlackCoroutine());
        Instance.loadingNextNevel = false;

        playerController.enabled = true;
        playerController.character.immortal = false;
        Instance.SetUI(true);
        TimeManager.RegularTime(false);
    }

    public static IEnumerator OnLevelPartFinnishCoroutine()
    {
        Instance.mainCanvas.gameObject.SetActive(false);
        Instance.playerCanvas.gameObject.SetActive(false);

        playerController.enabled = false;
        playerController.character.immortal = true;
        TimeManager.RegularTime(true);

        yield return Instance.StartCoroutine(mainCameraController.FadeToBlackCoroutine());
    }

    private void SetUI(bool on)
    {
        mainCanvas.gameObject.SetActive(on);
        playerCanvas.gameObject.SetActive(on);
    }

    public static void LoadRandomLevel()
    {
        string levelName = "";
        do
        {
            levelName = Levels.randomLevelNames[
            Random.Range(0, Levels.randomLevelNames.Count)];
        }
        while (levelName == SceneManager.GetActiveScene().name);
        LoadLevel(levelName);
    }

    /// <summary>
    /// Loads level by name
    /// </summary>
    public static void LoadLevel(string name)
    {
        if (Instance.loadingNextNevel) return;
        Instance.StartCoroutine(Instance.LoadLevelCoroutine(name));
    }

    public static void LoadLevel(int number)
    {
        if (Instance.loadingNextNevel) return;
        Instance.StartCoroutine(Instance.LoadLevelCoroutine(number));
    }

    /// <summary>
    /// Loads next level on build list
    /// </summary>
    public static void LoadNextLevel()
    {
        LoadLevel(Levels.tutorialLevelNames[FindObjectOfType<LevelFlowControl>().tutorialLvlNumber]);
    }

    public static void ReloadLevel()
    {
        Instance.StartCoroutine(Instance.LoadLevelCoroutine(
            SceneManager.GetActiveScene().name));
    }

    protected IEnumerator LoadLevelCoroutine(string name = "")
    {
        FlashTextManager.ClearTexts();
        Instance.loadingNextNevel = true;
        yield return StartCoroutine(OnLevelPartFinnishCoroutine());

        if (name != "")
            DoLoadLevel(name);
        else
        {
            int levelNumber = SceneManager.GetActiveScene().buildIndex + 1;
            if (levelNumber >= SceneManager.sceneCountInBuildSettings) levelNumber = 0;
            DoLoadLevel(levelNumber);
        }
    }

    protected IEnumerator LoadLevelCoroutine(int levelNumber)
    {
        Instance.loadingNextNevel = true;
        yield return StartCoroutine(OnLevelPartFinnishCoroutine());
        DoLoadLevel(levelNumber);
    }

    protected static void DoLoadLevel(int number)
    {
        SceneManager.LoadScene(number);
    }

    protected static void DoLoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void SetGameStatus(GameStatus status)
    {
        Instance.gameStatus = status;

        if(GameStatus == GameStatus.Menu)
        {
            Instance.pauseMenuObject.SetActive(true);
            TimeManager.PauseGame(true);
            PlayerInputManager.SetCursor(true);
            PlayerInputManager.ClearValues();
        }
        else
        {
            Instance.pauseMenuObject.SetActive(false);
            TimeManager.PauseGame(false);
            PlayerInputManager.SetCursor(false);
            PlayerInputManager.ClearValues();
        }
    }
}

public enum GameStatus { Gameplay, Menu }