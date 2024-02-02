using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookEnemyUpgrade : IUpgrade
{
    public override void BuyUpgrade()
    {
        base.BuyUpgrade();
        GameController.GetGameController().GetUpgradeManagerData().HasHookToEnemies = true; // NO quitar. //en el gancho se chekeará si esta variable está activa y se activará
        GameController.GetGameController().GetPlayer().MovementController.GrapplingController.AddLayerCanHook(LayerMask.GetMask("EnemieHookableSurface"));


    }
}