using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Transform playerTransform;
    public Player player;

    [SerializeField] private UIManager uiManager;

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
        SetMatchState(EMatchState.InProgress);
        uiManager.OnUIStateChanged += SetUIState;
    }

    private void Start()
    {
        SetMatchState(EMatchState.InProgress);
        uiManager.OnUIStateChanged += SetUIState;
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