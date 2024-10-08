using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject uiBackground;

    public void ResetGame()
    {
        ToggleUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        ToggleUI();
        SceneManager.LoadScene(0);
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
}
