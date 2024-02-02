using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRespawnTrigger : MonoBehaviour
{

    public Vector3 customPlayerPosition;

    [Tooltip("Yaw is transform.rotation.y")]
    public float customYaw;

    [Tooltip("Pitch is pitchController gameObject's transform.rotation.x")]
    public float customPitch;

}
