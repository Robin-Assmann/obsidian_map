using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ADisplay<T> : MonoBehaviour, IPointerClickHandler where T : IData
{
    [SerializeField] protected FadeController fadeController;
    [SerializeField] protected Transform textContainer;
    [SerializeField] protected TextMeshProUGUI underlayText;
    [SerializeField] protected TextMeshProUGUI label;

    public T Data => _data;

    protected T _data;

    public virtual void SetData(T data, CameraController cameraController)
    {
        Refresh(data);
    }

    public virtual void Refresh(T data)
    {
        _data = data;

        transform.position = new Vector3(data.xPosition, data.yPosition, -25f);

        textContainer.localPosition = new Vector3(data.xTextOffset, data.yTextOffset, 0);
        textContainer.localRotation = Quaternion.Euler(0, 0, data.zTextRotation);

        underlayText.text = data.displayName;
        underlayText.fontSize = data.GetLabelSize();
        label.text = data.displayName;
        label.fontSize = data.GetLabelSize();
    }

    public abstract void SetInteractiblity(bool value);

    public void ShowDescriptionWindow()
    {
        WindowController.ShowDescriptionWindow(_data.displayName);
    }

    public void ShowDataWindow()
    {
        WindowController.ShowDataWindow(_data, this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Selected {gameObject.name}");
        ModeManager.SelectDisplay(this);
    }
}