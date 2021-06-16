using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SingleChampionPointManager : MonoBehaviour
{
    //value parameters
    private float value;
    private float zeroValue;
    private float badValue;
    private float scale;

    private Champion champion;

    public Champion Champion
    {
        get => champion;
        set
        {
            champion = value;
            GetComponent<MeshRenderer>().material.mainTexture =
                Resources.Load($"ChampionIcons/{champion.iconName}_0") as Texture;
        }
    }

    private Rank rank;
    private Position position;

    //visual parameters
    public Material[] materials;
    public Vector3 initialLocalPosition;
    public List<Mesh> connectedMeshes;
    public List<MeshRenderer> connectedMeshRenderers;
    public List<int> indexPerConnectedMesh;
    public Vector3 normalOnSphere;
    private bool isPartOfStatistic;
    private bool needsUpdate;

    public ChampionInformationSetter championInformationSetter;

    private void Awake()
    {
        UpdatePosition();
    }

    public void SelectRankAndPosition(Rank rank, Position position)
    {
        this.rank = rank;
        this.position = position;
        needsUpdate = true;
    }

    private void SetValueBasedOnRankAndPosition()
    {
        var rankSet = champion.GetRankSet(rank, position);
        if (rankSet != null)
        {
            isPartOfStatistic = true;
            value = rankSet.winrate;
            zeroValue = 50;
            badValue = 48;
            scale = 0.08f;
        }
        else
        {
            isPartOfStatistic = false;
        }

       
    }

    private void UpdatePosition()
    {
        Vector3 localPosition;

        if (isPartOfStatistic)
        {
            //set Position based on value
            var newPosition = (value - zeroValue) * scale * normalOnSphere;
            localPosition = initialLocalPosition + newPosition;
        }
        else
        {
            localPosition = initialLocalPosition;
        }

        transform.localPosition = localPosition;

        //update Position based on value
        for (int i = 0; i < connectedMeshes.Count; i++)
        {
            var mesh = connectedMeshes[i];
            var copiedVertices = mesh.vertices;
            copiedVertices[indexPerConnectedMesh[i]] = localPosition;
            mesh.vertices = copiedVertices;
        }

        //update color based on value
        foreach (var meshRenderer in connectedMeshRenderers)
        {
            if (value <= badValue)
            {
                meshRenderer.material = materials[1];
            }
            else if (value <= zeroValue)
            {
                meshRenderer.material = materials[0];
            }
            else
            {
                meshRenderer.material = materials[2];
            }
        }
    }

    public void Update()
    {
        if (needsUpdate)
        {
            SetValueBasedOnRankAndPosition();
            UpdatePosition();
            needsUpdate = false;
        }

        //rotate ball towards camera
        transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up);

        gameObject.GetComponent<MeshRenderer>().enabled = isPartOfStatistic;
    }

    public void DisplayInformationOnBoard()
    {
        if (champion != null)
        {
            championInformationSetter.SetInformation(champion);
        }
    }
}