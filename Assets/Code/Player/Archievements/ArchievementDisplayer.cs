using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArchievementDisplayer : MonoBehaviour 
{
    [SerializeField]Animation arch_Animation;
    ArchievementData archivedData;
    [SerializeField]TextMeshProUGUI archievementText;

    bool waitingForNewAnimation = false;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (waitingForNewAnimation)
        {
            if (!arch_Animation.isPlaying)
            {
                Archieved(archivedData);
            }
        }
    }
    public void Archieved(ArchievementData data)
    {
        archivedData = data;

        if (arch_Animation.isPlaying)
        {
            waitingForNewAnimation = true;
        }
        else
        {
            waitingForNewAnimation = false;
            archievementText.text = archivedData.Name;
            arch_Animation.Play();
        }
    }


}
