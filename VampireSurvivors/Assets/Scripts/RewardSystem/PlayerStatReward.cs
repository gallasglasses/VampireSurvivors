using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStatReward", menuName = "Rewards/PlayerStatReward")]
public class PlayerStatReward : RewardObject
{
    public enum StatType { Health, AutoHeal, Speed, Damage }
    public StatType statType;
    public float statIncreaseAmount;
}
