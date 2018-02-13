using System.Collections.Generic;
using UnityEngine;

public class RayTraceColorRenderer : RayTraceRenderer
{
    [SerializeField] private Color _color;
    private Mesh _mesh;

    private List<Vector3> _normals;
    private List<int> _tris;

    protected override void Awake()
    {
        base.Awake();
        _mesh = GetComponent<MeshCollider>().sharedMesh;

        _normals = new List<Vector3>();
        _tris = new List<int>();
        _mesh.GetNormals(_normals);
        _mesh.GetTriangles(_tris, 0);
    }

    public override Color CalculateColor(RaycastHit hitInfo)
    {
        Vector3 interpNormal = GetInterpNormal(hitInfo.barycentricCoordinate, _tris, _normals, hitInfo.triangleIndex);
        Color lightAmt = CalculateLight(hitInfo.point, interpNormal);

        Color col = _color * lightAmt;

        col.a = 1.0f;
        return col;
    }
}
