using UnityEngine;
using static Enums;

[CreateAssetMenu(menuName ="Def/Site", fileName = "SiteDef")]
public class SiteDef : ScriptableObject
{
    [SerializeField] SiteTypes type;
    [SerializeField] Sprite siteIcon;
    [SerializeField] float fadeDistance;
    [SerializeField] float labelSize;

    public SiteTypes Type => type;
    public Sprite SiteIcon => siteIcon;
    public float FadeDistance => fadeDistance;
    public float LabelSize => labelSize;
}
