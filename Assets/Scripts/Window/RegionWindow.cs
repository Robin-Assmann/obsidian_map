using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class RegionWindow : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_Dropdown typeDropdown;
    [SerializeField] TMP_InputField xTextOffset;
    [SerializeField] TMP_InputField yTextOffset;
    [SerializeField] TMP_InputField zTextRotation;
    [SerializeField] Button closeButton;
    [SerializeField] Button newButton;
    [SerializeField] Button deleteButton;

    RegionData _regionData;
    RegionDisplay _display;

    private void Awake()
    {
        nameInput.onValueChanged.AddListener(value => { _regionData.displayName = value; RefreshDisplay(); });
        nameInput.onSelect.AddListener(_ => ModeManager.SetInputFieldSelection(true));
        nameInput.onDeselect.AddListener(_ => ModeManager.SetInputFieldSelection(false));
        xTextOffset.onValueChanged.AddListener(value => { _regionData.xTextOffset = value.SaveParseFloat(); RefreshDisplay(); });
        yTextOffset.onValueChanged.AddListener(value => { _regionData.yTextOffset = value.SaveParseFloat(); RefreshDisplay(); });
        zTextRotation.onValueChanged.AddListener(value => { _regionData.zTextRotation = value.SaveParseFloat(); RefreshDisplay(); });
        closeButton.onClick.AddListener(() => {
            ModeManager.UnselectDisplays();
            gameObject.SetActive(false);
        });
        deleteButton.onClick.AddListener(() => {
            if (_regionData != null)
            {
                DisplayManager.RemoveRegion(_regionData);
            }
        });
        newButton.onClick.AddListener(() => {
            ModeManager.UnselectDisplays();
        });

        typeDropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (RegionTypes type in Enum.GetValues(typeof(RegionTypes)))
        {
            options.Add(new TMP_Dropdown.OptionData(type.ToString()));
        }
        typeDropdown.AddOptions(options);
        typeDropdown.onValueChanged.AddListener(value => {
            Debug.Log($"Region {_regionData == null} {value}");
            _regionData.type = (RegionTypes)value;
            RefreshDisplay();
        });
    }

    public void SetData(RegionData regionData, RegionDisplay display)
    {
        Debug.Log($"RegionData {regionData == null}");
        _regionData = regionData;
        _display = display;
        Refresh();
        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        _display.Refresh(_regionData);
    }

    void Refresh()
    {
        nameInput.SetTextWithoutNotify(_regionData.displayName);
        xTextOffset.SetTextWithoutNotify(_regionData.xTextOffset + "");
        yTextOffset.SetTextWithoutNotify(_regionData.yTextOffset + "");
        zTextRotation.SetTextWithoutNotify(_regionData.zTextRotation + "");
        typeDropdown.SetValueWithoutNotify((int)_regionData.type);
    }
}

