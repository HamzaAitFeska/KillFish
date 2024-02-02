using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text upgradeName;
    [SerializeField] TMP_Text upgradeCost;
    [SerializeField] Image upgradeImage;
    [SerializeField] Image backgroundImage;
    [SerializeField] Button purchasedButton;
    
    public void DisplayUpgrade(Upgrade _upgrade)
    {
        upgradeName.text = _upgrade.UpgradeName;
        upgradeCost.text = _upgrade.upgradeCost.ToString();
        upgradeImage.sprite = _upgrade.upgradeImage;
        purchasedButton.interactable = !_upgrade.upgradeBought;
        if(_upgrade.upgradeBought)
        {
            backgroundImage.color = Color.red;
        }
        else
        {
            backgroundImage.color = Color.white;
        }
    }
}
