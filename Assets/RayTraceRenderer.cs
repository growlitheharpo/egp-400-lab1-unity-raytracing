using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RayTraceRenderer : MonoBehaviour
{
    [SerializeField] protected float _bounceBias;
    [SerializeField] protected float _lightBias;

    protected static Color ambientLight = new Color(0.1f, 0.1f, 0.1f, 1.0f);

    private Light _worldLight;
    private Transform _lightTransform;

    private Mesh _mesh;
    private List<Vector3> _normals;
    private List<int> _tris;

    protected virtual void Awake()
    {
        _worldLight = FindObjectOfType<Light>();
        _lightTransform = _worldLight.transform;

        MeshCollider col = GetComponent<MeshCollider>();
        if (col == null)
            return;

        _mesh = col.sharedMesh;
        if (_mesh == null)
            return;

        _normals = new List<Vector3>();
        _tris = new List<int>();
        _mesh.GetNormals(_normals);
        _mesh.GetTriangles(_tris, 0);
    }

    protected Color CalculateLight(Vector3 hitPoint, Vector3 norm)
    {
        switch (_worldLight.type)
        {
            case LightType.Directional:
                return CalculateLightDirectional(hitPoint, norm) + ambientLight;
            case LightType.Spot:
            case LightType.Point:
            case LightType.Area:
                return CalculateLightPoint(hitPoint, norm) + ambientLight;
            default:
                return ambientLight;
        }
    }

    protected Color CalculateLightPoint(Vector3 hitPoint, Vector3 norm)
    {
        Vector3 toLight = _lightTransform.position - hitPoint;
        bool inShadow = Physics.Raycast(hitPoint + norm * _lightBias, toLight, toLight.magnitude);
        if (inShadow)
            return Color.black;

        float intensity = _worldLight.range * 0.25f / (hitPoint - _lightTransform.position).sqrMagnitude;

        return _worldLight.color * Vector3.Dot(toLight.normalized, norm) * intensity;
    }

    protected Color CalculateLightDirectional(Vector3 hitPoint, Vector3 norm)
    {
        Vector3 toLight = _worldLight.transform.forward;
        bool inShadow = Physics.Raycast(hitPoint + norm * _lightBias, -toLight);
        if (inShadow)
            return Color.black;

        return _worldLight.color * Vector3.Dot(-toLight, norm);
    }

    protected Vector3 GetInterpNormal(Vector3 bary, Vector3 hitNorm, int tri)
    {
        if (_tris == null)
            return hitNorm;

        Vector3 n0 = _normals[_tris[tri * 3 + 0]];
        Vector3 n1 = _normals[_tris[tri * 3 + 1]];
        Vector3 n2 = _normals[_tris[tri * 3 + 2]];
        return (n0 * bary.x + n1 * bary.y + n2 * bary.z).normalized;
    }

    public abstract Color CalculateColor(Ray originalRay, RaycastHit hitInfo, int depth);
}
