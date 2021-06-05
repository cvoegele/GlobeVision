namespace DefaultNamespace
{
    public class RankSet
    {
        public readonly Champion champion;
        public readonly float winrate;
        public readonly float popularity;
        public readonly Position position;
        public readonly Rank rank;
        
        public RankSet(float winrate, float popularity, Position position, Rank rank, Champion champion)
        {
            this.winrate = winrate;
            this.popularity = popularity;
            this.position = position;
            this.rank = rank;
            this.champion = champion;
        }
    }
}