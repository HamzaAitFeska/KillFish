using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageIndicatorUnit : MonoBehaviour
{
    const float maxTimer = 1.0f;
    float currentTimer = maxTimer;

    private CanvasGroup canvasGroup = null;
    protected CanvasGroup CanvasGroup
    {
        get
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup;
        }
    }

    private RectTransform rect = null;
    protected RectTransform Rect
    {
        get
        {
            if (rect == null)
            {
                rect = GetComponent<RectTransform>();
                if (rect == null)
                {
                    rect = gameObject.AddComponent<RectTransform>();
                }
            }
            return rect;
        }
    }

    public Transform target { get; protected set; } = null;
    private Transform player = null;

    private IEnumerator IE_Countdown = null;
    private Action unRegister = null;

    private Quaternion tRot = Quaternion.identity;
    private Vector3 tPos = Vector3.zero;

    public void Register(Transform t, Transform p, Action unRegister)
    {
        this.target = t;
        this.player = p;
        this.unRegister = unRegister;

        StartCoroutine(RotateToTheTarget());
        StartTimer();
    }
    void StartTimer()
    {
        if (IE_Countdown != null) { StopCoroutine(IE_Countdown); }

        IE_Countdown = Countdown();

        StartCoroutine(IE_Countdown);
    }
    IEnumerator RotateToTheTarget()
    {
        while (enabled)
        {
            if (target)
            {
                tPos = target.position;
                tRot = target.rotation;
            }
            Vector3 direction = player.position - tPos;

            tRot = Quaternion.LookRotation(direction);

            tRot.z = -tRot.y;
            tRot.x = 0;
            tRot.y = 0;

            Vector3 northDirection = new Vector3(0, 0, player.eulerAngles.y);
            Rect.localRotation = tRot * Quaternion.Euler(northDirection);

            yield return null;
        }
    }

    IEnumerator Countdown()
    {
        while (CanvasGroup.alpha < 1.0f)
        {
            CanvasGroup.alpha += 4.0f * Time.deltaTime;
            yield return null;
        }
        while (currentTimer > 0)
        {
            currentTimer--;
            yield return new WaitForSeconds(1);
        }
        while (CanvasGroup.alpha > 0.0f)
        {
            CanvasGroup.alpha -= 2 * Time.deltaTime;
            yield return null;
        }
        unRegister();
        Destroy(gameObject);
    }

    public void Restart()
    {
        currentTimer = maxTimer;
        StartTimer();
    }
}
