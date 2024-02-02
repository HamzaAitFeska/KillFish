using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHookTrigger : MonoBehaviour
{
    [SerializeField] Transform PointsParent;
    IHookPoint[] Points;

    private void Start()
    {
        Points = PointsParent.GetComponentsInChildren<IHookPoint>();
    }

    // Returns the closest point
    public Transform GetNearestPoint(Vector3 position)
    {
        Transform nearestPoint = null;
        float nearestDistance = Mathf.Infinity;

        foreach (IHookPoint point in Points)
        {
            float distance = Vector3.Distance(position, point.transform.position);
            if (distance < nearestDistance)
            {
                nearestPoint = point.transform;
                nearestDistance = distance;
            }
        }

        return nearestPoint;
    }
}
