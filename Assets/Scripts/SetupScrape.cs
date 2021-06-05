using DefaultNamespace;
using UnityEngine;

public class SetupScrape : MonoBehaviour
{
    public GameObject sphereBuilderHolder;
    public float sphereScale;
    
    void Start()
    {
        var set = ChampionGgScraper.GetAll();
        var builder = sphereBuilderHolder.GetComponent<DataSphere>();
        builder.Setup();
        builder.AssignDataSet(set);
        sphereBuilderHolder.transform.localScale = new Vector3(sphereScale,sphereScale,sphereScale);
    }
}