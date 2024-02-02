using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugerUpgrades : MonoBehaviour
{
    // Start is called before the first frame update
    LayerMask hookEnemyMask;
    PlayerVariableContainer player;
    void Start()
    {
        player = GameController.GetGameController().GetPlayer();
        hookEnemyMask = LayerMask.GetMask("EnemieHookableSurface");
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H)) GameController.GetGameController().GetRespawnData().RawRespawn(-57.01f, 22.74f, 287.39f);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameController.GetGameController().GetUpgradeManagerData().HasDashAbility = true;
            GameController.GetGameController().GetPlayer().PlayersUIVariableContainer.GetPlayerDashUI().ActivateDash();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameController.GetGameController().GetUpgradeManagerData().HasDoubleDashAbility = true;
            GameController.GetGameController().GetPlayer().PlayersUIVariableContainer.GetPlayerDashUI().ActivateDoubleDash();
            player.MovementController.FPSMoveController.SetCurrentDashCount(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameController.GetGameController().GetUpgradeManagerData().HasDoubleJumpAbility = true;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameController.GetGameController().GetUpgradeManagerData().HasStompAbility = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameController.GetGameController().GetUpgradeManagerData().HasHookToEnemies = true;
            GameController.GetGameController().GetPlayer().MovementController.GrapplingController.AddLayerCanHook(LayerMask.GetMask("EnemieHookableSurface"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GameController.GetGameController().GetUpgradeManagerData().HasMoreShootingAbility = true;
            GameController.GetGameController().GetPlayer().WeaponController.EnableHasMoreShootingFirstShot();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GameController.GetGameController().GetUpgradeManagerData().HasDashInvincibleAbility = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            GameController.GetGameController().GetUpgradeManagerData().HasSniperExplodeAbility = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GameController.GetGameController().GetUpgradeManagerData().HasDoubleDamageAbility = true;
        }
#endif
    }
}
