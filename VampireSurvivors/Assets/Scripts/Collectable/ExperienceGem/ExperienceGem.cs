using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExperienceGem : Collectable
{
    private ObjectPool<ExperienceGem> _pool;
    [SerializeField] private TypeXPGem typeXPGem { get; set; } //set from enemy
    private SpriteRenderer spriteRenderer;


    protected override bool CanBeCollected(Player _player)
    {
        if (_player.TryGetComponent<ExperienceComponent>(out ExperienceComponent experienceComponent))
        {
            //Debug.Log("CanBeCollected true");
            experienceComponent.TryToAddXP(typeXPGem);
            return true;
        }
        else
        {
            //Debug.Log("CanBeCollected false");
            return false;
        }
    }

    public void SetPool(ObjectPool<ExperienceGem> pool)
    {
        _pool = pool;
    }

    protected override void HandleDestroy()
    {
        _pool.Release(this);
    }

    public void SetTypeXPGem(TypeXPGem newTypeXPGem)
    {
        typeXPGem = newTypeXPGem;
    }

    public void UpdateGemSprite(Sprite updatedSprite)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = updatedSprite;
        }
    }
}

 public enum TypeXPGem
{
    BLUE,
    GREEN,
    RED
}
