using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    }

    private void OnEnable()
    {
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
        move.Disable();
        fire.Disable();
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

    public void HandleDeath() // rewrite! bad code
    {
        GameManager.Instance.Pause();
        uiManager.ToggleDeadMenu();
    }
}
