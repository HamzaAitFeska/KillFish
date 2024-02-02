using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookLineRenderer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    Transform HookPoint;
    public void SetHookPoint(Transform hookPoint) { HookPoint = hookPoint; }
    [SerializeField] Transform hookParent;
    void Update()
    {
        Vector3 hookPositionRespectParent = hookParent.InverseTransformPoint(HookPoint.position);
        lineRenderer.SetPosition(1, hookPositionRespectParent);
    }
}
