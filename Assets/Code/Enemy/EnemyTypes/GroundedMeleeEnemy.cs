using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedMeleeEnemy : IEnemyAI
{
    [SerializeField] LayerMask masksToIgnore;
    float damageOnAttack;
    [SerializeField] float inclinationChangeSpeed;
    void Start()
    {
        float damageOnDifficulty;
        DifficultyType difficulty = GameController.GetGameController().GetDifficulty().currentDifficulty;
        if (difficulty == DifficultyType.EASY)
        {
            damageOnDifficulty = GameController.GetGameController().GetDifficulty().DogDamageEasyMode;
        }
        else if (difficulty == DifficultyType.MEDIUM)
        {
            damageOnDifficulty = GameController.GetGameController().GetDifficulty().DogDamageMediumMode;
        }
        else
        {
            damageOnDifficulty = GameController.GetGameController().GetDifficulty().DogDamageHardMode;
        }

        damageOnAttack = damageOnDifficulty;

        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.dogSpawn, transform.position);
    }
    protected override void Update()
    {
        base.Update();
        float speed = Mathf.Clamp(NavMeshAgent.velocity.magnitude / 5, 0, 1); //5 Value to change => A proper value would be the maximum speed of an agent, which seems to be SpeedOnChase Variable
        myAnimator.SetFloat("Speed", speed);
        //Lanzar un rayo hacia abajo desde la posición del modelo
        //if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 5, ~masksToIgnore))
        //{
        //    // Obtener la normal del suelo en el punto de colisión
        //    Vector3 groundNormal = hit.normal;
        //
        //    // Calcular la rotación necesaria para alinearse con la normal del suelo
        //    Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal);
        //
        //    // Cambiar solo el eje X de la rotación local
        //    Vector3 currentEulerAngles = transform.localRotation.eulerAngles;
        //    Vector3 targetEulerAngles = targetRotation.eulerAngles;
        //    Vector3 newEulerAngles = new Vector3(targetEulerAngles.x, currentEulerAngles.y, currentEulerAngles.z);
        //    transform.localRotation = Quaternion.Euler(newEulerAngles);
        //}
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 5, ~masksToIgnore))
        {
            Quaternion newRot = Quaternion.LookRotation(Vector3.Cross(transform.TransformDirection(Vector3.right), hit.normal));
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, inclinationChangeSpeed * Time.deltaTime);
        }

    }
    protected override void UpdateHitState()
    {
        base.UpdateHitState();
    }
    protected override void SetChaseState()
    {
        base.SetChaseState();
        rb.isKinematic = true;
        NavMeshAgent.enabled = true;
    }
    protected override void AttackPlayer()
    {
        base.AttackPlayer();
        myAnimator.SetTrigger("Attack");
        //Sound
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.dogAttack, transform.position);
    }
    public override void SetDieState()
    {
        base.SetDieState();
        //Sound
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.dogDeath, transform.position);
        gameObject.SetActive(false);
    }

    public void DealDamage()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > distanceToStopAttackingPlayer) return;
        player.HealthSystem.TakeDamage(damageOnAttack);
        if (!player.HealthSystem.IsInvencible()) PlayerDamageIndicatorUI.CreateIndicator(transform);
    }
    protected override void UpdateAttackState()
    {
        base.UpdateAttackState();

        transform.LookAt(player.transform.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    void PlayFootstep()
    {
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.dogRun, transform.position);
    }
}
