using UnityEngine;

public class RayTraceReflectiveRenderer : RayTraceRenderer
{
    [SerializeField] private Color _color;
    [SerializeField] private float _reflectivity = 1.0f;

    public override Color CalculateColor(Ray originalRay, RaycastHit hitInfo, int depth)
    {
        if (depth < 0)
            return Color.clear;

        Vector3 interpNormal = GetInterpNormal(hitInfo.barycentricCoordinate, hitInfo.normal, hitInfo.triangleIndex);
        Color lightAmt = CalculateLight(hitInfo.point, interpNormal);

        Vector3 reflect = Vector3.Reflect(originalRay.direction, interpNormal);
        Vector3 start = hitInfo.point + (interpNormal * _bounceBias);
        Color reflection = Bounce(new Ray(start, reflect), depth);

        Color col = (_color + reflection * _reflectivity) * lightAmt;
        col.a = 1.0f;

        return col;
    }

    private Color Bounce(Ray ray, int depth)
    {
        RaycastHit raycastHitInfo;
        if (Physics.Raycast(ray, out raycastHitInfo))
        {
            RayTraceRenderer comp = raycastHitInfo.collider.GetComponent<RayTraceRenderer>();
            if (comp != null)
                return comp.CalculateColor(ray, raycastHitInfo, depth - 1);
        }

        return Color.clear;
    }
}
