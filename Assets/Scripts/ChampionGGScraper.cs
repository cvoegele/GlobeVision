using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using UnityEngine;

namespace DefaultNamespace
{
    public static class ChampionGgScraper
    {
        private static readonly string[] Urls = new[]
        {
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=iron&region=world",
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=bronze&region=world",
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=silver&region=world",
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=gold&region=world",
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=platinum&region=world",
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=diamond&region=world",
            "https://champion.gg/statistics/overview?queue=ranked-solo-duo&rank=platinum_plus&region=world"
        };

        private static DataSet _dataSet;

        public static DataSet GetAll()
        {
            _dataSet = new DataSet();

            for (var index = 0; index < Urls.Length; index++)
            {
                Get(Urls[index], index);
            }

            CalculateCombined();

            return _dataSet;
        }

        private static void CalculateCombined()
        {
            foreach (var dataSetChampion in _dataSet.Champions)
            {
                var winrateSum = 0f;
                var popularitySum = 0f;

                foreach (var valueRankSet in dataSetChampion.Value.rankSets)
                {
                    winrateSum += valueRankSet.Value.winrate;
                    popularitySum += valueRankSet.Value.popularity;
                }

                winrateSum /= dataSetChampion.Value.rankSets.Count;
                popularitySum /= dataSetChampion.Value.rankSets.Count;

                var rankSet = new RankSet(winrateSum, popularitySum, Position.All, Rank.All);
                dataSetChampion.Value.rankSets.Add(new Tuple<Rank, Position>(Rank.All, Position.All), rankSet);
            }
        }

        public static void Get(string rankUrl, int rankIndex)
        {
            var rank = (Rank) rankIndex;

            var url = rankUrl;
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var nodes = doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Contains("champion-row"));

            foreach (var node in nodes)
            {
                //champion
                var text = node.InnerText;
                var champName = "";
                var winRate = "";
                var pop = "";

                var splits = text.Split(new[] {"-"}, StringSplitOptions.None);

                champName = splits[1];

                var position = FindPosition(champName);

                champName = champName.Replace("jungle", "");
                champName = champName.Replace("mid", "");
                champName = champName.Replace("top", "");
                champName = champName.Replace("bot", "");
                champName = champName.Replace("support", "");
                champName = champName.Replace("tier", "");
                champName = champName.Replace("#x27;", "'");
                champName = champName.Replace("amp;", "&");

                var percentageSplits = splits[2].Split(new[] {"%"}, StringSplitOptions.None);

                winRate = percentageSplits[0].Replace("N/A", "");
                pop = percentageSplits[2].Replace("N/A", "");

                var winRatePreDot = winRate.Split('.')[0];
                if (winRatePreDot.Length >= 3)
                {
                    winRate = winRate.Substring(1, winRate.Length - 2);
                }

                float.TryParse(winRate, out var winrateParsed);
                float.TryParse(pop, out var popParsed);

                var rankSet = new RankSet(winrateParsed, popParsed, position, rank);

                if (_dataSet.Champions.ContainsKey(champName))
                {
                    var champion = _dataSet.Champions[champName];
                    champion.rankSets.Add(new Tuple<Rank, Position>(rank, position), rankSet);
                }
                else
                {
                    var champion = new Champion(champName);
                    champion.rankSets.Add(new Tuple<Rank, Position>(rank, position), rankSet);
                    _dataSet.Champions.Add(champName, champion);
                }
            }

            // var names = set.Select(x => x.name).Distinct().ToList();
            // List<Champion> distinctChamps = new List<Champion>();
            //
            // //remove duplicates
            // foreach (var champion in set.Distinct())
            // {
            //     if (names.Contains(champion.name))
            //     {
            //         int otherCounter = 1;
            //         float fullWinrate = 0;
            //         float fullPop = 0;
            //
            //         fullPop += champion.rankSets[rankIndex].popularity;
            //         fullWinrate += champion.rankSets[rankIndex].winrate;
            //
            //         foreach (var champion1 in set.ToList())
            //         {
            //             if (champion != champion1 && champion.name == champion1.name)
            //             {
            //                 otherCounter++;
            //                 fullPop += champion1.rankSets[rankIndex].popularity;
            //                 fullWinrate += champion1.rankSets[rankIndex].winrate;
            //             }
            //         }
            //
            //         names.Remove(champion.name);
            //
            //         fullPop /= otherCounter;
            //         fullWinrate /= otherCounter;
            //
            //         var champ = new Champion(champion.name, fullWinrate, fullPop, urls.Length);
            //         distinctChamps.Add(champ);
            //     }
            // }
        }

        private static Position FindPosition(string champName)
        {
            if (champName.Contains("top"))
            {
                return Position.Top;
            }

            if (champName.Contains("jungle"))
            {
                return Position.Jungle;
            }

            if (champName.Contains("mid"))
            {
                return Position.Mid;
            }

            if (champName.Contains("bot"))
            {
                return Position.Bottom;
            }

            if (champName.Contains("support"))
            {
                return Position.Support;
            }

            return Position.Unknown;
        }
    }
}