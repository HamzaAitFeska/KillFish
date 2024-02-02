using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Data/UpgradeManager"), System.Serializable]
public class UpgradeManager : ScriptableObject
{
    [SerializeField] bool hasDashAbility;
    public bool HasDashAbility { get { return hasDashAbility; } set { hasDashAbility = value; } }

    [SerializeField] bool hasSniperExplodeAbility;
    public bool HasSniperExplodeAbility { get { return hasSniperExplodeAbility; } set { hasSniperExplodeAbility = value; } }

    [SerializeField] bool hasDashInvincibleAbility;
    public bool HasDashInvincibleAbility { get { return hasDashInvincibleAbility; } set { hasDashInvincibleAbility = value; } }

    [SerializeField] bool hasMoreShootingAbility;
    public bool HasMoreShootingAbility { get { return hasMoreShootingAbility; } set { hasMoreShootingAbility = value; } }

    [SerializeField] bool hasStompAbility;
    public bool HasStompAbility { get { return hasStompAbility; } set { hasStompAbility = value; } }

    [SerializeField] bool hasDoubleJumpAbility;
    public bool HasDoubleJumpAbility { get { return hasDoubleJumpAbility; } set { hasDoubleJumpAbility = value; } }
    [SerializeField] bool hasDoubleDamageAbility;
    public bool HasDoubleDamageAbility { get { return hasDoubleDamageAbility; } set { hasDoubleDamageAbility = value; } }

    [SerializeField] bool hasHookToEnemies;
    public bool HasHookToEnemies { get { return hasHookToEnemies; } set { hasHookToEnemies = value; } }

    [SerializeField] bool hasDoubleDashAbility;
    public bool HasDoubleDashAbility { get { return hasDoubleDashAbility; } set { hasDoubleDashAbility = value; } }
    public void ResetValues() //Called on GameController
    {
        hasDashAbility = false;
        HasDoubleJumpAbility = false;
        hasDoubleDamageAbility = false;
        hasHookToEnemies = false;
        HasDoubleDashAbility = false;
        HasStompAbility = false;
        HasMoreShootingAbility = false;
        HasDashInvincibleAbility = false;
        HasSniperExplodeAbility = false;
    }

}

