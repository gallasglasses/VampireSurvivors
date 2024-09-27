using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Reward_UI rewardUI;
    private bool isUIEnable;
    [SerializeField] private GameObject uiPauseMenu;
    [SerializeField] private GameObject uiDeadMenu;

    public delegate void OnUIStateChangedEvent(bool _isUIEnable);
    public event OnUIStateChangedEvent OnUIStateChanged;

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
        uiPauseMenu.SetActive(!uiPauseMenu.activeSelf);
    }

    public void ToggleDeadMenu()
    {
        uiDeadMenu.SetActive(!uiDeadMenu.activeSelf);
    }
}
