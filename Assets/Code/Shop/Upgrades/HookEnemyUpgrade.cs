using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookEnemyUpgrade : IUpgrade
{
    public override void BuyUpgrade()
    {
        base.BuyUpgrade();
        GameController.GetGameController().GetUpgradeManagerData().HasHookToEnemies = true; // NO quitar. //en el gancho se chekear� si esta variable est� activa y se activar�
        GameController.GetGameController().GetPlayer().MovementController.GrapplingController.AddLayerCanHook(LayerMask.GetMask("EnemieHookableSurface"));


    }
}