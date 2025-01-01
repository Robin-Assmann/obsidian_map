using UnityEngine;
using static Enums;

[CreateAssetMenu(menuName = "Def/Road", fileName = "RoadDef")]
public class RoadDef : ScriptableObject
{
    [SerializeField] RoadTypes type;
    [SerializeField] Color roadColor;
    [SerializeField] float fadeDistance;
    [SerializeField] float labelSize;

    public RoadTypes Type => type;
    public Color RoadColor => roadColor;
    public float FadeDistance => fadeDistance;
    public float LabelSize => labelSize;
}
