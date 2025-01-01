using JetBrains.Annotations;
using TMPro;
using UnityEngine.EventSystems;

public class TrackedInputfield : TMP_InputField
{
    [CanBeNull]
    public static TMP_InputField Selected;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        Selected = this;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        Selected = null;
    }
}
