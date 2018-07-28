using UnityEngine;
using System.Collections.Generic;

namespace SoulsEngine.Utility
{
    public class Utility
    {
        public static T GetComponentInChildren<T>(GameObject __gameObject)where T:Component
        {
            var array = __gameObject.GetComponentsInChildren<T>();

            foreach(T item in array)
            {
                if(item.gameObject.GetInstanceID() != __gameObject.GetInstanceID())
                {
                    return item;
                }
            }

            return null;
        }

        public static List<T> GetComponentsInChildren<T>(GameObject __gameObject) where T:Component
        {
            var list = new List<T>();
            var array = __gameObject.GetComponentsInChildren<T>();

            foreach(T item in array)
            {
                if(item.gameObject.GetInstanceID() != __gameObject.GetInstanceID())
                {
                    list.Add(item);
                }
            }

            return list ?? null;
        }
    }
}