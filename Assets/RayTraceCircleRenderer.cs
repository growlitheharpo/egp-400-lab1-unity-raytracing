using System.Collections.Generic;
using UnityEngine;

public class RayTraceCircleRenderer : RayTraceRenderer
{
    private Vector2 _circleCenter;

    protected override void Awake()
    {
        base.Awake();
        _circleCenter = Vector2.one * 0.5f;
    }

    public override Color CalculateColor(Ray originalRay, RaycastHit hitInfo, int depth)
    {
        if (depth < 0)
            return Color.clear;

        Vector3 interpNormal = GetInterpNormal(hitInfo.barycentricCoordinate, hitInfo.normal, hitInfo.triangleIndex);
        Color lightAmt = CalculateLight(hitInfo.point, interpNormal);
        Color fragColor = Color.white * lightAmt;

        float rad = 0.125f;
        if ((hitInfo.textureCoord - _circleCenter).sqrMagnitude < rad)
            fragColor = Color.red * lightAmt;

        fragColor.a = 1.0f;

        return fragColor;
    }
}
