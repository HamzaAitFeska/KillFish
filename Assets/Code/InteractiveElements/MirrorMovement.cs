using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform mirror;
    [SerializeField] Transform player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localplayer = mirror.InverseTransformPoint(player.transform.position);
        transform.position = mirror.TransformPoint(new Vector3(localplayer.x,localplayer.y, -localplayer.z));

        Vector3 lookatmirror = mirror.TransformPoint(new Vector3(-localplayer.x, localplayer.y, localplayer.z));
        transform.LookAt(lookatmirror);
    }
}
