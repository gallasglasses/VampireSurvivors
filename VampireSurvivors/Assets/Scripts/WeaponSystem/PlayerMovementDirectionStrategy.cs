using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementDirectionStrategy : IProjectileDirectionStrategy
{
    private Vector2 playerMovementVector;

    public PlayerMovementDirectionStrategy(PlayerController playerController)
    {
        var playerMovement = GameManager.Instance.player.GetComponent<Movement>();
        if (playerMovement != null)
        {
            playerMovementVector = playerMovement.PlayerMovementDirection.normalized;
        }
    }

    public Vector3 GetDirection(Transform weaponTransform)
    {
        return playerMovementVector;
    }
}