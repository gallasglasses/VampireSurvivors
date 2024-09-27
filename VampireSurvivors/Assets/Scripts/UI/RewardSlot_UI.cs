using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using static UnityEditor.Progress;

public class RewardSlot_UI : MonoBehaviour
{
    public Image itemIcon;
    public Button itemButton;
    public TextMeshProUGUI nameText;
    public TypeReward typeReward;

    private Reward_UI rewardUI;

    void Start()
    {
        Button iconButton = itemIcon.GetComponent<Button>();
        if (itemButton != null)
        {
            itemButton.onClick.AddListener(OnRewardIconClick);
        }
    }

    public void UpdateSlot(Reward slot)
    {
        if (slot != null)
        {
            typeReward = slot.typeReward;
            itemIcon.sprite = slot.icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            nameText.text = slot.rewardName;
        }
        else
        {
            itemIcon.sprite = null;
            itemIcon.color = new Color(1, 1, 1, 0);
            nameText.text = "";
        }
        var pulsingIcon = itemIcon.GetComponent<IconPulseOnHover>();
        if (pulsingIcon != null)
        {
            pulsingIcon.StopAnimation();
        }
    }

    public void Initialize(Reward_UI parentInventoryUI)
    {
        rewardUI = parentInventoryUI;
    }

    private void OnRewardIconClick()
    {
        rewardUI.AcceptReward(typeReward);
    }
}
