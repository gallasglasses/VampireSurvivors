using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDirectionStrategy : IProjectileDirectionStrategy
{
    private Camera mainCamera;

    public MouseDirectionStrategy(Camera camera)
    {
        mainCamera = camera;
    }

    public Vector3 GetDirection(Transform weaponTransform)
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - weaponTransform.position).normalized;
        return new Vector3(direction.x, direction.y, 0);
    }
}