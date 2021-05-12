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
        var localPosition = transform.position;
        // if (prevFramePos == null)
        // {
        //     prevFramePos = localPosition;
        //     return;
        // }
        // else
        // {
        //     if (!(prevFramePos == transform.position))
        //     {
                Vector3[] vertices = mesh.vertices;
             
                vertices[meshIndex] = localPosition;
                mesh.vertices = vertices;
                prevFramePos = localPosition;
        //     }
        // }
    }
}
