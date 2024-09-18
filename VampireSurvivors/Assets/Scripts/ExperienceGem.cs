using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Collectable
{
    private int XP; /*{ get; set; }*/
    [SerializeField] private TypeXPGem typeXPGem; /*{ get; set; }*/ //set from enemy
    public Dictionary<TypeXPGem, int> dictionary;

    void Start()
    {
        XP = dictionary[typeXPGem];
        Debug.Log("XP " + XP);
    }

    protected override bool CanBeCollected(Player _player)
    {
        if (_player.TryGetComponent<ExperienceComponent>(out ExperienceComponent experienceComponent))
        {
            Debug.Log("CanBeCollected true");
            experienceComponent.SetXP(XP);
            return true;
        }
        else
        {
            Debug.Log("CanBeCollected false");
            return false;
        }
    }

}
 public enum TypeXPGem
{
    BLUE,
    GREEN,
    RED
}
