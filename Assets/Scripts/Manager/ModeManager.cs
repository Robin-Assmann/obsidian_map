using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Enums;

public class ModeManager : MonoBehaviour
{
    [Serializable]
    class ToggleButton
    {
        public KeyCode keyCode;
        public Toggle toggle;
        public ModeType modesType;
    }

    [SerializeField] ToggleButton[] modeToggles;
    [SerializeField] CameraController cameraController;
    [SerializeField] DisplayManager displayManager;
    [SerializeField] DistanceManager distanceManager;
    [SerializeField] RegionManager regionManager;

    static ModeManager _instance;
    bool _inputFieldSelected;
    ModeType _currentType;
    RegionDisplay _selectedRegionDisplay;
    RoadDisplay _selectedRoadDisplay;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        foreach (var modeToggle in modeToggles)
        {
            modeToggle.toggle.onValueChanged.AddListener((isOn) => { if (isOn) ChangeMode(modeToggle.modesType); });
        }

        modeToggles[0].toggle.isOn = true;
    }

    private void Update()
    {
        if (!Input.anyKey) return;
        if (_inputFieldSelected) return;

        foreach (var modeToggle in modeToggles)
        {
            if (Input.GetKey(modeToggle.keyCode))
            {
                modeToggle.toggle.isOn = true;
            }
        }
    }

    public static void SetToggleOn(ModeType type) => _instance.DoSetToggleOn(type);

    void DoSetToggleOn(ModeType type)
    {
        modeToggles.First(t => t.modesType == type).toggle.isOn = true;
    }

    void ChangeMode(ModeType newType)
    {
        _currentType = newType;

        displayManager.SetRegionInteractibility(newType == ModeType.Move || newType == ModeType.CreateRegion);
        displayManager.SetSiteInteractibility(newType == ModeType.Move || newType == ModeType.CreateSite);
        displayManager.SetRoadInteractibility(newType == ModeType.Move || newType == ModeType.CreateRoad);

        if (newType != ModeType.CheckDistance)
        {
            distanceManager.Reset();
        }

        if (newType != ModeType.CreateRegion) _selectedRegionDisplay = null;
    }

    public static void SelectDisplay<T>(ADisplay<T> display) where T : IData => _instance.DoSelectDisplay(display);

    void DoSelectDisplay<T>(ADisplay<T> display) where T : IData
    {
        if (display.Data.modeType == ModeType.CreateRegion)
        {
            _selectedRegionDisplay = display as RegionDisplay;
        }

        if (display.Data.modeType == ModeType.CreateRoad)
        {
            _selectedRoadDisplay = display as RoadDisplay;
        }

        if (_currentType == display.Data.modeType)
        {
            display.ShowDataWindow();
        }

        if (_currentType == ModeType.Move)
        {
            display.ShowDescriptionWindow();
        }
    }

    public static void UnselectDisplays() => _instance.DoUnselectDisplays();

    void DoUnselectDisplays()
    {
        _selectedRegionDisplay = null;
        _selectedRoadDisplay = null;
        WindowController.CloseDescriptionWindow();
        WindowController.CloseDataWindows();
    }

    public static void SetInputFieldSelection(bool isSelected) => _instance._inputFieldSelected = isSelected;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_currentType == ModeType.Move) cameraController.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_currentType == ModeType.Move) cameraController.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_currentType == ModeType.Move) cameraController.OnEndDrag(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentType == ModeType.CreateSite)
        {
            var screenPosition = new Vector3(eventData.position.x, eventData.position.y, -cameraController.Cam.transform.position.z);
            var position = cameraController.Cam.ScreenToWorldPoint(screenPosition);

            var siteData = new SiteData();
            siteData.displayName = "New Town";
            siteData.type = SiteTypes.City;
            siteData.yTextOffset = 50;
            siteData.xPosition = Mathf.RoundToInt(position.x * 100) / 100f;
            siteData.yPosition = Mathf.RoundToInt(position.y * 100) / 100f;

            SaveManager.saveData.AddSite(siteData);

            var display = displayManager.AddSite(siteData);
            display.ShowDataWindow();
            SetToggleOn(ModeType.Move);
        }

        if (_currentType == ModeType.CreateRegion)
        {
            var screenPosition = new Vector3(eventData.position.x, eventData.position.y, -cameraController.Cam.transform.position.z);
            var position = cameraController.Cam.ScreenToWorldPoint(screenPosition);

            if (_selectedRegionDisplay != null)
            {
                var points = _selectedRegionDisplay.Data.points.AsVector();
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    _selectedRegionDisplay.Data.points = points.Take(points.Length - 1).AsData();
                    _selectedRegionDisplay.Refresh(_selectedRegionDisplay.Data);
                    return;
                }

                var list = new List<Vector3>(points)
                {
                    position
                };
                _selectedRegionDisplay.Data.points = list.AsData();
                _selectedRegionDisplay.Refresh(_selectedRegionDisplay.Data);
            } else
            {
                var regionData = new RegionData();
                regionData.displayName = "New Region";
                regionData.type = RegionTypes.Empire;
                regionData.xPosition = Mathf.RoundToInt(position.x * 100) / 100f;
                regionData.yPosition = Mathf.RoundToInt(position.y * 100) / 100f;
                regionData.points = Array.Empty<Vector3Data>();

                SaveManager.saveData.AddRegion(regionData);

                var display = displayManager.AddRegion(regionData);
                display.ShowDataWindow();
                _selectedRegionDisplay = display;
            }
        }

        if (_currentType == ModeType.CreateRoad)
        {
            var screenPosition = new Vector3(eventData.position.x, eventData.position.y, -cameraController.Cam.transform.position.z);
            var position = cameraController.Cam.ScreenToWorldPoint(screenPosition);

            if (_selectedRoadDisplay != null)
            {
                var points = _selectedRoadDisplay.Data.points.AsVector();
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    _selectedRoadDisplay.Data.points = points.Take(points.Length - 1).AsData();
                    _selectedRoadDisplay.Refresh(_selectedRoadDisplay.Data);
                    return;
                }

                var list = new List<Vector3>(points)
                {
                    position
                };
                _selectedRoadDisplay.Data.points = list.AsData();
                _selectedRoadDisplay.Refresh(_selectedRoadDisplay.Data);
            } else
            {
                var roadData = new RoadData();
                roadData.displayName = "New Road";
                roadData.type = RoadTypes.Street;
                roadData.xPosition = Mathf.RoundToInt(position.x * 100) / 100f;
                roadData.yPosition = Mathf.RoundToInt(position.y * 100) / 100f;
                roadData.points = Array.Empty<Vector3Data>();

                SaveManager.saveData.AddRoad(roadData);

                var display = displayManager.AddRoad(roadData);
                display.ShowDataWindow();
                _selectedRoadDisplay = display;
            }
        }

        if (_currentType == ModeType.CheckDistance)
        {
            distanceManager.OnPointerClick(eventData);
        }
    }
}