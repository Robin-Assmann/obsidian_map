using System;
using static Enums;

[Serializable]
public class RegionData : IEquatable<RegionData>, IData
{
    public DataType dataType => DataType.Region;
    public ModeType modeType => ModeType.CreateRegion;

    public string displayName { get; set; }
    public RegionTypes type { get; set; }
    public float xPosition { get; set; }
    public float yPosition { get; set; }
    public float xTextOffset { get; set; }
    public float yTextOffset { get; set; }
    public float zTextRotation { get; set; }
    public Vector3Data[] points { get; set; }

    public RegionData() { }

    public bool Equals(RegionData other)
    {
        return displayName.Equals(other.displayName)
            && type.Equals(other.type)
            && xPosition.Equals(other.xPosition)
            && yPosition.Equals(other.yPosition)
            && xTextOffset.Equals(other.xTextOffset)
            && yTextOffset.Equals(other.yTextOffset)
            && zTextRotation.Equals(other.zTextRotation);
    }

    public float GetFadeDistance() => DefManager.GetRegionDef(type).FadeDistance;
    public float GetLabelSize() => DefManager.GetRegionDef(type).LabelSize;
}
