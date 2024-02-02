using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIShop : MonoBehaviour
{
    PlayerVariableContainer player;

    [Header("UI Objects")] //Objects that form the UI of the Shop
    [SerializeField] GameObject inputToOpenShopCanvas;
    [SerializeField] GameObject shopCanvas;
    [SerializeField] GameObject R_Arrow;
    [SerializeField] GameObject L_Arrow;
    [SerializeField] GameObject Left_Preview;
    [SerializeField] GameObject Right_Preview;
    [SerializeField] TMP_Text pointsText;
    [SerializeField] UnityEngine.UI.Button purchasedButton;
    StyleScoreData scoreData;
    [SerializeField] TextMeshProUGUI InteractText; 
    public InputActionReference interactKey;

    bool IsInShopArea = false;

    [SerializeField] ScriptableObject[] scriptableObjectsUpgrade;
    [SerializeField] ShopDisplay shopDisplay;
    [SerializeField] ShopDisplayLeft shopDisplayLeft;
    [SerializeField] ShopDisplayRight shopDisplayRight;
    int currentIndexObject = 0;
    int currentIndexObject_Left = 0;
    int currentIndexObject_Right = 0;

    [Header("Upgrade types for buying")]
    [SerializeField] HookEnemyUpgrade hookUpgrade;
    [SerializeField] MoreDashUpgrade doubleDash;
    [SerializeField] StompUpgrade stompUpgrade;
    [SerializeField] DashInvincibleUpgrade invincibledashUpgrade;
    [SerializeField] MoreShootingUpgrade shootingUpgrade;
    [SerializeField] ExplosiveSniperUpgrade sniperUpgrade;
    [SerializeField] DoubleJumpUpgrade doubleJumpUpgrade;
    [SerializeField] Upgrade scriptableObject1;
    [SerializeField] Upgrade scriptableObject2;

    void Start()
    {
        ChangeUpgrades(0);
        player = GameController.GetGameController().GetPlayer();
        scoreData = player.StyleSystem.GetScoreData();
                
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = scoreData.PlayerTotalPoints().ToString();

        //Quit Shop
        if (shopCanvas.activeSelf && (player.ActionManager.Interact() || player.ActionManager.Escape())) CloseShop();

        if (player.ActionManager.Interact() && IsInShopArea)
        {
            shopCanvas.SetActive(true);
        }

        if(IsInShopArea && !shopCanvas.activeSelf)
        {
            inputToOpenShopCanvas.SetActive(true);
        }

        //Buy
        if (player.ActionManager.Jump() && shopCanvas.activeSelf)
        {
           L_Arrow.GetComponent<UnityEngine.UI.Button>().interactable = false;
           R_Arrow.GetComponent<UnityEngine.UI.Button>().interactable = false;
           BuyUpgrade();
        }
        else
        {
            L_Arrow.GetComponent<UnityEngine.UI.Button>().interactable = true;
            R_Arrow.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        

        //Move in shop
        if (player.ActionManager.LeftShop() && shopCanvas.activeSelf) { ChangeUpgrades(-1); }
        
        if (player.ActionManager.RightShop() && shopCanvas.activeSelf) { ChangeUpgrades(1); }


        if(shopCanvas.activeSelf) { player.SetCanMove(false); player.CameraController.SetFreezeCamera(true); UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true; player.WeaponController.SetCanShoot(false);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inputToOpenShopCanvas.SetActive(true);
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    InteractText.text = "PRESS " + "[" + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(interactKey) + "]" + " TO OPEN THE SHOP";
                    break;
                case SystemLanguage.French:
                    InteractText.text = "PRESSE " + "[" + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(interactKey) + "]" + " POUR OUVRIR LA BOUTIQUE";
                    break;
                case SystemLanguage.Spanish:
                    InteractText.text = "PRESIONA " + "[" + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(interactKey) + "]" + " PARA ABRIR LA TIENDA";
                    break;
            }
            IsInShopArea = true;
            ChangeUpgrades(-scriptableObjectsUpgrade.Length);
            
        }
                
    }

   

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inputToOpenShopCanvas.SetActive(false);
            shopCanvas.SetActive(false);
            IsInShopArea = false;
            player.SetCanMove(true);
            player.WeaponController.SetCanShoot(true);
            player.CameraController.SetFreezeCamera(false);
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UpdateShopUpgrade();
        }
    }

    void UpdateShopUpgrade() 
    {
        Upgrade currentUpgrade = scriptableObjectsUpgrade[currentIndexObject] as Upgrade;
        if (currentUpgrade.upgradeBought)
        {
            int lastUpgradeIndex = scriptableObjectsUpgrade.Length - 1;

            
            for (int i = currentIndexObject; i < scriptableObjectsUpgrade.Length-1; i++)
            {
                scriptableObjectsUpgrade[i] = scriptableObjectsUpgrade[i + 1];
            }
            
            
            scriptableObjectsUpgrade[lastUpgradeIndex] = currentUpgrade;

            
            currentIndexObject = lastUpgradeIndex;
        }

        ChangeUpgrades(-scriptableObjectsUpgrade.Length);
    }

   
    public void BuyUpgrade() //Buys upgrade(enables it's buyupgrade method)
    {
        Upgrade currentUpgrade = scriptableObjectsUpgrade[currentIndexObject] as Upgrade;
        if (currentUpgrade != null)
        {
            if(!currentUpgrade.upgradeBought)
            {
                switch (currentUpgrade.UpgradeName)
                {
                    case "HOOK TO ENEMIES":
                        if(scoreData.PlayerTotalPoints() >= (float)currentUpgrade.upgradeCost)
                        {
                            scoreData.SetMinusPlayerTotalPoints((float)currentUpgrade.upgradeCost);
                            hookUpgrade.BuyUpgrade();
                            currentUpgrade.upgradeBought = true;
                            UpdateShopUpgrade();
                        }
                        break;
                    case "DASH INVINCIBLE":
                        if (scoreData.PlayerTotalPoints() >= (float)currentUpgrade.upgradeCost )
                        {
                            scoreData.SetMinusPlayerTotalPoints((float)currentUpgrade.upgradeCost);
                            invincibledashUpgrade.BuyUpgrade();
                            currentUpgrade.upgradeBought = true;
                            UpdateShopUpgrade();
                        }
                        break;
                    case "DOUBLE DASH":
                        if (scoreData.PlayerTotalPoints() >= (float)currentUpgrade.upgradeCost)
                        {
                            scoreData.SetMinusPlayerTotalPoints((float)currentUpgrade.upgradeCost);
                            doubleDash.BuyUpgrade();
                            currentUpgrade.upgradeBought = true;
                            UpdateShopUpgrade();
                        }
                        break;
                    case "STOMP":
                        if (scoreData.PlayerTotalPoints() >= (float)currentUpgrade.upgradeCost)
                        {
                            scoreData.SetMinusPlayerTotalPoints((float)currentUpgrade.upgradeCost);
                            stompUpgrade.BuyUpgrade();
                            currentUpgrade.upgradeBought = true;
                            UpdateShopUpgrade();
                        }
                        break;
                    case "MORE AMMO":
                        if (scoreData.PlayerTotalPoints() >= (float)currentUpgrade.upgradeCost)
                        {
                            scoreData.SetMinusPlayerTotalPoints((float)currentUpgrade.upgradeCost);
                            shootingUpgrade.BuyUpgrade();
                            currentUpgrade.upgradeBought = true;
                            UpdateShopUpgrade();
                        }
                        break;
                    case "EXPLOSIVE SNIPER":
                        if (scoreData.PlayerTotalPoints() >= (float)currentUpgrade.upgradeCost)
                        {
                            scoreData.SetMinusPlayerTotalPoints((float)currentUpgrade.upgradeCost);
                            sniperUpgrade.BuyUpgrade();
                            currentUpgrade.upgradeBought = true;
                            UpdateShopUpgrade();
                        }
                        break;
                    case "DOUBLE JUMP":
                        if(scoreData.PlayerTotalPoints() >= (float)currentUpgrade.upgradeCost)
                        {
                            scoreData.SetMinusPlayerTotalPoints((float)currentUpgrade.upgradeCost);
                            doubleJumpUpgrade.BuyUpgrade();
                            currentUpgrade.upgradeBought = true;
                            UpdateShopUpgrade();
                        }
                        break;
                }
            }
           
        }
        
    }

    public void CloseShop()
    {
        shopCanvas.SetActive(false);
        player.SetCanMove(true);
        player.WeaponController.SetCanShoot(true);
        player.CameraController.SetFreezeCamera(false);
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    public void ChangeUpgrades(int change)
    {
        currentIndexObject += change;
        if(currentIndexObject < 0) currentIndexObject = 0;
        else if(currentIndexObject > scriptableObjectsUpgrade.Length - 1) currentIndexObject = scriptableObjectsUpgrade.Length - 1;//poner al revés estos valores por si quieren infinita la tienda

        if(shopDisplay != null) shopDisplay.DisplayUpgrade((Upgrade)scriptableObjectsUpgrade[currentIndexObject]);

        if(currentIndexObject == 0)
        {
            L_Arrow.SetActive(false);
            Left_Preview.SetActive(false);
        }
        else { L_Arrow.SetActive(true); Left_Preview.SetActive(true); }

        if (currentIndexObject == scriptableObjectsUpgrade.Length - 1)
        {
            R_Arrow.SetActive(false);
            Right_Preview.SetActive(false);
        }
        else { R_Arrow.SetActive(true); Right_Preview.SetActive(true); }

        ChangeUpgradeRight();
        ChangeUpgradesLeft();

    }

    public void ChangeUpgradesLeft()
    {
        currentIndexObject_Left = currentIndexObject - 1;
        if(currentIndexObject_Left < 0)currentIndexObject_Left = 0;
        if (shopDisplayLeft != null) shopDisplayLeft.DisplayUpgrade((Upgrade)scriptableObjectsUpgrade[currentIndexObject_Left]);
    }

    public void ChangeUpgradeRight()
    {
        currentIndexObject_Right = currentIndexObject + 1;
        if (currentIndexObject_Right > scriptableObjectsUpgrade.Length - 1) currentIndexObject_Right = scriptableObjectsUpgrade.Length - 1;
        if (shopDisplayRight != null) shopDisplayRight.DisplayUpgrade((Upgrade)scriptableObjectsUpgrade[currentIndexObject_Right]);
    }
    //Reseta los Scriptable Objects una vez se ha cerrado el juego o salido de playmode,
    //así se guardan entre escenas los valores
    private void OnApplicationQuit()
    {
        ResetUpgradesValues();
    }

    public void ResetUpgradesValues()
    {
        foreach (Upgrade scriptableObject in Resources.FindObjectsOfTypeAll<Upgrade>())
        {
            if (scriptableObject != scriptableObject1 && scriptableObject != scriptableObject2)
            {
                scriptableObject.Reset();
            }
        }
    }
}
