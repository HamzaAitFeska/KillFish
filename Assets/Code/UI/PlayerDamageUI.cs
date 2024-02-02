using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageUI : MonoBehaviour
{
    PlayerVariableContainer playerVariableContainer;
    [SerializeField] Image weaponReady;
    [SerializeField] Image weaponReadyBar;
    [SerializeField] Image weaponLoading;
   

    private void Start()
    {
      playerVariableContainer = GameController.GetGameController().GetPlayer();
    }
    void Update()
    {
        weaponLoading.fillAmount = playerVariableContainer.WeaponController.GetTimeSinceLastShotRifle() / playerVariableContainer.WeaponController.GetRifleFrequencyShot();
        weaponReadyBar.enabled = playerVariableContainer.WeaponController.CanStartChargingRifle();
        weaponReady.enabled = playerVariableContainer.WeaponController.CanStartChargingRifle();
    }



}
