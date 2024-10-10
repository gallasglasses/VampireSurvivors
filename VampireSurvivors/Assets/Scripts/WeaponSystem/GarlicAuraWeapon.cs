using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicAuraWeapon : Weapon
{
    private GarlicAura garlicAura;

    public GarlicAuraWeapon(float cooldown, GarlicAura aura)
    {
        Cooldown = cooldown;
        garlicAura = aura;
    }

    public override void Attack()
    {
        if (!canFire) return;

        if (!garlicAura.IsEnabled)
        {
            garlicAura.EnableAura();
        }
        else
        {
            garlicAura.SetPowerUpDamage(1.0f);
        }

        canFire = false;
    }
}
