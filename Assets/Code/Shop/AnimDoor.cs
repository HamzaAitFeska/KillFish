using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimDoor : MonoBehaviour
{
    [SerializeField] Animation doorAnimation;
    [SerializeField] AnimationClip openClip;
    [SerializeField] AnimationClip closeClip;
    [SerializeField] AnimationClip lockedClip;
    [SerializeField] AnimationClip UnlockedClip;

    private void Start()
    {
        openClip = Resources.Load<AnimationClip>("AnimationClips/Environment/Door/DoorCapsule_1_Opening");
        closeClip = Resources.Load<AnimationClip>("AnimationClips/Environment/Door/DoorCapsule_1_Closing");
        lockedClip = Resources.Load<AnimationClip>("AnimationClips/Environment/Door/DoorCapsule_1_Locked");
        UnlockedClip = Resources.Load<AnimationClip>("AnimationClips/Environment/Door/DoorCapsule_1_Unlocked");
    }
    public void CanOpenDoor()
    {
        doorAnimation.Play(UnlockedClip.name);
    }
    public void CantOpenDoor()
    {
        doorAnimation.Play(lockedClip.name);
    }
    public void OpenDoor()
    {
        doorAnimation.Play(openClip.name); 
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.DoorOpen, transform.position);
    }
    public void CloseDoor()
    {
        doorAnimation.Play(closeClip.name);
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.DoorClose, transform.position);
        Invoke("CantOpenDoor", closeClip.length);
    }
}
