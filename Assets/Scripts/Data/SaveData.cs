using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public SiteData[] sites;
    public RegionData[] regions;
    public RoadData[] roads;

    public SaveData()
    {
        sites = Array.Empty<SiteData>();
        regions = Array.Empty<RegionData>();
        roads = Array.Empty<RoadData>();
    }

    public void AddSite(SiteData siteData)
    {
        var list = new List<SiteData>(sites)
        {
            siteData
        };
        sites = list.ToArray();
        SaveManager.Save();
    }

    public void RemoveSite(SiteData siteData)
    {
        var list = new List<SiteData>(sites);
        list.Remove(siteData);
        sites = list.ToArray();
        SaveManager.Save();
    }

    public void AddRegion(RegionData regionData)
    {
        var list = new List<RegionData>(regions)
        {
            regionData
        };
        regions = list.ToArray();
        SaveManager.Save();
    }

    public void RemoveRegion(RegionData regionData)
    {
        var list = new List<RegionData>(regions);
        list.Remove(regionData);
        regions = list.ToArray();
        SaveManager.Save();
    }

    public void AddRoad(RoadData regionData)
    {
        var list = new List<RoadData>(roads)
        {
            regionData
        };
        roads = list.ToArray();
        SaveManager.Save();
    }

    public void RemoveRoad(RoadData regionData)
    {
        var list = new List<RoadData>(roads);
        list.Remove(regionData);
        roads = list.ToArray();
        SaveManager.Save();
    }
}
