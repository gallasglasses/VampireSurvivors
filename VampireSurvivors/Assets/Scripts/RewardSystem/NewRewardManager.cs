using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class NewRewardManager : MonoBehaviour
{
    public static NewRewardManager Instance { get; private set; }

    [SerializeField] private WeaponComponent weaponComponent;
    [SerializeField] private UIManager uiManager;

    [SerializeField] private RewardsInventory rewards;

    private HashSet<AuraType> obtainedAuraWeapons = new HashSet<AuraType>();
    private HashSet<ProjectileType> obtainedProjectileWeapons = new HashSet<ProjectileType>();
    private ExperienceComponent xpComponent;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        xpComponent = GameManager.Instance.player.GetComponent<ExperienceComponent>();
        if (xpComponent != null)
        {
            xpComponent.OnLevelUp += DisplayOfferedRewards;
        }
    }

    public void DeleteReward(RewardObject removableReward)
    {
        if (rewards.rewardsInventory.Contains(removableReward))
        {
            rewards.rewardsInventory.Remove(removableReward);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(rewards);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }

    public void AddRewards(List<RewardObject> newRewards)
    {
        foreach (var reward in newRewards)
        {
            if (!rewards.rewardsInventory.Contains(reward))
            {
                rewards.rewardsInventory.Add(reward);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(rewards);
                UnityEditor.AssetDatabase.SaveAssets();
#endif
            }
        }


    }

    private void DisplayOfferedRewards()
    {
        List<RewardObject> filteredRewards = rewards.rewardsInventory.Where(reward =>
        {
            if (reward is WeaponUpgradeReward upgradeReward)
            {
                if (upgradeReward.weaponType == WeaponType.AURA && !obtainedAuraWeapons.Contains(upgradeReward.auraType))
                {
                    return false;
                }
                if (upgradeReward.weaponType == WeaponType.PROJECTILE && !obtainedProjectileWeapons.Contains(upgradeReward.projectileType))
                {
                    return false;
                }
            }
            return true;
        }).ToList();

        List<RewardObject> offeredRewards = filteredRewards
            .OrderBy(r => Random.Range(0, filteredRewards.Count))
            .Take(4)
            .ToList();

        uiManager.DisplayRewards(offeredRewards);
    }

    public void ApplyReward(RewardObject reward)
    {
        uiManager.DisableUI();
        if (reward is PlayerStatReward playerStatReward)
        {
            ApplyPlayerStatReward(playerStatReward);
        }
        else if (reward is WeaponReward weaponReward)
        {
            ApplyWeaponReward(weaponReward);
        }
        else if (reward is WeaponUpgradeReward weaponUpgradeReward)
        {
            ApplyWeaponUpgradeReward(weaponUpgradeReward);
        }
    }

    private void ApplyPlayerStatReward(PlayerStatReward reward)
    {
        switch (reward.statType)
        {
            case PlayerStatReward.StatType.Health:
                IncreaseHealth();
                break;
            case PlayerStatReward.StatType.AutoHeal:
                IncreaseAutoHeal();
                break;
            case PlayerStatReward.StatType.Speed:
                IncreaseSpeed();
                break;
            case PlayerStatReward.StatType.Damage:
                IncreaseDamage();
                break;
        }
    }

    private void ApplyWeaponReward(WeaponReward reward)
    {
        if(reward.weaponType == WeaponType.AURA)
        {
            if (!obtainedAuraWeapons.Contains(reward.auraType))
            {
                obtainedAuraWeapons.Add(reward.auraType);
                // Logic for adding new weapons to the player's inventory

                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                Debug.Log($"Player obtained {reward.auraType}!");
            }
            else
            {
                Debug.Log($"{reward.auraType} is already obtained!");
            }
        }

        if (reward.weaponType == WeaponType.PROJECTILE)
        {
            if (!obtainedProjectileWeapons.Contains(reward.projectileType))
            {
                obtainedProjectileWeapons.Add(reward.projectileType);
                // Logic for adding new weapons to the player's inventory

                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                Debug.Log($"Player obtained {reward.projectileType}!");
            }
            else
            {
                Debug.Log($"{reward.projectileType} is already obtained!");
            }
        }
    }
    private void ApplyWeaponUpgradeReward(WeaponUpgradeReward reward)
    {
        if (reward.weaponType == WeaponType.AURA)
        {
            if (obtainedAuraWeapons.Contains(reward.auraType))
            {
                // Logic for upgrading already acquired weapons

                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                switch (reward.weaponUpgradeType)
                {
                    case WeaponUpgradeType.Cooldown:
                        // !!!!
                        break;
                    case WeaponUpgradeType.Damage:
                        // !!!!
                        break;
                    case WeaponUpgradeType.EffectArea:
                        // !!!!
                        break;
                }
                Debug.Log($"Upgrading {reward.auraType}.");
            }
            else
            {
                Debug.Log($"Cannot upgrade {reward.auraType}, as it hasn't been obtained yet.");
            }
        }
        if (reward.weaponType == WeaponType.PROJECTILE)
        {
            if (obtainedProjectileWeapons.Contains(reward.projectileType))
            {
                // Logic for upgrading already acquired weapons

                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                switch (reward.weaponUpgradeType)
                {
                    case WeaponUpgradeType.Cooldown:
                        // !!!!
                        break;
                    case WeaponUpgradeType.Damage:
                        // !!!!
                        break;
                    case WeaponUpgradeType.AdditionalProjectile:
                        // !!!!
                        break;
                }
                Debug.Log($"Upgrading {reward.projectileType}.");
            }
            else
            {
                Debug.Log($"Cannot upgrade {reward.projectileType}, as it hasn't been obtained yet.");
            }
        }

    }

    void IncreaseHealth()
    {
        var healthComponent = GameManager.Instance.player.GetComponent<PlayerHealthComponent>();
        if (healthComponent != null)
        {
            Debug.Log("Reward IncreaseHealth");
            healthComponent.PowerUpHealth();
        }
    }

    void IncreaseAutoHeal()
    {
        var healthComponent = GameManager.Instance.player.GetComponent<PlayerHealthComponent>();
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

}
