using UnityEngine;
using UnityEngine.AI;

public class GroundedDistanceEnemy : IEnemyAI
{
    [Header("Distance enemy body parts")]
    [SerializeField] GameObject YawBodyPart;
    [SerializeField] GameObject PitchLeftArm;
    [SerializeField] GameObject PitchRightArm;

    [SerializeField] float LerpRotationSpeedTowardsPlayer;

    [Header("Enemy Logics")]
    [SerializeField] float distanceToFleeFromPlayer;
    [SerializeField] float timeBetweenAttacks;
    bool canAttack;

    [Header("Projectiles & Shooting Related")]
    [SerializeField] float projectileSpeed;
    CurrentShootingPoint currentShootingPoint;
    [SerializeField] Transform RightShootingPoint;
    [SerializeField] Transform LeftShootingPoint;
    [SerializeField] ParticleSystem RightShootingParticles;
    [SerializeField] ParticleSystem LeftShootingParticles;

    [Header("ProjectilesPool")]
    [SerializeField] int maxProjectilesPool = 4;
    TCObjectPool<EnemyProjectile> projectilePool;
    [SerializeField] EnemyProjectile projectile;


    private void Start()
    {
        canAttack = true;
        NavMeshAgent.updateRotation = false;
        NavMeshAgent.updatePosition = false;
        projectilePool = new TCObjectPool<EnemyProjectile>(maxProjectilesPool, projectile);

        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.turretSpawn, transform.position);

    }
    protected override void Update()
    {
        base.Update();
        myAnimator.SetFloat("Speed", NavMeshAgent.velocity.magnitude);

        //Sets destination farther from player if too close
       // if (Vector3.Distance(transform.position, player.transform.position) < distanceToFleeFromPlayer && !isAttacking)
       // {
       //     Vector3 dirFromPlayer = transform.position - player.transform.position; //The direction from the player to me (the enemy)
       //
       //     Vector3 newPos = player.transform.position + (dirFromPlayer.normalized * distanceToFleeFromPlayer);
       //
       //     NavMeshAgent.SetDestination(GetClosestAvailablePoint(newPos));
       // }

        //TURRET YAW
        // Calculate the forward direction as the difference between the target's position and the robot's position
        Vector3 forwardDirection = player.transform.position - YawBodyPart.transform.position;
        forwardDirection.y = 0f; // Set the y-component to 0 to ensure rotation only happens on the X axis

        // Calculate the up direction for the robot
        Vector3 upDirection = Vector3.up;

        // Create the desired rotation
        Quaternion YawDesiredRotation = Quaternion.LookRotation(forwardDirection, upDirection);

        // Apply the rotation to the robot
        YawBodyPart.transform.localRotation = YawDesiredRotation;
        Vector3 currentRotation = YawBodyPart.transform.localRotation.eulerAngles;
        Vector3 newRotation = new Vector3(-currentRotation.y + transform.eulerAngles.y, 0f, 0f);
        YawBodyPart.transform.localRotation = Quaternion.Euler(newRotation);

        //TURRET PITCH
        // Calculate the forward direction as the difference between the target's position and the robot's position
        Vector3 forwardDirectionPitch = player.transform.position - YawBodyPart.transform.position;
        // Create the desired rotation
        Quaternion PitchDesiredRotation = Quaternion.LookRotation(forwardDirectionPitch, upDirection);

        // Apply the rotation to the robot
        PitchLeftArm.transform.localRotation = PitchDesiredRotation;
        Vector3 currentRot = PitchLeftArm.transform.localRotation.eulerAngles;
        Vector3 newRot = new Vector3(0, 0, -currentRot.x);
        PitchLeftArm.transform.localRotation = Quaternion.Euler(newRot);
        PitchRightArm.transform.localRotation = Quaternion.Euler(newRot);


        //Agent's rotation
        // if (NavMeshAgent.velocity.magnitude > 0) { 
        //      Vector3 direction = player.transform.position - transform.position;
        //      Quaternion desiredRotation = Quaternion.LookRotation(direction);
        //      transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Lerp(transform.eulerAngles.y, desiredRotation.eulerAngles.y, LerpRotationSpeedTowardsPlayer * Time.deltaTime), 0);
        // }
    }
    protected override void UpdateAttackState()
    {
        base.UpdateAttackState();
    }
    protected override void AttackPlayer()
    {
        base.AttackPlayer();
        if (!canAttack)
        {
            SetChaseState();
            return;
        }
        myAnimator.SetTrigger("Shoot");

        canAttack = false;
        Invoke("CanAttackAgain", timeBetweenAttacks);
    }

    void CanAttackAgain()
    {
        canAttack = true;
    }
    public void StartLeftChargeParticles()
    {
        LeftShootingParticles.Play();
    }
    public void StartRightChargeParticles()
    {
        RightShootingParticles.Play();
    }
    public void StopLeftChargeParticles()
    {
        LeftShootingParticles.Stop();
    }
    public void StopRightChargeParticles()
    {
        RightShootingParticles.Stop();
    }
    public void SetIsAttacking()
    {
        NavMeshAgent.isStopped = true;
    }
    public void SetIsNOTAttacking()
    {
        NavMeshAgent.isStopped = false;
    }

    public void InstantiateLeftProjectile()
    {
        ChangeCurrentShootingPointPos(CurrentShootingPoint.Left);

        InstantiateProjectile();
    }
    public void InstantiateRightProjectile()
    {
        ChangeCurrentShootingPointPos(CurrentShootingPoint.Right);

        InstantiateProjectile();
    }

    void InstantiateProjectile()
    {
        Vector3 shootingPointPos = currentShootingPoint == CurrentShootingPoint.Right ? RightShootingPoint.position : LeftShootingPoint.position;

        EnemyProjectile iProjectile = projectilePool.GetNextElement();
        Transform projectileTransform = iProjectile.transform;
        projectileTransform.position = shootingPointPos;
        projectileTransform.rotation = Quaternion.identity;
        iProjectile.rbody.velocity = (player.transform.position - shootingPointPos).normalized * projectileSpeed;
        iProjectile.trail.Clear();
        iProjectile.gameObject.SetActive(true);
        iProjectile.SetParent(gameObject);

        //Sound
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.turretAttack, transform.position);
    }

    void ChangeCurrentShootingPointPos(CurrentShootingPoint point)
    {
        currentShootingPoint = point;
    }
    protected override void SetChaseState()
    {
        base.SetChaseState();
    }
    protected override void UpdateHitState()
    {
        base.UpdateHitState();
    }
    public override void SetDieState()
    {
        base.SetDieState();

        //Sound
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.turretDeath, transform.position);
        gameObject.SetActive(false);
    }
}

enum CurrentShootingPoint { Left, Right }
