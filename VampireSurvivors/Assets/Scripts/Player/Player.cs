using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Slider levelBarSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    private HealthComponent healthComponent;
    private ExperienceComponent experienceComponent;
    private PlayerController playerController;

    void OnEnable()
    {
        healthComponent = GetComponent<HealthComponent>();
        if (healthComponent != null )
        {
            healthComponent.OnHealthChanged += UpdateHealthBar;
            healthComponent.OnDeath += HandleDeath;
        }
        UpdateHealthBar();

        experienceComponent = GetComponent<ExperienceComponent>();
        if (experienceComponent != null )
        {
            experienceComponent.OnXPChanged += UpdateXPBar;
            experienceComponent.OnLevelUp += UpdateLevelText;
        }
    }

    void UpdateXPBar()
    {
        levelBarSlider.value = experienceComponent.GetXPPercent();
    }

    void UpdateLevelText()
    {
        levelText.text = experienceComponent.GetLevel().ToString();
    }

    void UpdateHealthBar()
    {
        healthBarFill.fillAmount = healthComponent.GetHealthPercent();
    }

    // rewrite bad code
    void HandleDeath()
    {
        Debug.Log("DEAD");
        playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.HandleDeath();
        }
    }
}
