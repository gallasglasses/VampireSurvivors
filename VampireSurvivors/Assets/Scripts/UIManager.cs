using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Reward_UI rewardUI;
    private bool isUIEnable;
    [SerializeField] private PauseMenu uiPauseMenu;
    [SerializeField] private DeadMenu uiDeadMenu;

    public delegate void OnUIStateChangedEvent(bool _isUIEnable);
    public event OnUIStateChangedEvent OnUIStateChanged;

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

    //private void Start()
    //{
    //    if (rewardUI == null)
    //    {
    //        rewardUI = GameObject.Find("Reward")?.GetComponent<Reward_UI>();
    //        if (rewardUI == null)
    //        {
    //            Debug.LogError("Reward_UI not found!");
    //        }
    //    }

    //    if (uiDeadMenu == null)
    //    {
    //        uiDeadMenu = GameObject.Find("DeadMenu")?.GetComponent<DeadMenu>();
    //        if (uiDeadMenu == null)
    //        {
    //            Debug.LogError("DeadMenu not found!");
    //        }
    //    }

    //    if (uiPauseMenu == null)
    //    {
    //        uiPauseMenu = GameObject.Find("PauseMenu")?.GetComponent<PauseMenu>();
    //        if (uiPauseMenu == null)
    //        {
    //            Debug.LogError("PauseMenu not found!");
    //        }
    //    }
    //}

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
            return;
        if (rewardUI == null)
        {
            rewardUI = GameObject.Find("Reward")?.GetComponent<Reward_UI>();
            if (rewardUI == null)
            {
                Debug.LogError("Reward_UI not found after scene load!");
            }
        }

        if (uiDeadMenu == null)
        {
            uiDeadMenu = GameObject.Find("DeadMenu")?.GetComponent<DeadMenu>();
            if (uiDeadMenu == null)
            {
                Debug.LogError("DeadMenu not found after scene load!");
            }
        }

        if (uiPauseMenu == null)
        {
            uiPauseMenu = GameObject.Find("PauseMenu")?.GetComponent<PauseMenu>();
            if (uiPauseMenu == null)
            {
                Debug.LogError("PauseMenu not found after scene load!");
            }
        }
    }

    public void DisplayRewards(List<Reward> rewards)
    {
        EnableUI();
        rewardUI.RefreshRewardPanel(rewards);
    }

    public void EnableUI()
    {
        isUIEnable = true;
        OnUIStateChanged?.Invoke(isUIEnable);
    }

    public void DisableUI()
    {
        isUIEnable = false;
        OnUIStateChanged?.Invoke(isUIEnable);
    }

    // rewrite! bad code
    public void TogglePauseMenu()
    {
        uiPauseMenu.ToggleUI();
    }

    public void ToggleDeadMenu()
    {
        uiDeadMenu.ToggleUI();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
