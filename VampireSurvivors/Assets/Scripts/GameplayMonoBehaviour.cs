using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class GameplayMonoBehaviour : MonoBehaviour
{
    private static bool isPaused;
    protected float originalAnimationSpeed;
    public static bool Paused { get { return isPaused; } set { isPaused = value; } }

    private float pauseStartTime;
    private float pauseDuration = 0f;

    protected virtual void Awake()
    {
        if(GameManager.Instance == null) return;

        GameManager.Instance.OnMatchStateChanged += OnMatchStateChanged;
    }

    protected virtual void Update()
    {
        if (!isPaused) 
        { 
            UnPausableUpdate(); 
        }
        else
        {
            PausableUpdate();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!isPaused) 
        { 
            UnPausableFixedUpdate();
        }
        else
        {
            PausableFixedUpdate();
        }
    }

    protected virtual void UnPausableUpdate() { }

    protected virtual void UnPausableFixedUpdate() { }
    protected virtual void PausableUpdate() { }

    protected virtual void PausableFixedUpdate() { }

    protected void OnMatchStateChanged(EMatchState state)
    {
        switch(state)
        {
            case EMatchState.Pause:
                Pause();
                break;
            case EMatchState.InProgress:
                UnPause();
                break;
            case EMatchState.GameOver:
                Pause();
                break;
            case EMatchState.WaitingToStart:
                break;
        }
    }

    protected float GetPauseDuration()
    {
        return pauseDuration;
    }

    private void Pause()
    {
        isPaused = true;
        pauseStartTime = Time.time;
    }

    private void UnPause()
    {
        isPaused = false;
        float currentTime = Time.time;
        pauseDuration += currentTime - pauseStartTime;
    }

    protected void ResetDuration()
    {
        pauseDuration = 0f;
    }
}
