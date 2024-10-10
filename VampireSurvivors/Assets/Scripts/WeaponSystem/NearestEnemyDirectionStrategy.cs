using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestEnemyDirectionStrategy : IProjectileDirectionStrategy
{
    private Transform FindNearestEnemy(Transform weaponTransform)
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = weaponTransform.position;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                nearestEnemy = enemy.transform;
                minDistance = distance;
            }
        }

        return nearestEnemy;
    }

    public Vector3 GetDirection(Transform weaponTransform)
    {
        Transform nearestEnemy = FindNearestEnemy(weaponTransform);
        if (nearestEnemy != null)
        {
            return (nearestEnemy.position - weaponTransform.position).normalized;
        }

        return new RandomDirectionStrategy().GetDirection(weaponTransform);
    }
}