using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconPulseOnHover : MonoBehaviour
{
    public Animator iconAnimator;

    private void Start()
    {
        if (iconAnimator != null)
        {
            iconAnimator.enabled = false;
        }
    }

    public void TriggerAnimation()
    {
        if (!iconAnimator.enabled && iconAnimator != null)
        {
            iconAnimator.enabled = true;
            iconAnimator.Play("IconPulse");
        }

    }

    public void StopAnimation()
    {
        if (iconAnimator != null)
        {
            iconAnimator.enabled = false;
        }
    }
}
