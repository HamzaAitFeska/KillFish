using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHookUI : MonoBehaviour
{
    PlayerVariableContainer playerVariableContainer;
    [SerializeField] Image HookCrosshair;

    private void Start()
    {
        playerVariableContainer = GameController.GetGameController().GetPlayer(); ;
    }

    void Update()
    {
        HookCrosshair.enabled = playerVariableContainer.MovementController.GrapplingController.IsAbleToShootHook() ? true: false;
    }
}
