using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDashUpgrade : IUpgrade
{
    [SerializeField] Animation HasActivadoDashCanvas;
    public override void BuyUpgrade()
    {
        base.BuyUpgrade();
        GameController.GetGameController().GetUpgradeManagerData().HasDashAbility = true;
        GameController.GetGameController().GetPlayer().PlayersUIVariableContainer.GetPlayerDashUI().ActivateDash();

        HasActivadoDashCanvas.Play();

    }
}

