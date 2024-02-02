using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUpgrade : MonoBehaviour
{
    public enum TypeUpgrade { Movement, Hook, Damage }

    public TypeUpgrade UpgradeType;

    protected bool isBought;
    public bool IsBought() { return isBought; }
    public virtual void BuyUpgrade()
    {
        isBought = true;
    }
}
