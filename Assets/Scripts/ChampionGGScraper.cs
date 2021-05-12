using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using UnityEngine;

namespace DefaultNamespace
{
    public enum Rank : int
    {
        Iron = 0,
        Bronze = 1,
        Silver = 2,
        Gold = 3,
        Platinum = 4,
        Diamond = 5,
        PlatinumPlus = 6
    }    
    
    public class ChampionGGScraper
    {
        public static string[] urls = new[]
        {
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=iron&region=world",
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=bronze&region=world",
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=silver&region=world",
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=gold&region=world",
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=platinum&region=world",
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=diamond&region=world",
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=platinum_plus&region=world"
        };

        public static DataSet GetAll()
        {
            var datasetList = new List<Champion>();
            for (var index = 0; index < urls.Length; index++)
            {
                var url = urls[index];
                var rankList = Get(url);

                foreach (var champion in rankList)
                {
                    if (datasetList.Contains(champion))
                    {
                        var inDataSetChamp = datasetList.Find(ch => ch.Equals(champion));
                        inDataSetChamp.rankSets[index] = new PercentSet(champion.winrate, champion.popularity);
                    }
                    else
                    {
                        champion.rankSets[index] = new PercentSet(champion.winrate, champion.popularity);
                        datasetList.Add(champion);
                    }
                }
            }

            return new DataSet(datasetList);
        }

        public static List<Champion> Get(string rankUrl)
        {
            var url = rankUrl;
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var nodes = doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Contains("champion-row"));

            List<Champion> set = new List<Champion>();

            foreach (var node in nodes)
            {
                //champion
                var text = node.InnerText;
                string champName = "";
                string winRate = "";
                string pop = "";

                var splits = text.Split(new[] {"-"}, StringSplitOptions.None);

                champName = splits[1];

                champName = champName.Replace("jungle", "");
                champName = champName.Replace("mid", "");
                champName = champName.Replace("top", "");
                champName = champName.Replace("bot", "");
                champName = champName.Replace("support", "");
                champName = champName.Replace("tier", "");

                var percentageSplits = splits[2].Split(new[] {"%"}, StringSplitOptions.None);

                winRate = percentageSplits[0].Replace("N/A", "");
                pop = percentageSplits[2].Replace("N/A", "");

                var winRatePreDot = winRate.Split('.')[0];
                if (winRatePreDot.Length >= 3)
                {
                    winRate = winRate.Substring(1, winRate.Length - 2);
                }

                float winrateParsed;
                float popParsed;
                var worked = float.TryParse(winRate, out winrateParsed);
                var worked0 = float.TryParse(pop, out popParsed);
                
                worked = float.TryParse(winRate, out winrateParsed);
                worked0 = float.TryParse(pop, out popParsed);

                if (!worked || !worked0)
                {
                    Debug.LogError(winrateParsed);
                }

                var champ = new Champion(champName, winrateParsed, popParsed, urls.Length);
                set.Add(champ);
            }

            var names = set.Select(x => x.name).Distinct().ToList();
            List<Champion> distinctChamps = new List<Champion>();

            //remove duplicates
            foreach (var champion in set.Distinct())
            {
                if (names.Contains(champion.name))
                {
                    int otherCounter = 1;
                    float fullWinrate = 0;
                    float fullPop = 0;

                    fullPop += champion.popularity;
                    fullWinrate += champion.winrate;

                    foreach (var champion1 in set.ToList())
                    {
                        if (champion != champion1 && champion.name == champion1.name)
                        {
                            otherCounter++;
                            fullPop += champion1.popularity;
                            fullWinrate += champion1.winrate;
                        }
                    }

                    names.Remove(champion.name);

                    fullPop /= otherCounter;
                    fullWinrate /= otherCounter;

                    var champ = new Champion(champion.name, fullWinrate, fullPop, urls.Length);
                    distinctChamps.Add(champ);
                }
            }

            return distinctChamps;
        }
    }
}