using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public GameObject fireworksEffect;

    public void TriggerFireworks()
    {
        if (fireworksEffect != null)
        {
            fireworksEffect.SetActive(true);
        }
    }

    void DisableFireworks()
    {
        fireworksEffect.SetActive(false);
    }
}
