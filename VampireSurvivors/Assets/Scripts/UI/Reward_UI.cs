using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reward_UI : MonoBehaviour
{
    public GameObject rewardPanel;
    public GameObject slotPrefab;
    public VerticalLayoutGroup vertLayoutGroup;
    public int numberOfSlots = 4;
    public RewardManager rewardManager;

    public List<RewardSlot_UI> slots = new List<RewardSlot_UI>();

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        CreateSlots();

        rewardPanel.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (rewardManager == null)
        {
            rewardManager = GameObject.Find("RewardManager")?.GetComponent<RewardManager>();
            if (rewardManager == null)
            {
                Debug.LogError("rewardManager not found!");
            }
        }
    }

    public void RefreshRewardPanel (List<Reward> rewards)
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            slots[i].UpdateSlot(rewards[i]);
        }
        ToggleInventory();
    }

    private void CreateSlots()
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject slotInstance = Instantiate(slotPrefab, vertLayoutGroup.transform);
            RewardSlot_UI slotUI = slotInstance.GetComponent<RewardSlot_UI>();
            slotUI.Initialize(this);
            slots.Add(slotUI);
        }
    }

    public void ToggleInventory()
    {
        rewardPanel.SetActive(!rewardPanel.activeSelf);
    }

    public void AcceptReward(TypeReward type)
    {
        ToggleInventory();
        rewardManager.ActivateReward(type);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
