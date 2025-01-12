using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static Vector3Data AsData(this Vector3 vector3)
    {
        return new Vector3Data() { x = vector3.x, y = vector3.y, z = vector3.z };
    }

    public static Vector3Data[] AsData(this IEnumerable<Vector3> vectors)
    {
        return vectors.Select(x => AsData(x)).ToArray();
    }

    public static Vector3 AsVector(this Vector3Data vector3)
    {
        return new Vector3() { x = vector3.x, y = vector3.y, z = vector3.z };
    }

    public static Vector3[] AsVector(this IEnumerable<Vector3Data> vectors)
    {
        return vectors.Select(x => AsVector(x)).ToArray();
    }

    public static float SaveParseFloat(this string input)
    {
        if (!float.TryParse(input, out float value))
        {
            value = 0;
        }
        return value;
    }

    public static Color WithAlpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
}
