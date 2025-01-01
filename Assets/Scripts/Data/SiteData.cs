using System;
using static Enums;

[Serializable]
public class SiteData : IEquatable<SiteData>, IData
{
    public DataType dataType => DataType.Site;
    public ModeType modeType => ModeType.CreateSite;

    public string displayName { get; set; }
    public bool showText { get; set; }
    public SiteTypes type { get; set; }
    public float xPosition { get; set; }
    public float yPosition { get; set; }
    public float xTextOffset { get; set; }
    public float yTextOffset { get; set; }
    public float zTextRotation { get; set; }

    public SiteData() { }

    public bool Equals(SiteData other)
    {
        return displayName.Equals(other.displayName)
            && showText == other.showText
            && type.Equals(other.type)
            && xPosition.Equals(other.xPosition)
            && yPosition.Equals(other.yPosition)
            && xTextOffset.Equals(other.xTextOffset)
            && yTextOffset.Equals(other.yTextOffset)
            && zTextRotation.Equals(other.zTextRotation);
    }

    public float GetFadeDistance() => DefManager.GetSiteDef(type).FadeDistance;
    public float GetLabelSize() => DefManager.GetSiteDef(type).LabelSize;
}
