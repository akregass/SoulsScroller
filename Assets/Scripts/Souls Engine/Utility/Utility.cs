using UnityEngine;
using System.Collections.Generic;
using System;
using MEC;

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

    public class Timer
    {
        float time;
        string message = "";
        public event Action TimerEvent;
        CoroutineHandle handle;

        bool active;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public Timer(float time)
        {
            this.time = time;
        }

        public Timer(float time, string message)
        {
            this.time = time;
            this.message = message;
        }

        public void Activate()
        {
            handle = Timing.RunCoroutine(Count());
            Active = true;
        }

        public void Deactivate()
        {
            Timing.KillCoroutines(handle);
            Active = false;
        }

        public IEnumerator<float> Count ()
        {
            yield return Timing.WaitForSeconds(time);

            Active = false;
            TimerEvent();
            if (message != "")
                Debug.Log(message);
        }
    }
}