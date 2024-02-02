using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //Health related variables
    float health;
    const float maxHealth = 100;
    public float GetMaxHealth() { return maxHealth; }
    public float GetCurrentHealth() { return health; }
    public bool IsFullHealth(){ return health == maxHealth;}

    //Invencibility variables
    bool invencibility = false;
    public bool IsInvencible() { return invencibility; }
    public void StartInvencibilityTimer(float time)
    {
        StartCoroutine(InvencibilityTimed(time));
    }
    IEnumerator InvencibilityTimed(float time)
    {
        StartInvencibility();
        yield return new WaitForSeconds(time);
        StopInvencibility();
    }
    public void StartInvencibility(){ invencibility = true;}
    public void StopInvencibility(){ invencibility = false;}

    [Tooltip("The time player is invencible after a dash")]
    [SerializeField] float dashTimeInvencible;
    public float GetDashTimeInvencible() { return dashTimeInvencible; }

    //ACTIONS
    public static Action OnPlayerHit;
    public static Action OnPlayerHeal;
    public static Action OnPlayerDie;
    void Start()
    {
        health = maxHealth;
    }

    private void OnEnable()
    {
        IEnemyHealth.OnEnemyDeath += EnemyKilled;
        EnemySpawner.OnWaveStarted += HealAll;
        EnemySpawner.OnWaveFinished += HealAll;
    }
    private void OnDisable()
    {
        IEnemyHealth.OnEnemyDeath -= EnemyKilled;
        EnemySpawner.OnWaveStarted -= HealAll;
        EnemySpawner.OnWaveFinished -= HealAll;
    }
    public void TakeDamage(float amount)
    {
        if (invencibility || health <= 0 || !GameController.GetGameController().GetPlayer().CanMove()) return; //Can't take away player's health

        health -= amount;
        OnPlayerHit?.Invoke();

        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.playerHit, transform.position);

        if (health <= 0){ Die(); }
    }
    void HealAll(int wave)
    {
        health = maxHealth;
        OnPlayerHeal?.Invoke();
    }
    public void HealPlayer(float amount)
    {
        if (IsFullHealth()) return;

        health += amount;
        OnPlayerHeal?.Invoke();
        if (health > maxHealth) health = maxHealth;
    }

    void EnemyKilled(IEnemyHealth enemy)
    {
        HealPlayer(enemy.HealPlayerOnDeath * GameController.GetGameController().GetPlayer().StyleSystem.GetCurrentStyle().Health_Damage); //Heal depends on the enemy killed and the current style health's multiplier
    }

    private void Die()
    {
        health = 0;

        OnPlayerDie?.Invoke();

        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.playerDie, transform.position);
        
        //Reset
        GameController.GetGameController().GetPlayer().SetCanMove(false);
        GameController.GetGameController().ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnTriggerEnter(Collider other)
    {
        //For getting respawn information
        if (other.tag != "Respawn") return;

        IRespawnTrigger respawnData = other.GetComponent<IRespawnTrigger>();
        GameController.GetGameController().GetRespawnData().SetRespawnData(respawnData.customPlayerPosition, respawnData.customYaw, respawnData.customPitch);
    }
}
