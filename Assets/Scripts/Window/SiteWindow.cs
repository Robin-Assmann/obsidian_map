using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Enums;

public class SiteWindow : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] Toggle showTextToggle;
    [SerializeField] TMP_Dropdown typeDropdown;
    [SerializeField] TMP_InputField xPosition;
    [SerializeField] TMP_InputField yPosition;
    [SerializeField] TMP_InputField xTextOffset;
    [SerializeField] TMP_InputField yTextOffset;
    [SerializeField] TMP_InputField zTextRotation;
    [SerializeField] Button closeButton;
    [SerializeField] Button deleteButton;

    SiteData _siteData;
    SiteDisplay _display;

    private void Awake()
    {
        nameInput.onValueChanged.AddListener(value => { _siteData.displayName = value; RefreshDisplay(); });
        nameInput.onSelect.AddListener(_ => ModeManager.SetInputFieldSelection(true));
        nameInput.onDeselect.AddListener(_ => ModeManager.SetInputFieldSelection(false));
        showTextToggle.onValueChanged.AddListener(value => { _siteData.showText = value; RefreshDisplay(); });
        xPosition.onValueChanged.AddListener(value => { _siteData.xPosition = value.SaveParseFloat(); RefreshDisplay(); });
        yPosition.onValueChanged.AddListener(value => { _siteData.yPosition = value.SaveParseFloat(); RefreshDisplay(); });
        xTextOffset.onValueChanged.AddListener(value => { _siteData.xTextOffset = value.SaveParseFloat(); RefreshDisplay(); });
        yTextOffset.onValueChanged.AddListener(value => { _siteData.yTextOffset = value.SaveParseFloat(); RefreshDisplay(); });
        zTextRotation.onValueChanged.AddListener(value => { _siteData.zTextRotation = value.SaveParseFloat(); RefreshDisplay(); });
        closeButton.onClick.AddListener(() => { gameObject.SetActive(false); });
        deleteButton.onClick.AddListener(() => { if (_siteData != null) DisplayManager.RemoveSite(_siteData); });

        typeDropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (SiteTypes type in Enum.GetValues(typeof(SiteTypes)))
        {
            options.Add(new TMP_Dropdown.OptionData(type.ToString()));
        }
        typeDropdown.AddOptions(options);
        typeDropdown.onValueChanged.AddListener(value => { _siteData.type = (SiteTypes)value; RefreshDisplay(); });
    }

    public void SetData(SiteData siteData, SiteDisplay display)
    {
        _siteData = siteData;
        _display = display;
        Refresh();
        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        _display.Refresh(_siteData);
    }

    void Refresh()
    {
        nameInput.SetTextWithoutNotify(_siteData.displayName);
        showTextToggle.SetIsOnWithoutNotify(_siteData.showText);
        xPosition.SetTextWithoutNotify(_siteData.xPosition + "");
        yPosition.SetTextWithoutNotify(_siteData.yPosition + "");
        xTextOffset.SetTextWithoutNotify(_siteData.xTextOffset + "");
        yTextOffset.SetTextWithoutNotify(_siteData.yTextOffset + "");
        zTextRotation.SetTextWithoutNotify(_siteData.zTextRotation + "");
        typeDropdown.SetValueWithoutNotify((int)_siteData.type);
    }
}
