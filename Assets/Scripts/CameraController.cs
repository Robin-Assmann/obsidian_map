using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public const float MaxDistance = 9900;
    public const float MinDistance = 600;

    [SerializeField] Camera cam;
    [SerializeField] Transform targetTx;
    [SerializeField] float dragSpeed = 0.001f;
    [SerializeField] float zoomSpeed = 1;
    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] CinemachineConfiner2D confiner2D;
    [SerializeField] RectTransform mapRect;

    public event Action zoomChanged;

    public Camera Cam => cam;
    public float ZoomPercentage => Mathf.Clamp01((_camDistance) / (MaxDistance));

    static CameraController _instance;
    float _camDistance;
    Vector3 _distance;
    Vector2 _startDrag;
    Vector3 _startPosition;
    float _mapWidth;
    float _mapHeight;
    Bounds _targetBounds = new();
    CinemachineFramingTransposer _framingTransposer;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void OnEnable()
    {
        _mapWidth = mapRect.rect.width * mapRect.lossyScale.x;
        _mapHeight = mapRect.rect.height * mapRect.lossyScale.y;

        CinemachineComponentBase componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is CinemachineFramingTransposer)
        {
            _framingTransposer = (componentBase as CinemachineFramingTransposer);
            _camDistance = _framingTransposer.m_CameraDistance;
        }
        AdjustBounds();
    }

    void Update()
    {
        if (_framingTransposer == null) return;
        confiner2D.InvalidateCache();

        if (Input.GetKey(KeyCode.UpArrow))
        {

            MovePosition(Vector3.up);
            return;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {

            MovePosition(Vector3.down);
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {

            MovePosition(Vector3.left);
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {

            MovePosition(Vector3.right);
            return;
        }

        if (Input.mouseScrollDelta == Vector2.zero) return;

        _camDistance = Mathf.Clamp(_framingTransposer.m_CameraDistance + Input.mouseScrollDelta.y * zoomSpeed, MinDistance, MaxDistance);
        _framingTransposer.m_CameraDistance = _camDistance;
        zoomChanged?.Invoke();
        AdjustBounds();
    }

    public static void ZoomTo(Vector2 position, float distance) => _instance.ZoomToPositionAndDistance(position, distance);

    public void ZoomToPositionAndDistance(Vector2 position, float distance)
    {
        MoveToPosition(position);
        _camDistance = Mathf.Clamp(distance, MinDistance, MaxDistance); ;
        _framingTransposer.m_CameraDistance = _camDistance;
        zoomChanged?.Invoke();
    }

    public void MoveToPosition(Vector2 position)
    {
        targetTx.localPosition = position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = targetTx.position;
        _startDrag = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var delta = _startDrag - eventData.position;
        var newPosition = _startPosition + ((Vector3)(dragSpeed * (_camDistance / MaxDistance) * delta));
        MoveAndClampTarget(newPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _startDrag = Vector2.zero;
    }

    void AdjustBounds()
    {
        var borderPoint = new Vector3(0, 0, -cam.transform.position.z);
        var borderPosition = cam.ScreenToWorldPoint(borderPoint);

        var midPoint = new Vector3(Screen.width / 2f, Screen.height / 2f, -cam.transform.position.z);
        var midPosition = cam.ScreenToWorldPoint(midPoint);

        _distance = midPosition - borderPoint;
        _targetBounds.min = new Vector3(-((_mapWidth + _distance.x) / 2f), -((_mapHeight + _distance.y) / 2f) + 0.3f * _distance.y);
        _targetBounds.max = new Vector3(((_mapWidth + _distance.x) / 2f), ((_mapHeight + _distance.y) / 2f - 0.3f * _distance.y));
        MoveAndClampTarget(targetTx.position);
    }

    void MovePosition(Vector3 delta)
    {
        var newPosition = targetTx.position + ((dragSpeed * (_camDistance / MaxDistance) * delta));
        MoveAndClampTarget(newPosition);
    }

    void MoveAndClampTarget(Vector3 newPosition)
    {
        targetTx.position = new Vector2(Mathf.Clamp(newPosition.x, _targetBounds.min.x, _targetBounds.max.x), Mathf.Clamp(newPosition.y, _targetBounds.min.y, _targetBounds.max.y));
    }
}