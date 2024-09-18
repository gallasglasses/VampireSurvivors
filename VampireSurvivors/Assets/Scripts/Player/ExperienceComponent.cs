using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceComponent : MonoBehaviour
{
    private int currentXP = 0;
    private int currentLvl = 0;

    [SerializeField] private int maxLvlXP = 5;
    [SerializeField] private int deltaUpdateLvlXP = 10;
    //[SerializeField] private int maxLvl = 5;

    public void SetXP(int newXP)
    {
        int nextXP = Mathf.Clamp(currentXP + newXP, 0, maxLvlXP);
        if(nextXP == maxLvlXP)
        {
            if (currentXP + newXP > nextXP)
            {
                LevelUp();
                currentXP = currentXP + newXP - nextXP;
            }
            else
            {
                LevelUp();
            }
        }
        else
        {
            currentXP = nextXP;
        }
        Debug.Log("Current XP " + currentXP);
        Debug.Log("Current Lvl " + currentLvl);
    }

    private void LevelUp()
    {
        currentXP = 0;
        currentLvl++;
        maxLvlXP += deltaUpdateLvlXP;
    }
}
