using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float projectileDamage = 1f;
    [SerializeField] ParticleSystem particlesOnHit;
    public TrailRenderer trail;
   public Rigidbody rbody;

    bool hasCausedDamage = false;

    GameObject parent;

    public GameObject ProjectileParent() { return parent; }
    public void SetParent(GameObject parent) { this.parent = parent; }
    private void Start()
    {
        float damageOnDifficulty;
        DifficultyType difficulty = GameController.GetGameController().GetDifficulty().currentDifficulty;
        if(difficulty == DifficultyType.EASY)
        {
            damageOnDifficulty = GameController.GetGameController().GetDifficulty().TurretDamageEasyMode;
        }
        else if(difficulty == DifficultyType.MEDIUM)
        {
            damageOnDifficulty = GameController.GetGameController().GetDifficulty().TurretDamageMediumMode;
        }
        else 
        {
            damageOnDifficulty = GameController.GetGameController().GetDifficulty().TurretDamageHardMode;
        }

        projectileDamage = damageOnDifficulty;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!hasCausedDamage)
            {
                GameController.GetGameController().GetPlayer().HealthSystem.TakeDamage(projectileDamage);
                if (!GameController.GetGameController().GetPlayer().HealthSystem.IsInvencible()) PlayerDamageIndicatorUI.CreateIndicator(parent.transform);
                hasCausedDamage = true;
            }
        }
        DestroyProjectile();
    }

    void DestroyProjectile()
    {
        Transform particlesOnHitTransform = particlesOnHit.transform;
        particlesOnHitTransform.SetParent(null);
        particlesOnHitTransform.position = transform.position;
        particlesOnHitTransform.rotation = Quaternion.identity;
        particlesOnHit.Play();

        gameObject.SetActive(false);
    }
}
