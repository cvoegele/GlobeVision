using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using HtmlAgilityPack;

public class Scrape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        var set = ChampionGGScraper.GetAll();

        //set.Champions.ForEach(Debug.Log);
        
    }

}
