using System.Collections.Generic;

namespace Glee;

//TODO: utils subfolder

public static partial class Utils
{

    public class PairDictionary<T1, T2>()
    {
        private Dictionary<T1, T2> forward = [];
        private Dictionary<T2, T1> reverse = [];


        public void Add(T1 t1, T2 t2)
        {
            forward.Add(t1, t2);
            reverse.Add(t2, t1);
        }

        public void Add(T2 t2, T1 t1)
        {
            forward.Add(t1, t2);
            reverse.Add(t2, t1);
        }


        public void Forward(T1 t1, T2 t2)
        {
            Add(t1, t2);
        }

        public void Reverse(T2 t1, T1 t2)
        {
            Add(t2, t1);
        }


        public void Remove(T1 t1, T2 t2)
        {
            forward.Remove(t1);
            reverse.Remove(t2);
        }

        public void Remove(T2 t2, T1 t1)
        {
            forward.Remove(t1);
            reverse.Remove(t2);
        }

        public void RemoveForwards(T1 t1, T2 t2)
        {
            Remove(t1, t2);
        }

        public void RemoveReverse(T2 t1, T1 t2)
        {
            Remove(t1, t2);
        }

    }

}

