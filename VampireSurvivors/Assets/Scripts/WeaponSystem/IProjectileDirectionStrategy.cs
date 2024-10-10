using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileDirectionStrategy
{
    Vector3 GetDirection(Transform weaponTransform);
}
