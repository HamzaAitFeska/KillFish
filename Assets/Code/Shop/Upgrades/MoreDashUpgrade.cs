using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreDashUpgrade : IUpgrade
{
    public override void BuyUpgrade()
    {
        base.BuyUpgrade();
        GameController.GetGameController().GetUpgradeManagerData().HasDoubleDashAbility = true;
        GameController.GetGameController().GetPlayer().PlayersUIVariableContainer.GetPlayerDashUI().ActivateDoubleDash();
        GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.SetCurrentDashCount(2);
    }
}
