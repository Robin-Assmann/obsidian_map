using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

public class DistanceManager : MonoBehaviour
{
    const float MinRadius = 0.2f;
    const float MaxRadius = 2f;

    [SerializeField] CameraController cameraController;
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] SplineExtrude splineExtrude;

    static DistanceManager _instance;

    List<Vector3> _points = new List<Vector3>();
    Spline _spline = new Spline();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        //Subsribe to Camera Zoom Changes and call for initial time
        cameraController.zoomChanged += OnZoomChanged;
        OnZoomChanged();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1) && _points.Count >= 2)
        {
            Reset();
            WindowController.CloseDistanceWindow();
        }
    }

    public static void InstantReset() => _instance.Reset();

    public void Reset()
    {
        _spline.Clear();
        _points.Clear();
        splineExtrude.Rebuild();
        splineExtrude.gameObject.SetActive(false);
    }

    void OnZoomChanged()
    {
        splineExtrude.Radius = Mathf.Clamp(cameraController.ZoomPercentage, MinRadius, MaxRadius);
        splineExtrude.Rebuild();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            return;
        }

        var position = new Vector3(eventData.position.x, eventData.position.y, -cameraController.Cam.transform.position.z);

        _points.Add(cameraController.Cam.ScreenToWorldPoint(position));

        RefreshDistance();
    }

    void RefreshDistance()
    {
        _spline.Clear();
        var distance = 0f;
        for (var i = 0; i < _points.Count; i++)
        {
            if (i > 0)
            {
                distance += Vector3.Distance(_points[i], _points[i - 1]);
            }

            var point = _points[i];
            _spline.Add(new BezierKnot(point));
        }

        if (_points.Count >= 2)
        {
            WindowController.ShowDistanceWindow(distance);
            splineExtrude.gameObject.SetActive(true);

        } else
        {
            WindowController.CloseDistanceWindow();
            splineExtrude.gameObject.SetActive(true);
        }


        splineContainer.Spline = _spline;
        splineExtrude.Rebuild();

    }
}
