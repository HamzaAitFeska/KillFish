using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIVariableContainer : MonoBehaviour
{
    [SerializeField] PlayerDamageIndicatorUI playerDamageIndicatorUI;
    public PlayerDamageIndicatorUI GetPlayerDamageIndicatorUI() { return playerDamageIndicatorUI; }
    [SerializeField] PlayerDamageUI playerDamageUI;
    public PlayerDamageUI GetPlayerDamageUI() { return playerDamageUI; }
    [SerializeField] PlayerDashUI playerDashUI;
    public PlayerDashUI GetPlayerDashUI() { return playerDashUI; }  
    [SerializeField] PlayerHookUI playerHookUI;
    public PlayerHookUI GetPlayerHookUI() { return playerHookUI; }
    [SerializeField] HealthSlider playerHealthSlider;
    public HealthSlider GetPlayerHealthSlider() { return playerHealthSlider; }
    [SerializeField] StyleMeterCanvas playerStyleMeterCanvas;
    public StyleMeterCanvas GetPlayerStyleMeterCanvas() { return playerStyleMeterCanvas; }
}
