using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem sniperChargeParticles;
    [SerializeField] ParticleSystem sniperChargeParticles2;

    [SerializeField] ParticleSystem sniperShotParticlePrefab; 
    [SerializeField] Transform shootingPointTransform;

    [SerializeField] ParticleSystem shotShotgunParticles;

    [SerializeField] ParticleSystem hookShotSparklesHit;
    [SerializeField] ParticleSystem hookLoopParticles;

    [SerializeField] ParticleSystem movementParticles;

    [SerializeField] ParticleSystem stompParticles;


    [Header("Hit particles Pool")]
    [SerializeField] ParticleSystem shotSparklesHit;
    [SerializeField] int MaxParticles = 30;
    TCObjectPool<ParticleSystem> hitParticlesPool;

    private void Start()
    {
        hitParticlesPool = new TCObjectPool<ParticleSystem>(MaxParticles, shotSparklesHit);
    }
    public void StartMovementParticles()
    {
        movementParticles.Play();
    }
    public void StopMovementParticles()
    {
        movementParticles.Stop();
    }
    public void ShotShotgunParticles()
    {
        shotShotgunParticles.Play();
    }

    public void HookShotParticles()
    {
        hookShotSparklesHit.Play();
    }
    public void StartHookLoopParticles()
    {
        hookLoopParticles.Play();
    }
    public void StopHookLoopParticles()
    {
        hookLoopParticles.Stop();
    }
    public void ShotRifleParticles()
    {
        Quaternion particleFwd = shootingPointTransform.rotation;
        Instantiate(sniperShotParticlePrefab, shootingPointTransform.position, particleFwd);
    }
    public void EnableSniperChargeParticles()
    {
        sniperChargeParticles.Play();
        sniperChargeParticles2.Play();
    }
    public void DisableSniperChargeParticles()
    {
        sniperChargeParticles.Stop();
        sniperChargeParticles2.Stop();
    }

    public void DoStompParticles()
    {
        stompParticles.Play();
    }
    public void ShotSparklesHit(RaycastHit hit)
    {

        Vector3 point = hit.point;
        Vector3 normal = GameController.GetGameController().GetPlayer().transform.position - point;
        Quaternion normalQuat = Quaternion.LookRotation(normal);

        ParticleSystem particle = hitParticlesPool.GetNextElement();
        particle.transform.position = point;
        particle.transform.rotation = normalQuat;
        particle.Play();
    }

}
