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
    }
}