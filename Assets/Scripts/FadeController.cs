using JetBrains.Annotations;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    static readonly Vector3 MinScale = new Vector3(0.3f, 0.3f, 0.3f);
    static readonly Vector3 MaxScale = new Vector3(1f, 1f, 1f);
    const float MinAlpha = 0f;
    const float MaxAlpha = 0.5f;
    const float FadingDistance = 300;

    [SerializeField] RectTransform rectTx;

    [Header("Region")]
    [SerializeField] MeshRenderer regionMesh;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] Material regionMat;

    [CanBeNull] Color _meshColor;
    CameraController _cameraController;
    CanvasGroup _canvasGroup;
    float _fadeDistance = -1;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (regionMesh != null) regionMesh.material = new Material(regionMat);
    }

    private void Update()
    {
        if (_canvasGroup == null || _fadeDistance < 0) return;

        var distance = -_cameraController.Cam.transform.position.z;
        var clampedDistance = Mathf.Clamp(distance - _fadeDistance, 0, FadingDistance);
        _canvasGroup.alpha = 1 - clampedDistance / FadingDistance;

        var percentage = (distance - CameraController.MinDistance) / 5000;
        rectTx.localScale = Vector3.Lerp(MinScale, MaxScale, percentage);


        if (regionMesh != null && _meshColor != null)
        {
            regionMesh.material.SetColor("_FaceColor", _meshColor.WithAlpha(Mathf.Lerp(MinAlpha, MaxAlpha, 1 - clampedDistance / FadingDistance)));
        }
    }

    public void SetData(IData data, CameraController cameraController)
    {
        _cameraController = cameraController;
        SetFadeDistance(data.GetFadeDistance());
    }

    public void SetFadeDistance(float fadeDistance) => _fadeDistance = fadeDistance;

    public void SetData(SiteData siteData, CameraController cameraController)
    {
        _cameraController = cameraController;
        RefreshData(siteData);
    }

    public void SetData(RegionData regionData, CameraController cameraController)
    {
        _cameraController = cameraController;
        RefreshData(regionData);
    }

    public void SetData(RoadData roadData, CameraController cameraController)
    {
        _cameraController = cameraController;
        RefreshData(roadData);
    }

    public void RefreshData(SiteData siteData)
    {
        _fadeDistance = siteData.GetFadeDistance();
    }

    public void RefreshData(RegionData regionData)
    {
        _fadeDistance = regionData.GetFadeDistance();
        _meshColor = DefManager.GetRegionDef(regionData.type).RegionColor;
    }

    public void RefreshData(RoadData roadData)
    {
        _fadeDistance = roadData.GetFadeDistance();
        _meshColor = DefManager.GetRoadDef(roadData.type).RoadColor;
    }
}