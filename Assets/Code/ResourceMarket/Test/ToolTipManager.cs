using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ToolTipManager : MonoBehaviour
{
    public TextMeshProUGUI toolTipText;
    public RectTransform toolTipWindow;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseExit;

    private void OnEnable()
    {
        OnMouseHover += ShowToolTip;
        OnMouseExit += HideToolTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowToolTip;
        OnMouseExit -= HideToolTip;
    }

    void Start()
    {
        HideToolTip();
    }

    private void ShowToolTip(string toolTip, Vector2 mousePos)
    {
        toolTipText.text = toolTip;
        toolTipWindow.sizeDelta = new Vector2(toolTipText.preferredWidth > 300 ? 300 : toolTipText.preferredWidth, toolTipText.preferredHeight);

        toolTipWindow.gameObject.SetActive(true);
        toolTipWindow.transform.position = new Vector2(mousePos.x + toolTipWindow.sizeDelta.x * 2, mousePos.y);
    }

    private void HideToolTip()
    {
        toolTipText.text = default;
        toolTipWindow.gameObject.SetActive(false);
    }
}
