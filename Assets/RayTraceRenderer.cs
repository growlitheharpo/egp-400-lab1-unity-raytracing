using System.Collections.Generic;
using UnityEngine;

public abstract class RayTraceRenderer : MonoBehaviour
{
    private Light _worldLight;
    private Transform _lightTransform;

    protected virtual void Awake()
    {
        _worldLight = FindObjectOfType<Light>();
        _lightTransform = _worldLight.transform;
    }

    protected Color CalculateLight(Vector3 hitPoint, Vector3 norm)
    {
        return CalculateLightPoint(hitPoint, norm);
    }

    protected Color CalculateLightPoint(Vector3 hitPoint, Vector3 norm)
    {
        var toLight = _lightTransform.position - hitPoint;
        var inShadow = Physics.Raycast(hitPoint, toLight, toLight.magnitude);
        if (inShadow)
            return Color.black;

        float intensity = _worldLight.range * 0.25f / (hitPoint - _lightTransform.position).sqrMagnitude;

        return _worldLight.color * Vector3.Dot(toLight.normalized, norm) * intensity;
    }

    protected Color CalculateLightDirectional(Vector3 hitPoint, Vector3 norm)
    {
        Vector3 toLight = _worldLight.transform.forward;
        var inShadow = Physics.Raycast(hitPoint, -toLight);
        if (inShadow)
            return Color.black;

        return _worldLight.color * Vector3.Dot(-toLight, norm);
    }

    protected Vector3 GetInterpNormal(Vector3 bary, List<int> tris, List<Vector3> normals, int tri)
    {
        Vector3 n0 = normals[tris[tri * 3 + 0]];
        Vector3 n1 = normals[tris[tri * 3 + 1]];
        Vector3 n2 = normals[tris[tri * 3 + 2]];
        return (n0 * bary.x + n1 * bary.y + n2 * bary.z).normalized;
    }

    public abstract Color CalculateColor(RaycastHit hitInfo);
}
