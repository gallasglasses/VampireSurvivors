using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput;
    private Movement playerMovement;
    private InputAction move;
    private InputAction fire;
    private InputAction pause;
    [SerializeField] private UIManager uiManager;

    private bool isLMBPressed = false;
    private bool isSpaceFirstPressed = false;

    public event Action<bool> OnAttack;

    private void Awake()
    {
        playerInput = new PlayerInput();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0)
            return;

        if(move != null && !move.enabled)
            move.Enable();
        if (fire != null && !fire.enabled)
            fire.Enable();
        if (pause != null && !pause.enabled)
            pause.Enable();
    }

    private void OnEnable()
    {
        playerInput.Enable();

        move = playerInput.Player.Move;
        move.Enable();

        fire = playerInput.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
        fire.canceled += Fire;

        pause = playerInput.UI.Pause;
        pause.Enable();
        pause.performed += Pause;

    }

    private void OnDisable()
    {
        fire.performed -= Fire;
        fire.canceled -= Fire;
        fire.Disable();
        move.Disable();

        pause.performed -= Pause;
        pause.Disable();

        playerInput.Disable();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        playerMovement = GameManager.Instance.player.GetComponent<Movement>();
    }

    void Update()
    {
        playerMovement.Move(move.ReadValue<Vector2>());
    }

    private void Fire(InputAction.CallbackContext callback)
    {
        //Debug.Log("fire");
        if (callback.performed)
        {
            isLMBPressed = true;
            //Debug.Log("isLMBPressed true");
            OnAttack.Invoke(isLMBPressed);
        }
        else if (callback.canceled)
        {
            isLMBPressed = false;
            //Debug.Log("isLMBPressed false");
            OnAttack.Invoke(isLMBPressed);
        }
    }

    private void Pause(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            HandlePause();
        }
    }

    public void HandlePause()
    {
        if (!isSpaceFirstPressed)
        {
            GameManager.Instance.Pause();
        }
        else
        {
            GameManager.Instance.UnPause();
        }
        uiManager.TogglePauseMenu();
        isSpaceFirstPressed = !isSpaceFirstPressed;
    }

    public void HandleUIUnause()
    {
        if (isSpaceFirstPressed)
        {
            GameManager.Instance.UnPause();
        }
        uiManager.TogglePauseMenu();
        isSpaceFirstPressed = !isSpaceFirstPressed;
    }

    public void HandleDeath() // rewrite! bad code
    {
        GameManager.Instance.GameOver();
        uiManager.ToggleDeadMenu();


        if (fire.enabled)
            fire.Disable();
        if (move.enabled)
            move.Disable();
        if (pause.enabled)
            pause.Disable();
    }
}
