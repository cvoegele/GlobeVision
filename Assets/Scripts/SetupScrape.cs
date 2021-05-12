using DefaultNamespace;
using UnityEngine;

public class SetupScrape : MonoBehaviour
{
    public GameObject sphereBuilderHolder;
    public float zeroValue;
    public float badValue;
    public float scale;
    
    void Start()
    {
        var set = ChampionGGScraper.GetAll();
        set.SortDataSetOnWinrate(Rank.PlatinumPlus);
        var builder = sphereBuilderHolder.GetComponent<SphereBuilder>();
        builder.Setup();
        builder.AssignDataSet(set, Rank.PlatinumPlus, zeroValue, badValue, scale);
    }
}