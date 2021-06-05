using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
    public class DataSet
    {
        internal Dictionary<string, Champion> Champions { get; }

        private IEnumerable<RankSet> accessList;
        public IEnumerable<RankSet> AccessList =>
            !accessList.Any()
                ? accessList = Champions.Select(pair => pair.Value.GetRankSet(Rank.All, Position.All))
                : accessList;

        public DataSet()
        {
            Champions = new Dictionary<string, Champion>();
            accessList = new List<RankSet>();
        }
        
        public void SortDataSetOnWinrate(Rank rank)
        {
            //Champions.Sort((champ0, champ1) => (int) (champ0.rankSets[(int) rank].winrate - champ1.rankSets[(int) rank].winrate));
        }

        public void GroupDataSetByPosition() =>
            accessList = accessList.GroupBy(champion => champion.position).Select(x => x.Where(y => y.rank == Rank.All))
                .SelectMany(perGroup => perGroup);
    }
}