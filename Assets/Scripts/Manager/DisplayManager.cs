using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] SiteDisplay siteDisplay;
    [SerializeField] RegionDisplay regionDisplay;
    [SerializeField] RoadDisplay roadDisplay;
    [SerializeField] CameraController cameraController;

    Dictionary<SiteData, SiteDisplay> _spawnedSiteDisplays = new Dictionary<SiteData, SiteDisplay>();
    Dictionary<RegionData, RegionDisplay> _spawnedRegionDisplays = new Dictionary<RegionData, RegionDisplay>();
    Dictionary<RoadData, RoadDisplay> _spawnedRoadDisplays = new Dictionary<RoadData, RoadDisplay>();

    bool _siteInteractibility = false;
    bool _regionInteractibility = false;
    bool _roadInteractibility = false;

    static DisplayManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static void Init() => _instance.DoInit();

    void DoInit()
    {
        var saveData = SaveManager.saveData;

        foreach (var site in saveData.sites)
        {
            AddSite(site);
        }

        foreach (var region in saveData.regions)
        {
            AddRegion(region);
        }

        foreach (var road in saveData.roads)
        {
            AddRoad(road);
        }
    }

    public void SetSiteInteractibility(bool value)
    {
        _siteInteractibility = value;
        foreach (var display in _spawnedSiteDisplays.Values)
        {
            display.SetInteractiblity(value);
        }
    }

    public void SetRegionInteractibility(bool value)
    {
        _regionInteractibility = value;
        foreach (var display in _spawnedRegionDisplays.Values)
        {
            display.SetInteractiblity(value);
        }
    }

    public void SetRoadInteractibility(bool value)
    {
        _roadInteractibility = value;
        foreach (var display in _spawnedRoadDisplays.Values)
        {
            display.SetInteractiblity(value);
        }
    }

    public SiteDisplay AddSite(SiteData siteData)
    {
        var siteD = Instantiate(siteDisplay, container);
        siteD.SetData(siteData, cameraController);
        siteD.SetInteractiblity(_siteInteractibility);
        _spawnedSiteDisplays.Add(siteData, siteD);
        return siteD;
    }

    public static void RemoveSite(SiteData siteData) => _instance.DoRemoveSite(siteData);

    void DoRemoveSite(SiteData siteData)
    {
        if (_spawnedSiteDisplays.TryGetValue(siteData, out var display))
        {
            Destroy(display.gameObject);
            _spawnedSiteDisplays.Remove(siteData);
            SaveManager.saveData.RemoveSite(siteData);
            WindowController.CloseDataWindows();
            ModeManager.SetToggleOn(Enums.ModeType.Move);
            return;
        }

        Debug.LogWarning("Tried RemoveSite, but could not find Display!");
    }

    public RegionDisplay AddRegion(RegionData regionData)
    {
        var regionD = Instantiate(regionDisplay, container);
        regionD.SetData(regionData, cameraController);
        regionD.SetInteractiblity(_regionInteractibility);
        _spawnedRegionDisplays.Add(regionData, regionD);
        return regionD;
    }

    public static void RemoveRegion(RegionData siteData) => _instance.DoRemoveRegion(siteData);

    void DoRemoveRegion(RegionData regionData)
    {
        if (_spawnedRegionDisplays.TryGetValue(regionData, out var display))
        {
            Destroy(display.gameObject);
            _spawnedRegionDisplays.Remove(regionData);
            SaveManager.saveData.RemoveRegion(regionData);
            WindowController.CloseDataWindows();
            ModeManager.SetToggleOn(Enums.ModeType.Move);
            return;
        }

        Debug.LogWarning("Tried RemoveRegion, but could not find Display!");
    }

    public RoadDisplay AddRoad(RoadData roadData)
    {
        var regionD = Instantiate(roadDisplay, container);
        regionD.SetData(roadData, cameraController);
        regionD.SetInteractiblity(_roadInteractibility);
        _spawnedRoadDisplays.Add(roadData, regionD);
        return regionD;
    }

    public static void RemoveRoad(RoadData siteData) => _instance.DoRemoveRoad(siteData);

    void DoRemoveRoad(RoadData roadData)
    {
        if (_spawnedRoadDisplays.TryGetValue(roadData, out var display))
        {
            Destroy(display.gameObject);
            _spawnedRoadDisplays.Remove(roadData);
            SaveManager.saveData.RemoveRoad(roadData);
            WindowController.CloseDataWindows();
            ModeManager.SetToggleOn(Enums.ModeType.Move);
            return;
        }

        Debug.LogWarning("Tried RemoveRoad, but could not find Display!");
    }

    public static void SelectBestFitDisplay(string searchTerm) => _instance.DoSelectBestFitDisplay(searchTerm);

    void DoSelectBestFitDisplay(string searchTerm)
    {
        ModeManager.SetToggleOn(Enums.ModeType.Move);

        var bestSite = FuzzySharp.Process.ExtractOne(searchTerm, _spawnedSiteDisplays.Keys.Select((key) => key.displayName));
        var bestRegion = FuzzySharp.Process.ExtractOne(searchTerm, _spawnedRegionDisplays.Keys.Select((key) => key.displayName));
        var bestRoad = FuzzySharp.Process.ExtractOne(searchTerm, _spawnedRoadDisplays.Keys.Select((key) => key.displayName));
        if (bestSite.Score > bestRegion.Score && bestSite.Score > bestRoad.Score)
        {
            var siteEntry = _spawnedSiteDisplays.First((kv) => kv.Key.displayName == bestSite.Value);
            CameraController.ZoomTo(siteEntry.Value.transform.position, siteEntry.Key.GetFadeDistance());
            siteEntry.Value.ShowDescriptionWindow();
            return;
        }

        if (bestRoad.Score > bestRegion.Score) {
            var roadEntry = _spawnedRoadDisplays.First((kv) => kv.Key.displayName == bestRoad.Value);
            CameraController.ZoomTo(roadEntry.Value.transform.position, roadEntry.Key.GetFadeDistance());
            roadEntry.Value.ShowDescriptionWindow();
            return;
        }

        var regionEntry = _spawnedRegionDisplays.First((kv) => kv.Key.displayName == bestRegion.Value);
        CameraController.ZoomTo(regionEntry.Value.transform.position, regionEntry.Key.GetFadeDistance());
        regionEntry.Value.ShowDescriptionWindow();
    }
}