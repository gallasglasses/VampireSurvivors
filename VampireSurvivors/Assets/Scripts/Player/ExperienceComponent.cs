using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class ExperienceComponent : MonoBehaviour
{
    private int currentXP = 0;
    private int currentLvl = 0;

    [SerializeField] private int maxLvlXP = 5;
    [SerializeField] private int deltaUpdateLvlXP = 10;
    public event Action OnXPChanged;
    public event Action OnLevelUp;

    [SerializeField] private MyDictionary<TypeXPGem, int> experienceDictionary;
    private Dictionary<TypeXPGem, int> xpDictionary = new Dictionary<TypeXPGem, int>();


    private void Start()
    {
        xpDictionary = experienceDictionary.ToDictionary();
        SetXP(currentXP);
    }

    public void TryToAddXP(TypeXPGem newXPType)
    {
        if(xpDictionary.TryGetValue(newXPType, out int newXP))
        {
            AddXP(currentXP + newXP);
        }
    }

    private void AddXP(int newXP)
    {
        if (newXP >= maxLvlXP)
        {
            newXP = newXP - maxLvlXP;
            SetXP(maxLvlXP);
            LevelUp();
        }
        SetXP(newXP);
    }

    private void SetXP(int XP)
    {
        var nextXP = Mathf.Clamp(XP, 0, maxLvlXP);
        currentXP = nextXP;

        if (OnXPChanged != null)
        {
            OnXPChanged?.Invoke();
        }
    }

    private void LevelUp()
    {
        SetXP(0);
        currentLvl++;
        maxLvlXP += deltaUpdateLvlXP;
        if (OnLevelUp != null)
        {
            OnLevelUp?.Invoke();
        }
    }

    public float GetXPPercent()
    {
        return (float)currentXP / maxLvlXP;
    }

    public int GetLevel()
    {
        return currentLvl;
    }

}

[Serializable]
public class MyDictionary<TKey, TValue>
{
    [SerializeField] public _MyDictionary<TKey, TValue>[] dictionary;

    public Dictionary<TKey, TValue> ToDictionary()
    {
        Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

        foreach (var item in dictionary)
        {
            dict.Add(item._myKey, item._myValue);
        }
        return dict;
    }
}

[Serializable]
public class _MyDictionary<TKey, TValue>
{
    [SerializeField] public TKey _myKey;
    [SerializeField] public TValue _myValue;
}
