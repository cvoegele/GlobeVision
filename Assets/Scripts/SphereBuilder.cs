using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class SphereBuilder : MonoBehaviour
{
    private readonly List<Vector3> vertices = new List<Vector3>();
    private Dictionary<Vector3, GameObject> points = new Dictionary<Vector3, GameObject>();
    public int icosahedronResolution;
    public GameObject vertexPrefab;
    public Material startMaterial;
    public ChampionInformationSetter championInformationSetter;
    public Rank rank;
    public Position position;

    // Start is called before the first frame update
    public void Setup()
    {
        championInformationSetter.rank = rank;
        championInformationSetter.position = position;
        BuildIcosahedron(icosahedronResolution);
    }

    private GameObject CreatePoint(Vector3 pos)
    {
        GameObject point = Instantiate(vertexPrefab, transform);
        point.transform.localPosition = pos;
        point.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        // point.AddComponent<NearInteractionGrabbable>();
        // point.AddComponent<ObjectManipulator>();
        PointPositioner positioner = point.GetComponent<PointPositioner>();
        positioner.normalOnSphere = pos.normalized;
        positioner.connectedMeshes = new List<Mesh>();
        positioner.connectedMeshRenderers = new List<MeshRenderer>();
        positioner.indexPerConnectedMesh = new List<int>();
        positioner.initialLocalPosition = pos;
        points.Add(pos, point);

        return point;
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

            GameObject A = null, B = null, C = null;

            if (!vertices.Contains(a))
            {
                vertices.Add(a);
                A = CreatePoint(a);
            }

            if (!vertices.Contains(b))
            {
                vertices.Add(b);
                B = CreatePoint(b);
            }

            if (!vertices.Contains(c))
            {
                vertices.Add(c);
                C = CreatePoint(c);
            }

            GameObject meshHolder = new GameObject("MeshHolder");
            meshHolder.transform.position = transform.position;
            meshHolder.transform.parent = transform;

            Mesh mesh = new Mesh();

            mesh.vertices = new[] {a, b, c};
            mesh.normals = new[] {a.normalized, b.normalized, c.normalized};
            mesh.triangles = new[] {0, 1, 2};

            MeshFilter meshFilter = meshHolder.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = meshHolder.AddComponent<MeshRenderer>();
            meshRenderer.material = startMaterial;
            meshFilter.mesh = mesh;

            mesh.RecalculateBounds();

            if (A is null)
            {
                A = points[a];
            }

            var positionerA = A.GetComponent<PointPositioner>();
            positionerA.connectedMeshes.Add(mesh);
            positionerA.connectedMeshRenderers.Add(meshRenderer);
            positionerA.indexPerConnectedMesh.Add(0);

            if (B is null)
            {
                B = points[b];
            }

            var positionerB = B.GetComponent<PointPositioner>();
            positionerB.connectedMeshes.Add(mesh);
            positionerB.connectedMeshRenderers.Add(meshRenderer);
            positionerB.indexPerConnectedMesh.Add(1);

            if (C is null)
            {
                C = points[c];
            }

            var positionerC = C.GetComponent<PointPositioner>();
            positionerC.connectedMeshes.Add(mesh);
            positionerC.connectedMeshRenderers.Add(meshRenderer);
            positionerC.indexPerConnectedMesh.Add(2);
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

    public void AssignDataSet(DataSet dataSet, float zeroValue, float badValue, float scale)
    {
        if (points.Count < dataSet.Champions.Count)
        {
            Debug.LogError("There arent enough points to display all points");
        }

        List<KeyValuePair<Vector3, GameObject>> list = points.ToList();
        var comp = new Vector3Comparer();
        //list.Sort(((pair0, pair1) => comp.Compare(pair0.Key, pair1.Key)));
        int i = 0;

        foreach (var championPair in dataSet.Champions)
        {
            var champion = championPair.Value;
            var rankSet = champion.GetRankSet(Rank.All, Position.All);

            if (rankSet != null && i < points.Count)
            {
                var point = list[i];
                i++;
                PointPositioner positioner = point.Value.GetComponent<PointPositioner>();

                positioner.value = rankSet.winrate;
                positioner.zeroValue = zeroValue;
                positioner.badValue = badValue;
                positioner.scale = scale;
               

                SetData setData = point.Value.GetComponent<SetData>();
                setData.data = champion;
                setData.ChampionInformationSetter = championInformationSetter;
                point.Value.GetComponent<MeshRenderer>().material.mainTexture =
                    Resources.Load($"ChampionIcons/{champion.iconName}_0") as Texture;

                positioner.SetValue();
            }
        }

        //disable rest of points
        if (i < points.Count)
        {
            for (int j = i; j < points.Count; j++)
            {
                list[j].Value.SetActive(false);
            }
        }
    }
}