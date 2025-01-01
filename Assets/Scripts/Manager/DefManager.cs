using System.Linq;
using UnityEngine;
using static Enums;

public class DefManager : MonoBehaviour
{
    [SerializeField] SiteDef[] siteDefs;
    [SerializeField] RegionDef[] regionDefs;
    [SerializeField] RoadDef[] roadDefs;

    static DefManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static SiteDef GetSiteDef(SiteTypes type) => _instance.siteDefs.First((def) => def.Type == type);
    public static RegionDef GetRegionDef(RegionTypes type) => _instance.regionDefs.First((def) => def.Type == type);
    public static RoadDef GetRoadDef(RoadTypes type) => _instance.roadDefs.First((def) => def.Type == type);

}
