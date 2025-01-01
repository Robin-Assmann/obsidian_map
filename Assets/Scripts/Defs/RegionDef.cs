using UnityEngine;
using static Enums;

[CreateAssetMenu(menuName = "Def/Region", fileName = "RegionDef")]
public class RegionDef : ScriptableObject
{
    [SerializeField] RegionTypes type;
    [SerializeField] Color regionColor;
    [SerializeField] float fadeDistance;
    [SerializeField] float labelSize;

    public RegionTypes Type => type;
    public Color RegionColor => regionColor;
    public float FadeDistance => fadeDistance;
    public float LabelSize => labelSize;
}
