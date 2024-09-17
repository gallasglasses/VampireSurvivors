using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeweringStalker : Enemy
{
    private SkeweringStalkerMovement skeweringStalkerMovement;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (TryGetComponent<SkeweringStalkerMovement>(out SkeweringStalkerMovement skeweringStalkerMovement))
        {
            skeweringStalkerMovement.OnExclusion += HandleDeath;
        }
    }
    protected override void Unsubcribe()
    {
        base.Unsubcribe(); // Unsubscribe from base events

        if (skeweringStalkerMovement != null)
        {
            skeweringStalkerMovement.OnExclusion -= HandleDeath;
        }
    }
}
