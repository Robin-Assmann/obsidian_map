using System;
using static Enums;

[Serializable]
public class RoadData : IEquatable<RoadData>, IData
{
    public DataType dataType => DataType.Road;
    public ModeType modeType => ModeType.CreateRoad;

    public string displayName { get; set; }
    public RoadTypes type { get; set; }
    public float xPosition { get; set; }
    public float yPosition { get; set; }
    public float xTextOffset { get; set; }
    public float yTextOffset { get; set; }
    public float zTextRotation { get; set; }
    public Vector3Data[] points { get; set; }

    public RoadData() { }

    public bool Equals(RoadData other)
    {
        return displayName.Equals(other.displayName)
            && type.Equals(other.type)
            && xPosition.Equals(other.xPosition)
            && yPosition.Equals(other.yPosition)
            && xTextOffset.Equals(other.xTextOffset)
            && yTextOffset.Equals(other.yTextOffset)
            && zTextRotation.Equals(other.zTextRotation);
    }

    public float GetFadeDistance() => DefManager.GetRoadDef(type).FadeDistance;
    public float GetLabelSize() => DefManager.GetRoadDef(type).LabelSize;
}