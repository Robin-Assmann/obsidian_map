using UnityEngine;
using UnityEngine.UI;

public class SiteDisplay : ADisplay<SiteData>
{
    [SerializeField] Image point;

    public override void SetData(SiteData data, CameraController cameraController)
    {
        base.SetData(data, cameraController);
        fadeController.SetData(data, cameraController);
    }

    public override void Refresh(SiteData data)
    {
        base.Refresh(data);
        textContainer.gameObject.SetActive(_data.showText);
        point.sprite = DefManager.GetSiteDef(data.type).SiteIcon;
        fadeController.RefreshData(data);
    }

    public override void SetInteractiblity(bool value)
    {
        point.raycastTarget = value;
    }
}