using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class JoyStick : ScrollRect
{
    private float radius;
    private float inputAngle;
    private Transform imgArrowTrans;
    protected override void Start()
    {
        base.Start();
        radius = content.sizeDelta.x * 0.5f;
    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        Vector2 contentPosition=content.anchoredPosition;
        if (contentPosition.magnitude>radius)
        {
            contentPosition = contentPosition.normalized * radius;
            SetContentAnchoredPosition(contentPosition);
        }
        Vector2 inputPosition = contentPosition;
        GameManager.instance.inputValue = inputPosition;
        inputAngle = -Mathf.Atan2(GameManager.instance.inputValue.x,GameManager.instance.inputValue.y)*Mathf.Rad2Deg;
        GameManager.instance.inputAngle = inputAngle;
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        SetContentAnchoredPosition(Vector2.zero);
        GameManager.instance.inputValue = Vector2.zero;
    }
}
