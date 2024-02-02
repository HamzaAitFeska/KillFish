using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using FMOD.Studio;

public class WeaponController : MonoBehaviour
{
    PlayerVariableContainer playerVariableContainer;
    [SerializeField] bool damageMultiplicatorActive;
    //Aiming variables
    bool isAiming;
    public bool IsAiming() { return isAiming; }

    bool wasAiming;

    [Header("Rifle Related")]
    [SerializeField] float minimumRifleDamage;
    [SerializeField] float maximumRifleDamage;
    [SerializeField] float timeToFullCharge;

    float rifleDamage;
    float elapsedTimeCharging;
    bool ShouldStartSniperMode() { return !shootingAnimation && isAiming && !wasAiming && CanStartChargingRifle(); }
    bool isInSniperMode = false;
    public bool IsInSniperMode() { return isInSniperMode; }
    //Was full charged
    bool wasLastShotFullCharged = false;
    public bool WasLastShotFullCharged() { return wasLastShotFullCharged; }

    //Frequency of shots rifle
    [SerializeField] float shotFrequencyRifle;
    public float GetRifleFrequencyShot() { return shotFrequencyRifle; }
    float timeSinceLastShotRifle;
    public float GetTimeSinceLastShotRifle() { return timeSinceLastShotRifle; }

    public bool CanStartChargingRifle() { return timeSinceLastShotRifle >= shotFrequencyRifle; }

    bool shootingAnimation = false;
    public bool ShootingAnimation { get { return shootingAnimation; } set { shootingAnimation = value; } }

    bool transformingWeapon = false;
    public bool TransformingWeapon { get { return transformingWeapon; } set { transformingWeapon = value; } }

    [Header("Shotgun Related")]
    [SerializeField] float minimumShotgunDamage;
    [SerializeField] float maximumShotgunDamage;
    [SerializeField] float maximumShotgunDistanceToFullDamage;
    [SerializeField] float maximumShotgunDistanceToDamage;
    float shotgunDamage;
    bool shotWithoutCharge = false;
    public void EnableHasMoreShootingFirstShot() { shotWithoutCharge = true; }

    float avoidJitterShotgunTime = 0.2f;
    bool canShotgunAvoidingJitter = true;
    //Variables para hacer el efecto cono
    public float radius = 5f;

    //Numero de rayos
    public int numberProjectiles = 30;


   //Last shot type
   public enum ShotType{ SHOTGUN_SHOT, RIFLE_SHOT }

    ShotType lastShot;
    public ShotType LastShot() { return lastShot; }

    //CAN SHOOT
    bool canShoot = true;
    public void SetCanShoot(bool value) { canShoot = value; }
    public bool CanShoot(){ return canShoot; }




    [SerializeField] LayerMask MasksToIgnore;


    bool lastShotIsHeadshot = false;
    public bool IsLastShotAHeadshot() { return lastShotIsHeadshot; }

    bool lastShotIsCollateral = false;
    public bool IsLastShotACollateral() { return lastShotIsCollateral; }

    //Collateral
    [SerializeField] float collateralOffset = 0; //0 seems to work jeje


    //SOUND
    EventInstance chargingSniper;

    //for archievements
    bool usedShotgun = false;
    bool usedSniper = false;


    Transform CameraPlayer;
    private void Start()
    {
        playerVariableContainer = GameController.GetGameController().GetPlayer();

        timeSinceLastShotRifle = shotFrequencyRifle;
        shotWithoutCharge = GameController.GetGameController().GetUpgradeManagerData().HasMoreShootingAbility;

        chargingSniper = AudioManager.GetAudioManager().CreateEventInstance(AudioManager.GetAudioManager().Events.chargeSniper);
        CameraPlayer = GameController.GetGameController().GetPlayer().CameraController.playerCamera.transform;

    }
    public void ShootingUpdate()
    {
        if (Time.timeScale == 0.0f) return;
        //Recharge Shotgun related
        timeSinceLastShotRifle += Time.deltaTime;

        if(timeSinceLastShotRifle >= shotFrequencyRifle) { timeSinceLastShotRifle = shotFrequencyRifle; }
        if (shootingAnimation) return;

        //Checks if is pressing for aiming
        isAiming = playerVariableContainer.ActionManager.Fire2() && CanStartChargingRifle() && !playerVariableContainer.MovementController.GrapplingController.IsUsingHook();
        if(ShouldStartSniperMode())
        {
            isInSniperMode = true;
            chargingSniper.start();
            playerVariableContainer.PlayerAnimationSystem.StartsSniperMode();
            playerVariableContainer.PlayerAnimationSystem.TransformingWeaponProcess();
        }
        else if(!isAiming && wasAiming)
        {
            isInSniperMode = false;
            chargingSniper.stop(STOP_MODE.ALLOWFADEOUT);
            playerVariableContainer.PlayerAnimationSystem.StopsSniperMode();
        } 


        //In case of aiming, it power's up the weapon
        if (isAiming && CanStartChargingRifle())
        {
            elapsedTimeCharging += Time.deltaTime;
        }
        else if(elapsedTimeCharging > 0)
        {
            elapsedTimeCharging = 0;
        }

        //If shoots do logics
        if (CanShoot())
        {
            if (playerVariableContainer.ActionManager.Fire1())
            {
                if (isAiming)
                {
                    if(timeSinceLastShotRifle >= shotFrequencyRifle && !transformingWeapon) FireRifle();
                }
                else 
                {
                    if(canShotgunAvoidingJitter) FireShotgun();
                }

                
            }
        }
        wasAiming = isAiming;
    }

    void CanShootRifleAgain()
    {
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.sniperRecharge, transform.position);

    }
    void FireRifle()
    {
        //Set type of shot
        lastShot = ShotType.RIFLE_SHOT;
        usedSniper = true;

        //Set animation, stop sound
        isInSniperMode = false;
        playerVariableContainer.PlayerAnimationSystem.StopsSniperMode();
        playerVariableContainer.PlayerAnimationSystem.StoppedTransformationProcess();
        chargingSniper.stop(STOP_MODE.ALLOWFADEOUT);
        
        //Set recoil and shake

        playerVariableContainer.CameraController.GetCameraShootRecoilShake().RifleRecoil = playerVariableContainer.CameraController.GetCameraShootRecoilShake().MinRifleRecoil //Calculate the recoil
            + (playerVariableContainer.CameraController.GetCameraShootRecoilShake().MaxRifleRecoil - playerVariableContainer.CameraController.GetCameraShootRecoilShake().MinRifleRecoil) 
            * (elapsedTimeCharging / timeToFullCharge);

        playerVariableContainer.CameraController.GetCameraShootRecoilShake().RecoilFire(playerVariableContainer.CameraController.GetCameraShootRecoilShake().RifleRecoil);//Set the recoil

        ShootRifleShake(); // Set Rifle Shake

        //Particles and sounds
        playerVariableContainer.PlayerAnimationSystem.ShootRifle();
        playerVariableContainer.PlayerParticlesSystem.ShotRifleParticles();
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.rifleShot, transform.position);

        //Set parameters 
        timeSinceLastShotRifle = 0;
        Invoke("CanShootRifleAgain", shotFrequencyRifle);
        rifleDamage = minimumRifleDamage + (maximumRifleDamage - minimumRifleDamage) * (elapsedTimeCharging / timeToFullCharge); //Set Rifle damage depending
        rifleDamage = Mathf.Clamp(rifleDamage, minimumRifleDamage, maximumRifleDamage); //In case elapsedTimeCharging is higher than timeToFullCharge

        wasLastShotFullCharged = false;
        if(elapsedTimeCharging >= timeToFullCharge) { wasLastShotFullCharged = true; } // If its full charged 
        elapsedTimeCharging = 0; //Reset time elapsed charging to 0

        //Logics
        RaycastHit hit;
        Debug.DrawRay(CameraPlayer.position, CameraPlayer.forward * maximumShotgunDistanceToDamage, Color.red,0.5f);
        if (Physics.Raycast(CameraPlayer.position, CameraPlayer.forward, out hit, Mathf.Infinity, ~MasksToIgnore))
        {
            TryDealDamage(hit, rifleDamage);
        }
    }
    public void FireShotgun()
    {
        //Set type of shot
        lastShot = ShotType.SHOTGUN_SHOT;
        usedShotgun = true;

        //Avoid exceptions of jitter clicking
        canShotgunAvoidingJitter = false;
        Invoke("CanShotgunAgain", avoidJitterShotgunTime);

        //Stop Hooking to shot        
        if (playerVariableContainer.MovementController.GrapplingController.IsUsingHook())
        {
            playerVariableContainer.MovementController.GrapplingController.HookStoppedByShooting();
        }

        //Set animation
        if (GameController.GetGameController().GetUpgradeManagerData().HasMoreShootingAbility && shotWithoutCharge)
        {
            playerVariableContainer.PlayerAnimationSystem.ShootShotgun();
            //SOUND ONLY SHOT
            AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.shotgunShot, transform.position);
            shotWithoutCharge = false;
        }
        else
        {
            playerVariableContainer.PlayerAnimationSystem.ShootRechargeShotgun();
            //SOUND SHOT RECHARGE
            AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.shotgunShotRecharge, transform.position);
            if (GameController.GetGameController().GetUpgradeManagerData().HasMoreShootingAbility) shotWithoutCharge = true;
        }

        //Set recoil and shake
        playerVariableContainer.CameraController.GetCameraShootRecoilShake().RecoilFire(playerVariableContainer.CameraController.GetCameraShootRecoilShake().ShotgunRecoil);
        // ShakeShoot(playerVariableContainer.CameraController.GetCameraShootRecoilShake().ShotgunCameraShake); //NOT USED

        //Particles and sounds
        playerVariableContainer.PlayerParticlesSystem.ShotShotgunParticles();

        //Logics
        for (int i = 0; i < numberProjectiles; i++)
        {
            //Get knockback Shotgun momentum
            if (playerVariableContainer.CameraController.IsLookingDown()) playerVariableContainer.MovementController.FPSMoveController.StartShotgunnedDown(playerVariableContainer.CameraController.playerCamera.transform.forward);

            // Calculate a random direction within a cone centered on the camera's forward vector
            Vector3 direction = playerVariableContainer.CameraController.playerCamera.transform.forward;
            direction += Random.insideUnitSphere * radius / 100f;
            direction.Normalize();

            // If the raycast hits something, apply damage to it
            RaycastHit hit;
            Debug.DrawRay(CameraPlayer.position, direction* maximumShotgunDistanceToDamage, Color.magenta);
            if (Physics.Raycast(CameraPlayer.position, direction, out hit, maximumShotgunDistanceToDamage, ~MasksToIgnore))
            {
                if (Vector3.Distance(playerVariableContainer.CameraController.playerCamera.transform.position, hit.point) < maximumShotgunDistanceToFullDamage) shotgunDamage = maximumShotgunDamage;
                else shotgunDamage = minimumShotgunDamage + ((maximumShotgunDamage - minimumShotgunDamage) * (1f - Vector3.Distance(playerVariableContainer.CameraController.playerCamera.transform.position, hit.point) / maximumShotgunDistanceToDamage));                

                if (playerVariableContainer.HealthSystem.IsFullHealth() && damageMultiplicatorActive)
                {
                    shotgunDamage *= playerVariableContainer.StyleSystem.GetCurrentStyle().Health_Damage;
                }
                TryDealDamage(hit, shotgunDamage);
            }
        }
    }
    void CanShotgunAgain()
    {
        canShotgunAvoidingJitter = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * maximumShotgunDistanceToFullDamage);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + transform.forward * maximumShotgunDistanceToFullDamage, transform.forward * maximumShotgunDistanceToDamage);
    }

    void TryDealDamage(RaycastHit hit ,float projectileDamage, bool collateral = false)
    {
        //Instantiate sparkles hit
        playerVariableContainer.PlayerParticlesSystem.ShotSparklesHit(hit);

        EnemyHead enemyHead = hit.collider.gameObject.GetComponent<EnemyHead>();
        IEnemyHealth enemyHealth = hit.collider.gameObject.GetComponent<IEnemyHealth>();
        ExplosiveBarrels barrel = hit.collider.gameObject.GetComponent<ExplosiveBarrels>();
        if (enemyHead != null)
        {
            lastShotIsHeadshot = true;
           enemyHead.TakeDamage(projectileDamage, false, collateral);

            if (GameController.GetGameController().GetUpgradeManagerData().HasSniperExplodeAbility)
            {
                if (LastShot() == WeaponController.ShotType.RIFLE_SHOT)
                {
                    enemyHead.explosiveComponent.Explode();
                }
            }
            TryDealColateralDamage(hit.point);

        }
        else if(enemyHealth != null)
        {
            lastShotIsHeadshot = false;
            enemyHealth.TakeDamage(projectileDamage, false, collateral);
            if (GameController.GetGameController().GetUpgradeManagerData().HasSniperExplodeAbility)
            {
                if (LastShot() == WeaponController.ShotType.RIFLE_SHOT)
                {
                    enemyHealth.explosiveComponent.Explode();
                }
            }
            TryDealColateralDamage(hit.point);
        }
        else if (barrel != null)
        {
            barrel.Explode();
        }
    }

    void TryDealColateralDamage(Vector3 hitPos)
    {
        if(LastShot() == ShotType.RIFLE_SHOT)
        {
            lastShotIsCollateral = false;
            RaycastHit hit;
            if (Physics.Raycast(hitPos + collateralOffset * playerVariableContainer.CameraController.playerCamera.transform.forward, 
                playerVariableContainer.CameraController.playerCamera.transform.forward, out hit, Mathf.Infinity, ~MasksToIgnore))
            {
                lastShotIsCollateral = true;
                TryDealDamage(hit, rifleDamage, true);
            }
        }
    }
    
    void ShakeShoot(CameraShakeInstanceVariables var)
    {
        playerVariableContainer.CameraController.GetCameraShaker().ShakeOnce(var.magnitude, var.roughness, var.fadeInTime, var.fadeOutTime);
    }

    CameraShakeInstanceVariables CalculateRifleShake()
    {
        CameraShakeInstanceVariables minshake = playerVariableContainer.CameraController.GetCameraShootRecoilShake().MinRifleShake;
        CameraShakeInstanceVariables maxshake = playerVariableContainer.CameraController.GetCameraShootRecoilShake().MaxRifleShake;

        CameraShakeInstanceVariables c = new CameraShakeInstanceVariables();
        c.magnitude = minshake.magnitude + (maxshake.magnitude - minshake.magnitude) * (elapsedTimeCharging / timeToFullCharge);
        c.roughness = minshake.roughness + (maxshake.roughness - minshake.roughness) * (elapsedTimeCharging / timeToFullCharge);
        c.fadeInTime = minshake.fadeInTime + (maxshake.fadeInTime - minshake.fadeInTime) * (elapsedTimeCharging / timeToFullCharge);
        c.fadeOutTime = minshake.fadeOutTime + (maxshake.fadeOutTime - minshake.fadeOutTime) * (elapsedTimeCharging / timeToFullCharge);

        return c;
    }
    void ShootRifleShake()
    {
        ShakeShoot(CalculateRifleShake());
    }

}