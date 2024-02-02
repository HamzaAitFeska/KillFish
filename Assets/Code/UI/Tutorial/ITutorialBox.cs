using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITutorialBox : MonoBehaviour
{
    public tutorialType TutorialName;
    TutorialScript tutorialScript;

    private void Start()
    {
        tutorialScript = GameController.GetGameController().GetPlayer().PlayerTutorialScript;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.GetComponent<PlayerVariableContainer>() == null)
        {
            return;
        }

        if (TutorialName == tutorialType.TutorialShoot)
        {
            tutorialScript.ShootTutorial();
        }

        if (TutorialName == tutorialType.TutorialHook)
        {
            tutorialScript.HookTutorial();
        }

        if (TutorialName == tutorialType.TutorialSniper)
        {
            tutorialScript.SniperTutorial();
        }
        if (TutorialName == tutorialType.StyleMeter)
        {
            tutorialScript.StyleMeterTutorial();
        }
        if (TutorialName == tutorialType.TutorialDash)
        {
            tutorialScript.DashTutorial();
        }
        Destroy(gameObject);
    }
}

public enum tutorialType
{
    TutorialShoot,
    TutorialHook,
    TutorialSniper, 
    StyleMeter, 
    TutorialDash
}
