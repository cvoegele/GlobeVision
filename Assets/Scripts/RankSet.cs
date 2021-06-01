namespace DefaultNamespace
{
    public class RankSet
    {
        public readonly float winrate;
        public readonly float popularity;
        public readonly Position position;
        public readonly Rank rank;
        
        public RankSet(float winrate, float popularity, Position position, Rank rank)
        {
            this.winrate = winrate;
            this.popularity = popularity;
            this.position = position;
            this.rank = rank;
        }
    }
}