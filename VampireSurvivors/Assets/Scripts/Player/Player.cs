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
    [SerializeField] private TextMeshProUGUI currentAmountKilledEnemies;
    private HealthComponent healthComponent;
    private ExperienceComponent experienceComponent;
    private PlayerController playerController;
    private KillingComponent killingComponent;
    private PlayerStats _playerStats = new();

    public PlayerStats PlayerStats
    { get => _playerStats; }


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

        killingComponent = GetComponent<KillingComponent>();
        if (killingComponent != null )
        {
            killingComponent.OnKilledNewEnemy += UpdateCurrentAmountKilledEnemies;
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
        if (scene.buildIndex == 0)
            return;
        _playerStats = new();
        transform.position = Vector3.zero;
        if (healthBarFill == null)
        {
            healthBarFill = GameObject.Find("HealthFill").GetComponent<Image>();
            if (healthBarFill == null)
            {
                Debug.LogError("HealthFill not found!");
            }
        }
        else
        {
            UpdateHealthBar();
        }

        if (levelBarSlider == null)
        {
            levelBarSlider = GameObject.Find("LvlBar").GetComponent<Slider>();
            if (levelBarSlider == null)
            {
                Debug.LogError("levelBarSlider not found!");
            }
        }
        else
        {
            UpdateXPBar();
        }

        if (levelText == null)
        {
            levelText = GameObject.Find("Level").GetComponent<TextMeshProUGUI>();
            if (levelText == null)
            {
                Debug.LogError("levelText not found!");
            }
        }
        else
        {
            UpdateLevelText();
        }
    }

    void UpdateXPBar()
    {
        levelBarSlider.value = experienceComponent.GetXPPercent();
    }

    void UpdateLevelText()
    {
        Debug.Log("UpdateLevelText!");
        _playerStats.maxLevel = GetCurrentLevel();
        Debug.Log(_playerStats.maxLevel);
        levelText.text = GetCurrentLevel().ToString();
    }

    void UpdateCurrentAmountKilledEnemies()
    {
        _playerStats.maxEnemiesKilled = GetCurrentAmountKilledEnemies();
        currentAmountKilledEnemies.text = GetCurrentAmountKilledEnemies().ToString();
    }

    public int GetCurrentLevel()
    {
        return experienceComponent.GetLevel();
    }

    public int GetCurrentAmountKilledEnemies()
    {
        return killingComponent.EnemiesKilled;
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
