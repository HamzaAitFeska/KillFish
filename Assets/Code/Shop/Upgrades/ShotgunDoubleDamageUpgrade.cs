using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunDoubleDamageUpgrade : IUpgrade
{
    public override void BuyUpgrade()
    {
        base.BuyUpgrade();
        GameController.GetGameController().GetUpgradeManagerData().HasDoubleDamageAbility = true;
    }

}
