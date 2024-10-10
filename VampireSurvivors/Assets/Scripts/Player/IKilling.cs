using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKilling
{
    void DeliverEnemy(GameObject enemy);
    
}

public class KillingActor : GameplayMonoBehaviour, IKilling
{
    KillingComponent killingComponent;

    public void DeliverEnemy(GameObject enemy)
    {
        killingComponent = FindObjectOfType<KillingComponent>();
        if (killingComponent != null)
        {
            killingComponent.FoundEnemy(enemy);
        }
    }
}
