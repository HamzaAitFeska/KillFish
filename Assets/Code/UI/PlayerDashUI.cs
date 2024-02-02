using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDashUI : MonoBehaviour
{
    //The father of the dash UI
    [SerializeField] GameObject dashObj;

    //First dash situations
    [SerializeField] Image dashReady;
    [SerializeField] Image dashReadyBar;
    [SerializeField] Image dashNotReadyBar;

    //Only used in double dash situations
    [SerializeField] Image doubleDashRadial;
    [SerializeField] TMP_Text dashCountText;
    void Start()
    {
        //If has dash active
        if (GameController.GetGameController().GetUpgradeManagerData().HasDashAbility) { ActivateDash();}
        else { dashObj.SetActive(false); }

        //If has double dash active
        if (GameController.GetGameController().GetUpgradeManagerData().HasDoubleDashAbility) { ActivateDoubleDash(); }
        else { doubleDashRadial.gameObject.SetActive(false); }

    }
    public void ActivateDash() //Active dash, on start or by activating the ability through orb
    {
        dashObj.SetActive(true);
    }
    public void ActivateDoubleDash()//Active double dash, on start or by activating the ability through shop
    {
        doubleDashRadial.gameObject.SetActive(true);
    }

    void Update()
    {
        //If charging first dash
        if (GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.GetCurrentDashCount() == 0)
        {
            dashNotReadyBar.fillAmount = 1 - (GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.GetDashCurrentCoolDown() / GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.GetDashCoolDown());
            dashReady.enabled = !GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.IsDashInCooldown();
            dashReadyBar.enabled = !GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.IsDashInCooldown();
        }
        //First dash components enabling
        dashReady.enabled = !GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.IsDashInCooldown() ||
                            GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.GetCurrentDashCount() >= 1;
        dashReadyBar.enabled = !GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.IsDashInCooldown() ||
                               GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.GetCurrentDashCount() >= 1;

        if (!GameController.GetGameController().GetUpgradeManagerData().HasDoubleDashAbility) return; //If doesn't have double dash ability, no need on the next code

        //Cooldown on the radial
        if (GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.IsDashInCooldown())
        {
            doubleDashRadial.fillAmount = 1 - 
                (GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.GetDashCurrentCoolDown() /
                GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.GetDashCoolDown());
        }
        else if(doubleDashRadial.fillAmount != 1) { doubleDashRadial.fillAmount = 1; }

        //Current dashes available
        dashCountText.text = GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.GetCurrentDashCount().ToString();

    }
}
