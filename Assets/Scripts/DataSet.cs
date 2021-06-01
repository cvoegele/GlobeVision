using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class DataSet
    {
        public Dictionary<string, Champion> Champions { get; }
        
        public DataSet()
        {
            Champions = new Dictionary<string, Champion>();
        }
        
        public void SortDataSetOnWinrate(Rank rank)
        {
            //Champions.Sort((champ0, champ1) => (int) (champ0.rankSets[(int) rank].winrate - champ1.rankSets[(int) rank].winrate));
        }
    }
}