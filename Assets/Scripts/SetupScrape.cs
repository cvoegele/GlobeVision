using DefaultNamespace;
using UnityEngine;

public class SetupScrape : MonoBehaviour
{
    public GameObject sphereBuilderHolder;
    public float zeroValue;
    public float badValue;
    public float scale;
    public float sphereScale;
    
    void Start()
    {
        var set = ChampionGGScraper.GetAll();
        set.SortDataSetOnWinrate(Rank.PlatinumPlus);
        var builder = sphereBuilderHolder.GetComponent<SphereBuilder>();
        builder.Setup();
        builder.AssignDataSet(set, zeroValue, badValue, scale);
        sphereBuilderHolder.transform.localScale = new Vector3(sphereScale,sphereScale,sphereScale);
    }
}