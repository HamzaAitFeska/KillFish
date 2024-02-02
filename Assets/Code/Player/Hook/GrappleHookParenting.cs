using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookParenting : MonoBehaviour
{
    PlayerVariableContainer player;
    [SerializeField] Transform hookPointChain;
    private void Start()
    {
        player = GameController.GetGameController().GetPlayer();

       transform.SetParent(player.MovementController.GrapplingController.HookshotTransformParent());
        FindObjectOfType<HookLineRenderer>().SetHookPoint(hookPointChain);
        player.MovementController.GrapplingController.SetHookShotTransform(transform);
        player.MovementController.GrapplingController.SetChainObj(hookPointChain.gameObject);
    }
}
