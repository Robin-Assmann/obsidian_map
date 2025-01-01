using System;
using System.Linq;
using Dreamteck.Splines;
using UnityEngine;

public class RegionDisplay : ADisplay<RegionData>
{
    [SerializeField] SplineComputer splineComputer;
    [SerializeField] GameObject splineRenderer;
    [SerializeField] MeshCollider meshCollider;

    public override void SetData(RegionData data, CameraController cameraController)
    {
        base.SetData(data, cameraController);
        fadeController.SetData(data, cameraController);
    }

    public override void Refresh(RegionData data)
    {
        base.Refresh(data);
        splineComputer.Break();
        splineComputer.SetPoints(data.points.Length != 0
            ? data.points.Select(p => new SplinePoint(p.AsVector())).ToArray()
            : Array.Empty<SplinePoint>());
        if (splineComputer.pointCount >= 3) splineComputer.Close();
        splineRenderer.SetActive(splineComputer.pointCount >= 3);
        fadeController.RefreshData(data);
    }

    public override void SetInteractiblity(bool value)
    {
        meshCollider.enabled = value;
    }
}
