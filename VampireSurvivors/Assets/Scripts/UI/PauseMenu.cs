using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject uiBackground;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
            return;
        if (playerController == null)
        {
            playerController = GameObject.Find("Player")?.GetComponent<PlayerController>();
            if (playerController == null)
            {
                Debug.LogError("playerController not found!");
            }
        }
    }

    public void ContinueGame()
    {
        playerController.HandleUIUnause();
    }

    public void QuitGame()
    {
        ToggleUI();
        Application.Quit();
    }

    public void ToggleUI()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
        uiBackground.SetActive(!uiBackground.activeSelf);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
