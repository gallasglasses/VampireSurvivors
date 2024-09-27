using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public enum TypeReward
{
    Health,
    Speed,
    Damage,
    Projectile,
    Grenade,
    Aura,
    AutoHeal
}

public class RewardManager : MonoBehaviour
{
    private List<Reward> rewards = new List<Reward>();
    [SerializeField] private Effects effects;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private WeaponComponent weaponComponent;

    [SerializeField] private MyDictionary<TypeReward, Sprite> rewardsDictionary;
    public Dictionary<TypeReward, Sprite> rewardsDict = new Dictionary<TypeReward, Sprite>();
    private ExperienceComponent xpComponent;

    void Start()
    {
        rewardsDict = rewardsDictionary.ToDictionary();
        SetRewardList();

        xpComponent = GameManager.Instance.player.GetComponent<ExperienceComponent>();
        if (xpComponent != null )
        {
            xpComponent.OnLevelUp += OnLevelUp;
        }
    }

    void SetRewardList()
    {
        foreach (var reward in rewardsDict)
        {
            rewards.Add(new Reward(reward.Key, reward.Key.ToString(), reward.Value, () => ActivateReward(reward.Key)));
        }
    }

    public void ActivateReward(TypeReward typeReward)
    {
        uiManager.DisableUI();
        switch (typeReward)
        {
            case TypeReward.Health:
                IncreaseHealth();
                break;
            case TypeReward.Speed: 
                IncreaseSpeed(); 
                break;  
            case TypeReward.Damage:
                IncreaseDamage();
                break;
            case TypeReward.Projectile:
                IncreaseProjectiles();
                break;
            case TypeReward.Grenade:
                IncreaseGrenade();
                break;
            case TypeReward.Aura:
                IncreaseAura();
                break;
            case TypeReward.AutoHeal:
                IncreaseAutoHeal();
                break;
            default:
                break;
        }
        effects.TriggerFireworks();
    }

    private void OnLevelUp()
    {
        List<Reward> offeredRewards = new List<Reward>();
        while (offeredRewards.Count < 4)
        {
            var randomReward = rewards[Random.Range(0, rewards.Count)];
            if (!offeredRewards.Contains(randomReward))
            {
                offeredRewards.Add(randomReward);
            }
        }
        uiManager.DisplayRewards(offeredRewards);
    }

    void IncreaseHealth()
    {
        var healthComponent = GameManager.Instance.player.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            Debug.Log("Reward IncreaseHealth");
            healthComponent.PowerUpHealth();
        }
    }

    void IncreaseAutoHeal()
    {
        var healthComponent = GameManager.Instance.player.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            Debug.Log("Reward IncreaseAutoHeal");
            healthComponent.PowerUpAutoHeal();
        }
    }

    void IncreaseSpeed()
    {
        var movement = GameManager.Instance.player.GetComponent<Movement>();
        if (movement != null)
        {
            Debug.Log("Reward IncreaseSpeed");
            movement.PowerUpSpeed();
        }
    }

    void IncreaseDamage()
    {
        if (weaponComponent != null)
        {
            Debug.Log("Reward IncreaseDamage");
            weaponComponent.PowerUpDamage();
        }
    }

    void IncreaseProjectiles()
    {
        if (weaponComponent != null)
        {
            Debug.Log("Reward IncreaseProjectiles");
            weaponComponent.PowerUpProjectile();
        }
    }

    void IncreaseGrenade()
    {
        if (weaponComponent != null)
        {
            Debug.Log("Reward IncreaseGrenade");

        }
    }

    void IncreaseAura()
    {
        if (weaponComponent != null)
        {
            Debug.Log("Reward IncreaseAura");
            weaponComponent.PowerUpGarlic();
        }
    }
}

public class Reward
{
    public TypeReward typeReward;
    public string rewardName;
    public Sprite icon;
    public Action applyEffect;

    public Reward(TypeReward type, string name, Sprite rewardIcon, Action effect)
    {
        typeReward = type;
        rewardName = name;
        icon = rewardIcon;
        applyEffect = effect;
    }
}