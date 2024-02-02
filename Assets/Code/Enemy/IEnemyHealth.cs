using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IEnemyHealth : MonoBehaviour
{

    float health;
    [SerializeField] float maxHealth;
    [SerializeField] IEnemyAI enemyAI;
    public IEnemyAI EnemyAI() { return enemyAI; }
    public static Action<IEnemyHealth> OnEnemyDeath;

    [SerializeField] float healPlayerOnDeath = 5f;
    public float HealPlayerOnDeath { get { return healPlayerOnDeath; } }

    bool killedByExplosion = false;
    public bool WasKilledByExplosion() { return killedByExplosion; }

    [SerializeField] ParticleSystem hitParticles;
    public IExplode explosiveComponent;

    void Start()
    {
        health = maxHealth;
    }
    public void TakeDamage(float amount, bool explosiveDamage = false, bool collateral = false)
    {
        if (health == 0) return;
        killedByExplosion = explosiveDamage;

        health -= amount;
        if (hitParticles != null) hitParticles.Play();

        if(GameController.GetGameController().GetPlayer().WeaponController.LastShot() == WeaponController.ShotType.SHOTGUN_SHOT && !explosiveDamage)
        {
            AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.ShotgunHit, transform.position);
        }

        PlayerHitmarkerUI.OnHitmark();

        if(enemyAI.EnemyState() != EnemyStates.STOMP) enemyAI.SetHitState();
        if (health <= 0)
        {
            health = 0;
            Die();
            if (collateral)
            {
                ArchievementsNameConstantsData.MetallicPinchitoAccomplisher();
            }
        }
    }

    protected virtual void Die()
    {
        OnEnemyDeath?.Invoke(this);
        GameController.GetGameController().GetSpawner().RemoveEnemy(this);
        enemyAI.SetDieState();
    }

}
