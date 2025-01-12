using UnityEngine;
using static Enums;

public class WindowController : MonoBehaviour
{
    [SerializeField] SiteWindow siteWindow;
    [SerializeField] RegionWindow regionWindow;
    [SerializeField] RoadWindow roadWindow;
    [SerializeField] DescriptionWindow descriptionWindow;
    [SerializeField] CameraController cameraController;
    [SerializeField] DistanceWindow distanceWindow;

    static WindowController _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }


        siteWindow.gameObject.SetActive(false);
    }

    public static void ShowDescriptionWindow(string title) => _instance.DoShowDescriptionWindow(title);

    public static void ShowDataWindow<T>(IData data, ADisplay<T> display) where T : IData => _instance.DoShowDataWindow(data, display);

    public static void ShowDistanceWindow(float distance) => _instance.DoShowDistanceWindow(distance);

    public static void CloseDataWindows()
    {
        _instance.siteWindow.gameObject.SetActive(false);
        _instance.regionWindow.gameObject.SetActive(false);
        _instance.roadWindow.gameObject.SetActive(false);
    }

    public static void CloseDescriptionWindow() => _instance.descriptionWindow.gameObject.SetActive(false);

    public static void CloseDistanceWindow() => _instance.distanceWindow.gameObject.SetActive(false);

    void DoShowDataWindow<T>(IData data, ADisplay<T> display) where T : IData
    {
        cameraController.MoveToPosition(new Vector2(data.xPosition, data.yPosition));

        switch (data.dataType)
        {
            case DataType.Site:
                siteWindow.SetData(data as SiteData, display as SiteDisplay);
                siteWindow.gameObject.SetActive(true);
                break;
            case DataType.Region:
                regionWindow.SetData(data as RegionData, display as RegionDisplay);
                regionWindow.gameObject.SetActive(true);
                break;
            case DataType.Road:
                roadWindow.SetData(data as RoadData, display as RoadDisplay);
                roadWindow.gameObject.SetActive(true);
                break;
        }
    }

    void DoShowDescriptionWindow(string title)
    {
        descriptionWindow.SetData(title);
        descriptionWindow.gameObject.SetActive(true);
    }

    void DoShowDistanceWindow(float distance)
    {
        distanceWindow.SetData(distance);
        distanceWindow.gameObject.SetActive(true);
    }
}