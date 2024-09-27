using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    public void ContinueGame()
    {
        playerController.HandlePause();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
