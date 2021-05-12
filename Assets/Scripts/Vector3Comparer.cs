using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Vector3Comparer : Comparer<Vector3>
    {
        public override int Compare(Vector3 x, Vector3 y)
        {
            var eps = Mathf.Epsilon;
            //y is euquals
            // if (Math.Abs(x.y - y.y) < eps)
            // {
            //
            //     // z is equals
            //     if (Math.Abs(x.z - y.z) < eps)
            //     {
            //
            //         return (int) (x.x - y.x);
            //
            //     }
            //
            //     return (int) (x.z - y.z);
            // }

            return (int) (x.y - y.y);
        }
        
    }
}