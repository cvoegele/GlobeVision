﻿using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampionInformationSetter : MonoBehaviour
{
    public TextMeshPro championName;
    public RawImage championImage;
    public TextMeshPro winrateText;
    public TextMeshPro popularityText;

    public void SetInformation(Champion champion)
    {
        var image = Resources.Load($"ChampionIcons/{champion.iconName}_0") as Texture;
        championImage.texture = image;

        championName.text = champion.name;
        winrateText.text = $"{champion.winrate:0.00}%";
        popularityText.text = $"{champion.popularity:0.00}%";
    }
}