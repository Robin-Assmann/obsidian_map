using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.U2D;

public class RegionManager : MonoBehaviour
{
    [SerializeField] SpriteShapeController shape;
    [SerializeField] SplineComputer splineComputer;

    public void AddNode(Vector3 worldPosition)
    {
        splineComputer.Break();
        splineComputer.SetPoint(splineComputer.pointCount, new SplinePoint(worldPosition));

        if (splineComputer.pointCount >= 3)
        {
            splineComputer.Close();
        }
    }

    public void RemoveLastNode()
    {
        splineComputer.Break();
        splineComputer.DisconnectNode(splineComputer.pointCount - 1);
    }

    public void Clear()
    {
        splineComputer.SetPoints(Array.Empty<SplinePoint>());
    }
}
