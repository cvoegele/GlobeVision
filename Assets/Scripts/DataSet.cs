using System.Collections.Generic;

namespace DefaultNamespace
{
    public class DataSet
    {
        public List<Champion> Champions;

        public DataSet(List<Champion> champions)
        {
            Champions = champions;
        }

        public void SortDataSetOnWinrate(Rank rank)
        {
            Champions.Sort((champ0, champ1) => (int) (champ0.rankSets[(int) rank].winrate - champ1.rankSets[(int) rank].winrate));
        }
    }
}