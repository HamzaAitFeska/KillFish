using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(IEnemyHealth))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(IExplode))]
public class IEnemyAI : MonoBehaviour
{
    float currentIdleAtStartTime = 0.0f;
    [SerializeField] float IdleAtStartTime = 1.0f;
    protected PlayerVariableContainer player;
    [SerializeField] protected Animator myAnimator;
    [SerializeField] BoxCollider boxCollider;

    [SerializeField] protected NavMeshAgent NavMeshAgent; //the Enemy Nav Mesh Agent
    protected EnemyStates enemyStates;
    public EnemyStates EnemyState() { return enemyStates; }

    [SerializeField] protected List<Transform> patrolTargets; //Patrol Points
    protected int currentPatrolTargetID = 0;

    [Header("Distances")]
    [SerializeField] protected float distanceToAttackPlayer;
    [SerializeField] protected float distanceToStopAttackingPlayer; //Has to be higher than distanceToAttackPlayer
    [SerializeField] protected float distanceToChasePlayer;
    [SerializeField] protected float normalStoppingDistance = 2.0f;

    [Header("AttackVariables")]
    [SerializeField] protected float attackFrequency; //Tries hit player every (attackFrequency) seconds
    float timeSinceLastAttack;

    [Header("Speeds")]
    [SerializeField] float SpeedOnPatrol;
    [SerializeField] float SpeedOnChase;

    [Header("Jump on stomp related")]
    [SerializeField] float groundDistance = 1;// Adjust the value based on your agent's height and the distance from the ground
    [SerializeField] protected Rigidbody rb;
    [SerializeField] float jumpForce;
    bool isJumping = false;

    [SerializeField] ParticleSystem dieParticles;



    private void OnEnable()
    {
        player = GameController.GetGameController().GetPlayer();
    }

    protected virtual void Update()
    {
        switch (enemyStates)
        {
            case EnemyStates.IDLE:
                UpdateIdleState();
                break;
            case EnemyStates.PATROL:
                UpdatePatrolState();
                break;
            case EnemyStates.CHASE:
                UpdateChaseState();
                break;
            case EnemyStates.ATTACK:
                UpdateAttackState();
                break;
            case EnemyStates.HIT:
                UpdateHitState();
                break;
            case EnemyStates.STOMP:
                UpdateStompState();
                break;
            case EnemyStates.STUN:
                UpdateStunState();
                break;
        }
    }

    protected virtual void SetIdleState()
    {
        enemyStates = EnemyStates.IDLE;
        NavMeshAgent.enabled = true;
    }
    protected virtual void UpdateIdleState()
    {
        currentIdleAtStartTime += Time.deltaTime;
        if(currentIdleAtStartTime > IdleAtStartTime)
        {
            SetPatrolState();
        }
    }
    protected virtual void SetPatrolState()
    {
        enemyStates = EnemyStates.PATROL;
        NavMeshAgent.destination = patrolTargets[currentPatrolTargetID].position;
        NavMeshAgent.speed = SpeedOnPatrol;
        NavMeshAgent.autoBraking = false;
        NavMeshAgent.stoppingDistance = 0;
    }
    protected virtual void UpdatePatrolState()
    {
        if (PatrolTargetPositionArrived()) MoveToRandomPatrolPosition();
        if (InDistanceToChasePlayer()) SetChaseState();
    }
    protected virtual void SetChaseState()
    {
        enemyStates = EnemyStates.CHASE;
        NavMeshAgent.speed = SpeedOnChase;
        NavMeshAgent.autoBraking = true;
        NavMeshAgent.stoppingDistance = normalStoppingDistance;

    }
    protected virtual void UpdateChaseState()
    {

        NavMeshAgent.destination = player.transform.position;
        if (InDistanceToAttackPlayer()) SetAttackState();
    }
    protected virtual void SetAttackState()
    {
        enemyStates = EnemyStates.ATTACK;
        NavMeshAgent.SetDestination(NavMeshAgent.transform.position);
        AttackPlayer();
    }
    protected virtual void UpdateAttackState()
    {
        if (InDistanceToStopAttackPlayer()) SetChaseState();

        timeSinceLastAttack += Time.deltaTime;

        if (timeSinceLastAttack >= attackFrequency)
        {
            AttackPlayer();
        }
    }
    public void SetHitState()
    {
        enemyStates = EnemyStates.HIT;
        NavMeshAgent.SetDestination(NavMeshAgent.transform.position);
        enemyStates = EnemyStates.IDLE;
    }
    protected virtual void UpdateHitState()
    {
    }
    public virtual void SetDieState()
    {
        enemyStates = EnemyStates.DIE;
        if (enemyStates != EnemyStates.STOMP) rb.isKinematic = true;
        NavMeshAgent.enabled = false;
        boxCollider.isTrigger = true;

        dieParticles.transform.SetParent(null);
        dieParticles.Play();

    }

    public virtual void SetStompState()
    {
        Jump();
        enemyStates = EnemyStates.STOMP;
    }
    protected virtual void UpdateStompState()
    {
        if (IsGrounded() && isJumping && rb.velocity.y < 0)
        {
            FinishJump();
        }
    }
    public virtual void SetStunState()
    {
        enemyStates = EnemyStates.STUN;
        NavMeshAgent.enabled = false;
        Debug.Log("Stunned");
    }
    protected virtual void UpdateStunState()
    {

    }
    public virtual void UnsetStunState()
    {
        SetIdleState();
    }
    public void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            NavMeshAgent.enabled = false;
            rb.isKinematic = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundDistance); ;
    }

    private void FinishJump()
    {
        isJumping = false;
        NavMeshAgent.enabled = true;
        rb.isKinematic = true;
        enemyStates = EnemyStates.IDLE;
    }
    protected virtual void AttackPlayer()
    {
        timeSinceLastAttack = 0;
    }
    protected bool PatrolTargetPositionArrived()
    {
        return !NavMeshAgent.hasPath && !NavMeshAgent.pathPending && NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
    }
    protected void MoveToRandomPatrolPosition()
    {
        currentPatrolTargetID = Random.Range(0, patrolTargets.Count);
       NavMeshAgent.destination = patrolTargets[currentPatrolTargetID].position;
    }

    bool InDistanceToChasePlayer()
    {
        Vector3 playerPosition = player.transform.position;
        return Vector3.Distance(playerPosition, transform.position) <= distanceToChasePlayer;
    }
    bool InDistanceToAttackPlayer()
    {
        Vector3 playerPosition = player.transform.position;
        return Vector3.Distance(playerPosition, transform.position) <= distanceToAttackPlayer;
    }
    bool InDistanceToStopAttackPlayer()
    {
        Vector3 playerPosition = player.transform.position;
        return Vector3.Distance(playerPosition, transform.position) >= distanceToStopAttackingPlayer;
    }


    protected Vector3 GetClosestAvailablePoint(Vector3 position)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(position, out hit, Mathf.Infinity, NavMesh.AllAreas);
        return hit.position;
    }
    private void OnDrawGizmos()
    {
        Ray r = new Ray(transform.position, Vector3.down.normalized * groundDistance);
        Gizmos.DrawRay(r);
    }
}

public enum EnemyStates { IDLE, PATROL, CHASE, ATTACK, HIT, DIE, STOMP, STUN }

