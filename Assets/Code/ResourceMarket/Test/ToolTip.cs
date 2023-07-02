using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    public string tooltipToShow;
    [SerializeField]
    private float fadeTime = 0.5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTimer());
        Debug.Log("Tooltip enabled");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        ToolTipManager.OnMouseExit();

        Debug.Log("Tooltip disabled");
    }

    private void ShowMessage()
    {
        ToolTipManager.OnMouseHover(tooltipToShow, Input.mousePosition);
    }

    private IEnumerator FadeTimer()
    {
        yield return new WaitForSeconds(fadeTime);

        ShowMessage();
    }
}
