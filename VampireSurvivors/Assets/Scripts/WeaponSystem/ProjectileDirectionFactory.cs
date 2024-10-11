using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ProjectileDirectionFactory
{
    public static IProjectileDirectionStrategy GetStrategy(ProjectileDirectionType directionType)
    {
        switch (directionType)
        {
            case ProjectileDirectionType.MOUSE:
                Debug.Log($"GetStrategy : ProjectileDirectionType.MOUSE");
                return new MouseDirectionStrategy();
            case ProjectileDirectionType.RANDOM:
                Debug.Log($"GetStrategy : ProjectileDirectionType.RANDOM");
                return new RandomDirectionStrategy();
            case ProjectileDirectionType.PlayerMovement:
                Debug.Log($"GetStrategy : ProjectileDirectionType.PlayerMovement");
                return new PlayerMovementDirectionStrategy();
            case ProjectileDirectionType.NearestEnemy:
                Debug.Log($"GetStrategy : ProjectileDirectionType.NearestEnemy");
                return new NearestEnemyDirectionStrategy();
            default:
                throw new System.ArgumentException("Invalid ProjectileDirectionType");
        }
    }
}
