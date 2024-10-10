using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI maxLevelText;
    [SerializeField] private TextMeshProUGUI maxEnemiesKilledText;

    private SaveManager saveManager;

    private void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        PlayerStats stats = saveManager.LoadStats();

        maxLevelText.text = stats.maxLevel.ToString();
        maxEnemiesKilledText.text = stats.maxEnemiesKilled.ToString();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
