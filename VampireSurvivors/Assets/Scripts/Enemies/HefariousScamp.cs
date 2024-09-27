using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HefariousScamp : Enemy
{
    private HefariousScampMovement hefariousScampMovement;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (TryGetComponent<HefariousScampMovement>(out HefariousScampMovement hefariousScampMovement))
        {
            hefariousScampMovement.OnExclusion += HandleRelease;
        }
    }
    protected override void Unsubcribe()
    {
        base.Unsubcribe();

        if (hefariousScampMovement != null)
        {
            hefariousScampMovement.OnExclusion -= HandleRelease;
        }
    }
}
