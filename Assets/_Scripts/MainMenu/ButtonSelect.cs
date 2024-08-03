using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 scaleAnimEnd;

    private RectTransform m_RectTransform;

    private void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_RectTransform != null)
        m_RectTransform.DOScale(scaleAnimEnd, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_RectTransform != null)
        m_RectTransform.DOScale(Vector3.one, 0.1f);
    }
}
