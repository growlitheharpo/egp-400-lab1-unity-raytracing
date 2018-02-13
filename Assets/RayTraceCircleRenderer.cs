using System.Collections.Generic;
using UnityEngine;

public class RayTraceCircleRenderer : RayTraceRenderer
{
    private Vector2 _circleCenter;
    private Mesh _mesh;

    private List<Vector3> _normals;
    private List<int> _tris;

    protected override void Awake()
    {
        base.Awake();
        _circleCenter = Vector2.one * 0.5f;
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
        Color fragColor = Color.white * lightAmt;

        float rad = 0.125f;
        if ((hitInfo.textureCoord - _circleCenter).sqrMagnitude < rad)
            fragColor = Color.red * lightAmt;

        fragColor.a = 1.0f;

        return fragColor;
    }
}
