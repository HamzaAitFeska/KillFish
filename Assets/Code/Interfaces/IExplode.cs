using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IExplode : MonoBehaviour
{
    [SerializeField] protected float maxDamage = 25f;
    [SerializeField] protected float minDamage = 10f;
    [SerializeField] protected float maxDistance = 10f;

    GameObject explosionParticle;

    protected bool hasExploted = false;
    public static Action OnExloded;

    [Header("Camera Shake related")]
    [SerializeField] protected float maxDistanceToShake;
    [SerializeField] protected CameraShakeInstanceVariables minExplosionShake;
    [SerializeField] protected CameraShakeInstanceVariables maxExplosionShake;
    protected CameraShakeInstanceVariables explosionShake;
    private void Start()
    {
        explosionParticle = Resources.Load<GameObject>("Particles/ExplosionParticle/ExplosionParticle");
    }
    public virtual void Explode()
    {
        if (hasExploted) return;
        hasExploted = true;
        //Play Sound here

        //CalculateDamage To Player
        CalculateDamage(GameController.GetGameController().GetPlayer().gameObject);

        //First we calculate how many enemies will be damaged
        List<IEnemyHealth> enemiesDamaged = GetCloseEnemies();
        Debug.Log(enemiesDamaged.Count);
        if(enemiesDamaged.Count >= 5)
        {
            ArchievementsNameConstantsData.RawdoggerAccomplisher();
        }
        //CalculateDamage To every enemy that is in range
        foreach (IEnemyHealth enemy in enemiesDamaged)
        {
            CalculateDamage(enemy.gameObject);
            
        }

        //Instantiate Particles
        Instantiate(explosionParticle, transform.position, Quaternion.identity);

        //Shake depending on distance to player
        ShakeExplosion();

        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.Explosion, transform.position);

        //Destroy
        Destroy(gameObject);
    }
    private List<IEnemyHealth> GetCloseEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance);
        List<IEnemyHealth> closeEnemies = new List<IEnemyHealth>();

        foreach (Collider collider in colliders)
        {
            IEnemyHealth enemyHealth = collider.GetComponent<IEnemyHealth>();

            if (enemyHealth != null)
            {
                closeEnemies.Add(enemyHealth);
            }
        }

        return closeEnemies;
    }

    void CalculateDamage(GameObject other)
    {
        float distance = Vector3.Distance(transform.position, other.transform.position);

        float damage = 0;

        if (distance <= maxDistance)
        {
            float normalizedDistance = 1f - Mathf.Clamp01(distance / maxDistance);
            damage = Mathf.Lerp(minDamage, maxDamage, normalizedDistance);
        }
        else return;
        TryDealDamage(other, (int)damage);
    }

    void TryDealDamage(GameObject other, float damage)
    {
        if (other.GetComponent<IEnemyHealth>() != null)
        {
            other.GetComponent<IEnemyHealth>().TakeDamage(damage, true);
        }
        else if (other.GetComponent<PlayerHealth>() != null)
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    void ShakeExplosion()
    {
        float forceByDistance = 1 - (Vector3.Distance(GameController.GetGameController().GetPlayer().transform.position, transform.position) / maxDistanceToShake);
        forceByDistance = Mathf.Clamp(forceByDistance, 0, 1);

        CameraShakeInstanceVariables c = new CameraShakeInstanceVariables();
        c.magnitude = minExplosionShake.magnitude + (maxExplosionShake.magnitude - minExplosionShake.magnitude) * (forceByDistance);
        c.roughness = minExplosionShake.roughness + (maxExplosionShake.roughness - minExplosionShake.roughness) * (forceByDistance);
        c.fadeInTime = minExplosionShake.fadeInTime + (maxExplosionShake.fadeInTime - minExplosionShake.fadeInTime) * (forceByDistance);
        c.fadeOutTime = minExplosionShake.fadeOutTime + (maxExplosionShake.fadeOutTime - minExplosionShake.fadeOutTime) * (forceByDistance);

        GameController.GetGameController().GetPlayer().CameraController.GetCameraShaker().ShakeOnce(c.magnitude, c.roughness, c.fadeInTime, c.fadeOutTime);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
