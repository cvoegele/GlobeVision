using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampionInformationSetter : MonoBehaviour
{
    public TextMeshPro championName;
    public RawImage championImage;
    public TextMeshPro winrateText;
    public TextMeshPro popularityText;
    public Rank rank;
    public Position position;

    public void SetInformation(Champion champion)
    {
        var image = Resources.Load($"ChampionIcons/{champion.iconName}_0") as Texture;
        championImage.texture = image;

        championName.text = champion.name;
        var rankSet = champion.GetRankSet(rank, position);
        winrateText.text = $"{rankSet.winrate:0.00}%";
        popularityText.text = $"{rankSet.popularity:0.00}%";
    }
}