using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class RoadWindow : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_Dropdown typeDropdown;
    [SerializeField] TMP_InputField xTextOffset;
    [SerializeField] TMP_InputField yTextOffset;
    [SerializeField] TMP_InputField zTextRotation;
    [SerializeField] Button closeButton;
    [SerializeField] Button newButton;
    [SerializeField] Button deleteButton;

    RoadData _roadData;
    RoadDisplay _display;

    private void Awake()
    {
        nameInput.onValueChanged.AddListener(value => { _roadData.displayName = value; RefreshDisplay(); });
        nameInput.onSelect.AddListener(_ => ModeManager.SetInputFieldSelection(true));
        nameInput.onDeselect.AddListener(_ => ModeManager.SetInputFieldSelection(false));
        xTextOffset.onValueChanged.AddListener(value => { _roadData.xTextOffset = value.SaveParseFloat(); RefreshDisplay(); });
        yTextOffset.onValueChanged.AddListener(value => { _roadData.yTextOffset = value.SaveParseFloat(); RefreshDisplay(); });
        zTextRotation.onValueChanged.AddListener(value => { _roadData.zTextRotation = value.SaveParseFloat(); RefreshDisplay(); });
        closeButton.onClick.AddListener(() => {
            ModeManager.UnselectDisplays();
            gameObject.SetActive(false);
        });
        deleteButton.onClick.AddListener(() => {
            if (_roadData != null)
            {
                DisplayManager.RemoveRoad(_roadData);
            }
        });
        newButton.onClick.AddListener(() => {
            ModeManager.UnselectDisplays();
        });

        typeDropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (RoadTypes type in Enum.GetValues(typeof(RoadTypes)))
        {
            options.Add(new TMP_Dropdown.OptionData(type.ToString()));
        }
        typeDropdown.AddOptions(options);
        typeDropdown.onValueChanged.AddListener(value => {
            _roadData.type = (RoadTypes)value;
            RefreshDisplay();
        });
    }

    public void SetData(RoadData roadData, RoadDisplay display)
    {
        _roadData = roadData;
        _display = display;
        Refresh();
        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        _display.Refresh(_roadData);
    }

    void Refresh()
    {
        nameInput.SetTextWithoutNotify(_roadData.displayName);
        xTextOffset.SetTextWithoutNotify(_roadData.xTextOffset + "");
        yTextOffset.SetTextWithoutNotify(_roadData.yTextOffset + "");
        zTextRotation.SetTextWithoutNotify(_roadData.zTextRotation + "");
        typeDropdown.SetValueWithoutNotify((int)_roadData.type);
    }
}