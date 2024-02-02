using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopDisplayLeft : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text upgradeName;
    [SerializeField] TMP_Text upgradeCost;
    [SerializeField] Image upgradeImage;
    [SerializeField] Image backgroundImage;
    
    public void DisplayUpgrade(Upgrade _upgrade)
    {
        upgradeName.text = _upgrade.UpgradeName;
        upgradeCost.text = _upgrade.upgradeCost.ToString();
        upgradeImage.sprite = _upgrade.upgradeImage;
        if (_upgrade.upgradeBought)
        {
            backgroundImage.color = Color.red;
        }
        else
        {
            backgroundImage.color = Color.white;
        }
    }
}
