using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDirectionStrategy : IProjectileDirectionStrategy
{
    public Vector3 GetDirection(Transform weaponTransform)
    {
        float randomAngle = Random.Range(0f, 360f);
        return new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad), 0);
    }
}
