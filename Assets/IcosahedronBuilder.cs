using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class IcosahedronBuilder : MonoBehaviour
{
    private readonly List<Vector3> vertices = new List<Vector3>();
    private Dictionary<Vector3, GameObject> points = new Dictionary<Vector3, GameObject>();
    public int icosahedronResolution;
    public Material meshMaterial;

    // Start is called before the first frame update
    void Start()
    {
        BuildIcosahedron(icosahedronResolution);

        foreach (var vertex in vertices)
        {
        }
    }

    private GameObject CreatePoint(Vector3 pos)
    {
        GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        point.transform.parent = transform;
        point.transform.localPosition = pos;
        point.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        point.AddComponent<NearInteractionGrabbable>();
        point.AddComponent<ObjectManipulator>();

        points.Add(pos, point);

        return point;
    }

    private void OnDrawGizmos()
    {
        foreach (var vertex in vertices)
        {
            Gizmos.DrawSphere(vertex, 0.1f);
        }
    }

    private void BuildIcosahedron(int depth)
    {
        const float x = 0.525731112119133606f;
        const float z = 0.850650808352039932f;

        var startVectors = new[]
        {
            new Vector3(-x, 0.0f, z),
            new Vector3(x, 0.0f, z),
            new Vector3(-x, 0.0f, -z),
            new Vector3(x, 0.0f, -z),
            new Vector3(0.0f, z, x),
            new Vector3(0.0f, z, -x),
            new Vector3(0.0f, -z, x),
            new Vector3(0.0f, -z, -x),
            new Vector3(z, x, 0.0f),
            new Vector3(-z, x, 0.0f),
            new Vector3(z, -x, 0.0f),
            new Vector3(-z, -x, 0.0f),
        };

        var indices = new[]
        {
            new Vector3Int(0, 1, 4),
            new Vector3Int(0, 4, 9),
            new Vector3Int(9, 4, 5),
            new Vector3Int(4, 8, 5),
            new Vector3Int(4, 1, 8),
            new Vector3Int(8, 1, 10),
            new Vector3Int(8, 10, 3),
            new Vector3Int(5, 8, 3),
            new Vector3Int(5, 3, 2),
            new Vector3Int(2, 3, 7),
            new Vector3Int(7, 3, 10),
            new Vector3Int(7, 10, 6),
            new Vector3Int(7, 6, 11),
            new Vector3Int(11, 6, 0),
            new Vector3Int(0, 6, 1),
            new Vector3Int(6, 10, 1),
            new Vector3Int(9, 11, 0),
            new Vector3Int(9, 2, 11),
            new Vector3Int(9, 5, 2),
            new Vector3Int(7, 11, 2),
        };

        for (int i = 0; i < indices.Length; i++)
        {
            var A = startVectors[indices[i].x];
            var B = startVectors[indices[i].y];
            var C = startVectors[indices[i].z];

            SubdivideIcosahedron(A, B, C, depth);
        }
    }

    private void SubdivideIcosahedron(Vector3 a, Vector3 b, Vector3 c, int depth)
    {
        if (depth == 0)
        {
            //at this point we know how the triangle is formed

            if (!vertices.Contains(a))
            {
                vertices.Add(a);
                CreatePoint(a);
            }


            if (!vertices.Contains(b))
            {
                vertices.Add(b);
                CreatePoint(b);
            }

            if (!vertices.Contains(c))
            {
                vertices.Add(c);
                CreatePoint(c);
            }

            var parentPos = transform.position;

            GameObject meshHolder = new GameObject();
            meshHolder.transform.parent = transform;

            Mesh mesh = new Mesh();
            MeshFilter meshFilter = meshHolder.AddComponent<MeshFilter>();
            MeshRenderer renderer = meshHolder.AddComponent<MeshRenderer>();
            renderer.material = meshMaterial;
            meshFilter.mesh = mesh;
            mesh.vertices = new[] {parentPos + a, parentPos + b, parentPos + c};
            mesh.normals = new[] {a.normalized, b.normalized, c.normalized};
            mesh.triangles = new[] {0, 1, 2};

            var aPoint = points[a];
            MeshMover aMeshMover = aPoint.AddComponent<MeshMover>();
            aMeshMover.mesh = mesh;
            aMeshMover.meshIndex = 0;

            var bPoint = points[b];
            MeshMover bMeshMover = bPoint.AddComponent<MeshMover>();
            bMeshMover.mesh = mesh;
            bMeshMover.meshIndex = 1; 

            var cPoint = points[c];
            MeshMover cMeshMover = cPoint.AddComponent<MeshMover>();
            cMeshMover.mesh = mesh;
            cMeshMover.meshIndex = 2;
        }
        else
        {
            var d = a + b;
            var e = b + c;
            var f = a + c;

            d = d.normalized;
            e = e.normalized;
            f = f.normalized;

            SubdivideIcosahedron(a, d, f, depth - 1);
            SubdivideIcosahedron(b, e, d, depth - 1);
            SubdivideIcosahedron(c, f, e, depth - 1);
            SubdivideIcosahedron(d, e, f, depth - 1);
        }
    }
}