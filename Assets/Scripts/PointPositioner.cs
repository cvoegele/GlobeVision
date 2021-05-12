﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPositioner : MonoBehaviour
{
    public List<Mesh> connectedMeshes;
    public List<MeshRenderer> connectedMeshRenderers;
    public List<int> indexPerConnectedMesh;
    public Vector3 normalOnSphere;
    public float value;
    public float zeroValue;
    public float badValue;
    public float scale;

    public Material[] materials;

    private Vector3 initialLocalPosition;
    private float prevValue;

    private void Awake()
    {
        initialLocalPosition = transform.localPosition;
        prevValue = value;
    }

    private void Update()
    {
        if (Math.Abs(prevValue - value) > 0.001)
        {
            //set Position based on value
            var newPosition = (value - zeroValue) * scale * normalOnSphere;
            transform.localPosition = initialLocalPosition + newPosition;

            //update Position based on value
            for (int i = 0; i < connectedMeshes.Count; i++)
            {
                var mesh = connectedMeshes[i];
                var copiedVertices = mesh.vertices;
                copiedVertices[indexPerConnectedMesh[i]] = initialLocalPosition + newPosition;
                mesh.vertices = copiedVertices;
            }

            //update color based on value

            foreach (var meshRenderer in connectedMeshRenderers)
            {
                if (value <= badValue)
                {
                    meshRenderer.material = materials[1];
                } else  if (value <= zeroValue)
                {
                    meshRenderer.material = materials[0];
                }
                else
                {
                    meshRenderer.material = materials[2];
                }
            }

            prevValue = value;
        }
    }
}