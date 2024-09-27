using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDodge : GameplayMonoBehaviour
{
    [SerializeField] private float dodgeSpeed = 2f;
    [SerializeField] private float dodgeDistance = 1f;
    [SerializeField] private float dodgeDuration = 0.2f;
    [SerializeField] private float detectionRadius = 0.5f;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private LayerMask projectileLayer;

    private Vector2 dodgeDirection;
    private bool isDodging = false;
    private float currentDodgeTime = 0f;
    private float checkInterval = 0.2f;
    private float lastCheckTime = 0f;


    protected override void UnPausableUpdate()
    {
        if (GetPauseDuration() > 0)
        {
            lastCheckTime += GetPauseDuration();
            ResetDuration();
        }

        if (Time.time >= lastCheckTime + checkInterval)
        {
            lastCheckTime = Time.time;

            Collider2D[] projectiles = Physics2D.OverlapCircleAll(transform.position, detectionRadius, projectileLayer);

            if (projectiles.Length > 0 && !isDodging)
            {
                Collider2D closestProjectile = GetClosestProjectile(projectiles);
                dodgeDirection = GetDodgeDirection(closestProjectile);
                currentDodgeTime = 0f;
                isDodging = true;
                enemyMovement.Dodge(isDodging);
            }
        }
        if (isDodging)
        {
            currentDodgeTime += Time.deltaTime;
            transform.position += (Vector3)(dodgeDirection * dodgeSpeed * Time.deltaTime);

            if (currentDodgeTime >= dodgeDuration)
            {
                isDodging = false;
                enemyMovement.Dodge(isDodging);
            }
        }
    }

    Collider2D GetClosestProjectile(Collider2D[] projectiles)
    {
        Collider2D closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D projectile in projectiles)
        {
            float distance = Vector2.Distance(transform.position, projectile.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = projectile;
            }
        }
        return closest;
    }

    Vector2 GetDodgeDirection(Collider2D projectile)
    {
        Vector2 enemyPosition = transform.position;
        Vector2 projectilePosition = projectile.transform.position;
        Vector2 direction = (enemyPosition - projectilePosition).normalized;

        return direction * dodgeDistance;
    }
}
