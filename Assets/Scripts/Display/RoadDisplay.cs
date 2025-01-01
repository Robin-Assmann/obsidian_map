using System;
using System.Linq;
using Dreamteck.Splines;
using UnityEngine;

public class RoadDisplay : ADisplay<RoadData>
{
    [SerializeField] SplineComputer splineComputer;
    [SerializeField] GameObject splineRenderer;
    [SerializeField] MeshCollider meshCollider;

    public override void SetData(RoadData data, CameraController cameraController)
    {
        base.SetData(data, cameraController);
        fadeController.SetData(data, cameraController);
    }

    public override void Refresh(RoadData data)
    {
        base.Refresh(data);
        splineComputer.SetPoints(data.points.Length != 0
            ? data.points.Select(p => new SplinePoint(p.AsVector())).ToArray()
            : Array.Empty<SplinePoint>());
        splineRenderer.SetActive(splineComputer.pointCount >= 2);
        fadeController.RefreshData(data);
    }

    public override void SetInteractiblity(bool value)
    {
        meshCollider.enabled = value;
    }
}