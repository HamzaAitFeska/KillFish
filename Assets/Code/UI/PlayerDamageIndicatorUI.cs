using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDamageIndicatorUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DamageIndicatorUnit indicatorPrefab = null;
    [SerializeField] private RectTransform holder;
    [SerializeField] private new Camera camera = null;
    [SerializeField] private Transform player = null;

    private Dictionary<Transform, DamageIndicatorUnit> Indicators = new Dictionary<Transform, DamageIndicatorUnit>();

    #region Delegates
    public static Action<Transform> CreateIndicator = delegate { };
    public static Func<Transform, bool> CheckIfObjectInSight = null;
    #endregion

    private void OnEnable()
    {
        CreateIndicator += Create;
        CheckIfObjectInSight += InSight;
    }
    private void OnDisable()
    {
        CreateIndicator -= Create;
        CheckIfObjectInSight -= InSight;
    }

    void Create(Transform target)
    {
        if (Indicators.ContainsKey(target))
        {
            Indicators[target].Restart();
            return;
        }

        DamageIndicatorUnit newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Register(target, player, new Action(() => { Indicators.Remove(target); }));

        Indicators.Add(target, newIndicator);
    }

    bool InSight(Transform t)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(t.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.y > 0 && screenPoint.x < 1 && screenPoint.y < 1;
    }
}
