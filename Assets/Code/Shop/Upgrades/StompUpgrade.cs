using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompUpgrade : IUpgrade
{
    // Start is called before the first frame update
    public override void BuyUpgrade()
    {
        base.BuyUpgrade();
        GameController.GetGameController().GetUpgradeManagerData().HasStompAbility = true;
        GameController.GetGameController().GetPlayer().PlayerTutorialScript.StompTutorial();

    }
}
