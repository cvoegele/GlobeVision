namespace DefaultNamespace
{
    public class Champion
    {
        public string name;

        public PercentSet[] rankSets;
        
        public float winrate;
        public float popularity;

        public Champion(string name, float winrate, float popularity, int rankSets)
        {
            this.name = name;
            this.winrate = winrate;
            this.popularity = popularity;
            this.rankSets = new PercentSet[rankSets];
        }

        public override string ToString()
        {
            return $"{name}: win: {winrate}, pop: {popularity}, {rankSets.Length}";
        }

        protected bool Equals(Champion other)
        {
            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Champion) obj);
        }

        public override int GetHashCode()
        {
            return (name != null ? name.GetHashCode() : 0);
        }
    }
}