using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;

public class Champion
{
    public readonly string name;
    public readonly string iconName;

    public readonly Dictionary<Tuple<Rank, Position>, RankSet> rankSets;
    
    public Champion(string name)
    {
        this.name = name;
        rankSets = new Dictionary<Tuple<Rank, Position>, RankSet>();
        
        iconName = name.Replace(" ", "");
        iconName = iconName.Replace("&", "");
        iconName = iconName.Replace("\'", "");
    }

    public RankSet GetRankSet(Rank rank, Position position)
    {
        return rankSets.FirstOrDefault(pair => Equals(pair.Key, new Tuple<Rank, Position>(rank, position))).Value;
    }
    
    public override string ToString()
    {
        return $"{name}: has {rankSets.Count} many rank sets";
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