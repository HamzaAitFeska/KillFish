using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class EnemyOrbSpawn : MonoBehaviour
{
    public bool hasActivatedEnemyWave = false;
    [SerializeField] GameObject pressInteractToStartWave;
    [SerializeField] Animation palancaAnimation;

    [SerializeField] TextMeshProUGUI pressInteractText;
    public InputActionReference interactKey;

    bool canActivateEnemyWave = false;

    [SerializeField] GameObject KillEmAllGO;


    private void Update()
    {
        if(canActivateEnemyWave && !hasActivatedEnemyWave)
        {
            if (GameController.GetGameController().GetPlayer().ActionManager.Interact())
            {
                ActivateOrb();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        if (!hasActivatedEnemyWave)
        {
            pressInteractToStartWave.SetActive(true);
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    pressInteractText.text = "PRESS "+"[" + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(interactKey) + "]";
                    break;
                case SystemLanguage.French:
                    pressInteractText.text = "PRESSE " + "[" + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(interactKey) + "]";
                    break;
                case SystemLanguage.Spanish:
                    pressInteractText.text = "PRESIONA " + "[" + GameController.GetGameController().GetPlayer().ActionManager.GetInputKeyStringForUI(interactKey) + "]";
                    break;
            }
            canActivateEnemyWave = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        pressInteractToStartWave.SetActive(false);
        canActivateEnemyWave = false;
    }

    void ActivateOrb()
    {
        palancaAnimation.Play(palancaAnimation.clip.name);
        hasActivatedEnemyWave = true;
        pressInteractToStartWave.SetActive(false);
        KillEmAllGO.SetActive(true);
        Invoke("StartWave",palancaAnimation.clip.length);
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.Lever, transform.position);
    }
    void StartWave()
    {
        GameController.GetGameController().GetSpawner().StartWaveOrb();
    }
}
