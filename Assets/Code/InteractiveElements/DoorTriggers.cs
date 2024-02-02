using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggers : MonoBehaviour
{
    [SerializeField] AnimDoor targetDoor;
    [SerializeField] DoorAction action;

    public void DoAction()
    {
        if(action == DoorAction.Open)
        {
            targetDoor.OpenDoor();
        }
        if(action == DoorAction.Close)
        {
            targetDoor.CloseDoor();
        }
    }
    
}
public enum DoorAction { Open, Close }; 
