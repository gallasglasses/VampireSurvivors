using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
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
        //UpdateHealthBar();

        experienceComponent = GetComponent<ExperienceComponent>();
        if (experienceComponent != null )
        {
            experienceComponent.OnXPChanged += UpdateXPBar;
            experienceComponent.OnLevelUp += UpdateLevelText;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transform.position = Vector3.zero;
        if (healthBarFill == null)
        {
            healthBarFill = GameObject.Find("HealthFill").GetComponent<Image>();
            if (healthBarFill == null)
            {
                Debug.LogError("HealthFill not found!");
            }
        }

        if (levelBarSlider == null)
        {
            levelBarSlider = GameObject.Find("LvlBar").GetComponent<Slider>();
            if (levelBarSlider == null)
            {
                Debug.LogError("levelBarSlider not found!");
            }
        }

        if (levelText == null)
        {
            levelText = GameObject.Find("Level").GetComponent<TextMeshProUGUI>();
            if (levelText == null)
            {
                Debug.LogError("levelText not found!");
            }
        }
    }

    void UpdateXPBar()
    {
        levelBarSlider.value = experienceComponent.GetXPPercent();
    }

    void UpdateLevelText()
    {
        levelText.text = GetCurrentLevel().ToString();
    }

    public int GetCurrentLevel()
    {
        return experienceComponent.GetLevel();
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

    private void OnDisable()
    {
        if (healthComponent != null)
        {
            healthComponent.OnHealthChanged -= UpdateHealthBar;
            healthComponent.OnDeath -= HandleDeath;
        }
        if (experienceComponent != null)
        {
            experienceComponent.OnXPChanged -= UpdateXPBar;
            experienceComponent.OnLevelUp -= UpdateLevelText;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
