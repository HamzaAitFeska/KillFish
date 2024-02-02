using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Data/Upgrades"), System.Serializable]
public class Upgrade : ScriptableObject
{
    // Start is called before the first frame update
    public string UpgradeName;
    public bool upgradeBought;
    public int upgradeCost;
    public Sprite upgradeImage;
    
    public virtual void BuyUpgrade()
    {
        upgradeBought = true;
    }

    public void Reset()
    {
        upgradeBought = false;
    }

}
