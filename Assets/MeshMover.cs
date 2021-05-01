using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMover : MonoBehaviour
{
    public int meshIndex;
    public Mesh mesh;
    private Vector3? prevFramePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (prevFramePos == null)
        {
            prevFramePos = transform.position;
            return;
        }
        else
        {
            if (!(prevFramePos == transform.position))
            {
                Vector3[] vertices = mesh.vertices;
                vertices[meshIndex] = transform.position;
                mesh.vertices = vertices;
                prevFramePos = transform.position;
            }
        }
    }
}
