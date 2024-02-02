using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreShootingUpgrade : IUpgrade
{
    // Start is called before the first frame update
    public override void BuyUpgrade()
    {
        base.BuyUpgrade();
        GameController.GetGameController().GetUpgradeManagerData().HasMoreShootingAbility = true;
        GameController.GetGameController().GetPlayer().WeaponController.EnableHasMoreShootingFirstShot();
    }
}
