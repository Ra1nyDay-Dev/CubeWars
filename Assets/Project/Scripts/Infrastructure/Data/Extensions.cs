using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Infrastructure.Data
{
    public static class Extensions
    {
        public static T With<T>(this T self, Action<T> set)
        {
            set.Invoke(self);
            return self;
        }
 
        public static T With<T>(this T self, Action<T> apply, Func<bool> when)
        {
            if (when())
                apply?.Invoke(self);
 
            return self;
        }
 
        public static T With<T>(this T self, Action<T> apply, bool when)
        {
            if (when)
                apply?.Invoke(self);
 
            return self;
        }
        
        public static Vector3 With(this Vector3 v, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(
                x ?? v.x,
                y ?? v.y,
                z ?? v.z
            );
        }
        
        public static List<T> Shuffle<T>(this List<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = Random.Range(0,n + 1);

                (list[k], list[n]) = (list[n], list[k]);
            }
            
            return list;
        }
    }
}