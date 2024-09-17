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

    private bool isLMBPressed = false;

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


}
