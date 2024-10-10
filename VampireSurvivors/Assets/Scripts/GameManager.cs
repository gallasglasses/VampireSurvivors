using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Transform playerTransform;
    public Player player;

    [SerializeField] private UIManager uiManager;
    private SaveManager saveManager;
    private PlayerStats currentStats;

    public delegate void OnMatchStateChangedEvent(EMatchState matchState);
    public event OnMatchStateChangedEvent OnMatchStateChanged;

    private EMatchState matchState;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
            return;
        SetMatchState(EMatchState.InProgress);
        uiManager.OnUIStateChanged += SetUIState;
    }

    private void Start()
    {
        SetMatchState(EMatchState.InProgress);
        uiManager.OnUIStateChanged += SetUIState;

        saveManager = FindObjectOfType<SaveManager>();
        currentStats = saveManager.LoadStats();
    }

    private void UpdateStats(PlayerStats playerStats)
    {
        if (playerStats.maxLevel > currentStats.maxLevel)
        {
            currentStats.maxLevel = playerStats.maxLevel;
        }

        if (playerStats.maxEnemiesKilled > currentStats.maxEnemiesKilled)
        {
            currentStats.maxEnemiesKilled = playerStats.maxEnemiesKilled;
        }

        saveManager.SaveStats(currentStats);
    }

    public void Pause()
    {
        if (matchState != EMatchState.Pause)
        {
            SetMatchState(EMatchState.Pause);
            Debug.Log($"EMatchState {EMatchState.Pause}");
        }
    }

    public void GameOver()
    {
        if (matchState != EMatchState.GameOver)
        {
            SetMatchState(EMatchState.GameOver);
            UpdateStats(Player.Instance.PlayerStats);
            Debug.Log($"EMatchState {EMatchState.GameOver}");
        }
    }

    public void UnPause()
    {
        if (matchState != EMatchState.InProgress)
        {
            SetMatchState(EMatchState.InProgress);
            Debug.Log($"EMatchState {EMatchState.InProgress}");
        }
    }

    private void SetMatchState(EMatchState state)
    {
        if (matchState == state) return;

        matchState = state;
        OnMatchStateChanged?.Invoke(matchState);
    }

    private void SetUIState(bool isUI)
    {
        if(isUI)
        {
            SetMatchState(EMatchState.Pause);
        }
        else
        {
            SetMatchState(EMatchState.InProgress);
        }
    }

    private void OnDisable()
    {
        uiManager.OnUIStateChanged -= SetUIState;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

public enum EMatchState
{
    WaitingToStart,
    InProgress,
    Pause,
    GameOver
}