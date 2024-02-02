using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject Marco;
    [SerializeField] GameObject Fondo;


    [SerializeField] Animation tutorialAnimation;
    [SerializeField] AnimationClip tutorialEntrance;
    [SerializeField] AnimationClip tutorialExit;

    public InputActionReference hookKey;
    public InputActionReference dashKey;
    public InputActionReference stompKey;
    

    tutorialType lastTutorialType;
    public void ShowAnimation()
    {
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.TutorialAppear, transform.position);
        tutorialAnimation.Play(tutorialEntrance.name);
    }
    public void ExitAnimation()
    {
        tutorialAnimation.Play(tutorialExit.name);
    }
    public void ShootTutorial()
    {
        StartCoroutine(ShootWait());
    }

    public void HookTutorial()
    {
        StartCoroutine(HookPopUp());
    }

    public void SniperTutorial()
    {
        StartCoroutine(SniperWait());
    }

    public void DashTutorial()
    {
        StartCoroutine(DashPopUp());
    }

    public void StompTutorial()
    {
        StartCoroutine(StompPopUp());
    }
    public void StyleMeterTutorial()
    {
        StartCoroutine(StyleMeterPopUp());
    }
    IEnumerator ShootWait()
    {
        Debug.Log(Application.systemLanguage);
        lastTutorialType = tutorialType.TutorialShoot;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                text.text = "Click [LMB] to SHOOT";
                break;
            case SystemLanguage.French:
                text.text = "Cliquez sur [LMB] pour tirer";
                break;
            case SystemLanguage.Spanish:
                text.text = "Haga clic en [LMB] para DISPARAR";
                break;
            case SystemLanguage.ChineseSimplified:
                text.text = "单击 [LMB] 进行拍摄";
                break;
            case SystemLanguage.Russian:
                text.text = "Нажмите [ЛКМ] чтобы СТРЕЛЯТЬ";
                break;
        }
        ShowAnimation();
        yield return new WaitForSeconds(3);
        if(lastTutorialType == tutorialType.TutorialShoot) ExitAnimation();        
    }
    IEnumerator SniperWait()
    {
        lastTutorialType = tutorialType.TutorialSniper;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                text.text = "Hold [RMB] to use SNIPER mode";
                break;
            case SystemLanguage.French:
                text.text = "Maintenez [RMB] pour utiliser le mode SNIPER";
                break;
            case SystemLanguage.Spanish:
                text.text = "Mantenga presionado [RMB] para usar el modo SNIPER";
                break;
            case SystemLanguage.ChineseSimplified:
                text.text = "按住 [RMB] 使用狙击模式";
                break;
            case SystemLanguage.Russian:
                text.text = "Нажмите и удерживайте [ПКМ] чтобы использовать режим СНАЙПЕР";
                break;
        }
        ShowAnimation();
        yield return new WaitForSeconds(4);
        if (lastTutorialType == tutorialType.TutorialSniper) ExitAnimation();        
    }
    IEnumerator HookPopUp()
    {
        lastTutorialType = tutorialType.TutorialHook;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                text.text = "Press " + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(hookKey) + " to use the HOOK. Hook into the GREEN stuff";
                break;
            case SystemLanguage.French:
                text.text = "Presse " + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(hookKey) + " utiliser le CROCHET. Accrochez-vous aux trucs VERTS";
                break;
            case SystemLanguage.Spanish:
                text.text = "Presiona " + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(hookKey) + " para usar el GANCHO. Enganchate a las cosas VERDES";
                break;
            case SystemLanguage.ChineseSimplified:
                text.text = "按 " + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(hookKey) + " 使用 钩. 迷上绿色事物";
                break;
            case SystemLanguage.Russian:
                text.text = "Нажимать " + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(hookKey) + " использовать КРЮК. Подсаживайтесь на ЗЕЛЕНЫЕ вещи";
                break;
        }
        ShowAnimation();
        while (!GameController.GetGameController().GetPlayer().IsMovingWithTheHook())
        {
            yield return null;
        }
        if (lastTutorialType == tutorialType.TutorialHook) ExitAnimation();       
        
    }
    IEnumerator DashPopUp()
    {
        lastTutorialType = tutorialType.TutorialDash;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                text.text = GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(dashKey) + " to use the DASH";
                break;
            case SystemLanguage.French:
                text.text = GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(dashKey) + " utiliser le TIRET";
                break;
            case SystemLanguage.Spanish:
                text.text = GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(dashKey) + " para usar SPRINT";
                break;
            case SystemLanguage.ChineseSimplified:
                text.text = GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(dashKey) + " 使用 短跑";
                break;
            case SystemLanguage.Russian:
                text.text = GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(dashKey) + " использовать СПРИНТ";
                break;
        }
        ShowAnimation();
        yield return new WaitForSeconds(5);
        ExitAnimation();          
    }
    IEnumerator StompPopUp()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                text.text = "Press" + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(stompKey) + " in-air to use the Stomp";
                break;
            case SystemLanguage.French:
                text.text = "Presse" + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(stompKey) + " ndans les airs pour utiliser le Stomp";
                break;
            case SystemLanguage.Spanish:
                text.text = "Presiona" + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(stompKey) + " en el aire para usar el Stomp";
                break;
            case SystemLanguage.ChineseSimplified:
                text.text = "按" + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(stompKey) + " 在空中使用 Stomp";
                break;
            case SystemLanguage.Russian:
                text.text = "Нажимать" + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(stompKey) + " в воздухе чтобы использовать Stomp";
                break;
        }
        
        ShowAnimation(); 
        while (!GameController.GetGameController().GetPlayer().ActionManager.Stomp())
        {
            yield return null;
        }
        ExitAnimation();           
    }

    IEnumerator StyleMeterPopUp()
    {
        lastTutorialType = tutorialType.StyleMeter;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                text.text = "KILL to restore your HEALTH";
                break;
            case SystemLanguage.French:
                text.text = "TUEZ pour restaurer votre SANTE";
                break;
            case SystemLanguage.Spanish:
                text.text = "MATA para restaurar tu SALUD";
                break;
            case SystemLanguage.ChineseSimplified:
                text.text = "杀戮来恢复你的健康";
                break;
            case SystemLanguage.Russian:
                text.text = "УБИВАЙТЕ чтобы восстановить ЗДОРОВЬЕ";
                break;
        }
        ShowAnimation();
        yield return new WaitForSeconds(4);
        ExitAnimation();
        yield return new WaitForSeconds(1);
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                text.text = "KILL the enemies in FLASHY ways to increase your STYLE METER";
                break;
            case SystemLanguage.French:
                text.text = "TUEZ les ennemis de manière FLASHY pour augmenter votre STYLE METER";
                break;
            case SystemLanguage.Spanish:
                text.text = "MATA a los enemigos de formas LLAMATIVAS para aumentar tu MEDIDOR DE ESTILO";
                break;
            case SystemLanguage.ChineseSimplified:
                text.text = "以华丽的方式杀死敌人以增加你的风格计";
                break;
            case SystemLanguage.Russian:
                text.text = "УБИВАЙТЕ врагов ЯРКИМИ способами чтобы увеличить свой ИЗМЕРИТЕЛЬ СТИЛЯ";
                break;

        }
        ShowAnimation();
        yield return new WaitForSeconds(5);
        ExitAnimation();
        yield return new WaitForSeconds(1);
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                text.text = "Increasing your STYLE METER will make you HEAL more";
                break;
            case SystemLanguage.French:
                text.text = "Augmenter votre STYLE METER vous fera GUERIR davantage";
                break;
            case SystemLanguage.Spanish:
                text.text = "Aumentar tu MEDIDOR DE ESTILO te hara SANAR mas";
                break;
            case SystemLanguage.ChineseSimplified:
                text.text = "增加你的风格计会让你更加治愈";
                break;
            case SystemLanguage.Russian:
                text.text = "Увеличение вашего ИЗМЕРИТЕЛЯ СТИЛЯ поможет вам лучше ИСЦЕЛЯТЬСЯ";
                break;
        }
        ShowAnimation();
        yield return new WaitForSeconds(7);
        ExitAnimation();
    }

}
