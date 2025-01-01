using System;
using UnityEngine;
using static Enums;

public interface IData
{
    DataType dataType { get; }
    ModeType modeType { get; }
    string displayName { get; set; }
    float xPosition { get; set; }
    float yPosition { get; set; }
    float xTextOffset { get; set; }
    float yTextOffset { get; set; }
    float zTextRotation { get; set; }

    float GetFadeDistance();
    float GetLabelSize();
}

[Serializable]
public class Vector3Data
{
    public float x;
    public float y;
    public float z;

    Vector3 ToVector3() => new Vector3(x, y, z);
}