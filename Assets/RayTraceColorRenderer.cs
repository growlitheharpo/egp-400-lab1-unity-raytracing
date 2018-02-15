using System.Collections.Generic;
using UnityEngine;

public class RayTraceColorRenderer : RayTraceRenderer
{
    [SerializeField] private Color _color;

    public override Color CalculateColor(Ray originalRay, RaycastHit hitInfo, int depth)
    {
        if (depth < 0)
            return Color.clear;

        Vector3 interpNormal = GetInterpNormal(hitInfo.barycentricCoordinate, hitInfo.normal, hitInfo.triangleIndex);
        Color lightAmt = CalculateLight(hitInfo.point, interpNormal);

        Color col = _color * lightAmt;

        col.a = 1.0f;
        return col;
    }
}
