using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthSlider : MonoBehaviour
{
    [SerializeField] Image hpBar;
    [SerializeField] Image hpYellowBar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] float smoothnessYellowBar;
    [SerializeField] float smoothnessGreenBar;

    [SerializeField] Animation DamageUI;
    PlayerVariableContainer playerVariableContainer;

    float elapsedTimeHitted;
    [SerializeField] float timeToDownHPBar = 1.0f;
    bool isHealing = false;

    [Header("HealthBar related anims")]
    [SerializeField] Animation HealthBarAnimation;
    [SerializeField] AnimationClip healthBarToGreen;
    [SerializeField] AnimationClip healthBarToNormal;

    [Tooltip("When higher value, it will change faster to blue from the green health bar")]
    [SerializeField][Range(0.001f, 0.1f)] float GoBackBlueBarHealthOnXLasting = 0.4f;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerHit += ResetTimeHitted;
        PlayerHealth.OnPlayerHeal += GotHealing;
    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerHit -= ResetTimeHitted;
        PlayerHealth.OnPlayerHeal -= GotHealing;
    }

    void Start()
    {
        playerVariableContainer = GameController.GetGameController().GetPlayer();
    }

    void Update()
    {
        elapsedTimeHitted += Time.deltaTime;
        float realFillAmountHealth = playerVariableContainer.HealthSystem.GetCurrentHealth() / playerVariableContainer.HealthSystem.GetMaxHealth();
        if (hpBar.fillAmount > realFillAmountHealth) hpBar.fillAmount = realFillAmountHealth;
        else if(hpBar.fillAmount < realFillAmountHealth) hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, realFillAmountHealth, smoothnessGreenBar * Time.deltaTime);
        
        if(hpBar.fillAmount >= (realFillAmountHealth - GoBackBlueBarHealthOnXLasting) && isHealing) //Si no tiene que subir la barra de vida y "sigue curándose", para de curar
        {
            isHealing = false;
            HealthBarAnimation.Stop();
            HealthBarAnimation.Play(healthBarToNormal.name);
        }
        if (hpBar.fillAmount > hpYellowBar.fillAmount)
        {
            hpYellowBar.fillAmount = hpBar.fillAmount;
        }
        else if (elapsedTimeHitted > timeToDownHPBar) hpYellowBar.fillAmount = Mathf.Lerp(hpYellowBar.fillAmount, hpBar.fillAmount, smoothnessYellowBar * Time.deltaTime);

        int health = (int)playerVariableContainer.HealthSystem.GetCurrentHealth();
        healthText.text = health.ToString();
    }
    void ResetTimeHitted()
    {
        elapsedTimeHitted = 0;
        //Set Blue color anim

        DamageUI.Rewind();
        DamageUI.Play();
    }
    void GotHealing()
    {
        isHealing = true;
        HealthBarAnimation.Stop();
        HealthBarAnimation.Play(healthBarToGreen.name);
    }
}
