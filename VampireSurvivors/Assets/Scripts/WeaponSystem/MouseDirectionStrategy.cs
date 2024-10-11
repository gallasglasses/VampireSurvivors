using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDirectionStrategy : IProjectileDirectionStrategy
{
    ///private Camera ñamera;

    //public MouseDirectionStrategy(Camera camera)
    //{
    //    mainCamera = camera;
    //}

    public Vector3 GetDirection(Transform weaponTransform)
    {
        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (camera == null)
        {
            Debug.LogError("mainCamera not found!");
        }
        Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - weaponTransform.position).normalized;
        return new Vector3(direction.x, direction.y, 0);
    }
}