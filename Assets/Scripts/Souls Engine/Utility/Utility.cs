using UnityEngine;
using System.Collections.Generic;
using System;
using MEC;

namespace SoulsEngine.Utility
{
    public class Utility
    {
        public const float PixelUnit = 1 / 16f;

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

        public static Vector2 SnapToPixelGrid(Vector2 pos)
        {
            float x = Mathf.Round(pos.x / PixelUnit) * PixelUnit;
            float y = Mathf.Round(pos.y / PixelUnit) * PixelUnit;

            return new Vector2(x, y);
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

        public CoroutineHandle Activate()
        {
            if (!Active)
            {
                handle = Timing.RunCoroutine(Run());
                Active = true;
                return handle;
            }
            else
                return Refresh();
        }

        public CoroutineHandle Refresh()
        {
            Timing.KillCoroutines(handle);
            handle = Timing.RunCoroutine(Run());
            return handle;
        }

        public void Deactivate()
        {
            Timing.KillCoroutines(handle);
            Active = false;
        }

        public IEnumerator<float> Run ()
        {
            yield return Timing.WaitForSeconds(time);

            Active = false;
            TimerEvent();
            if (message != "")
                Debug.Log(message);
        }
    }

    public class Heap<T> where T : IHeapItem<T>
    {

        T[] items;
        int elementCount;
        public int Count { get { return elementCount; }}

        public Heap(int size)
        {
            items = new T[size];
        }

        public void Add(T item)
        {
            item.Index = Count;
            items[Count] = item;
            SortUp(item);
            elementCount++;
        }

        public T RemoveFirst()
        {
            T item = items[0];
            elementCount--;
            items[0] = items[Count];
            items[0].Index = 0;
            SortDown(items[0]);

            return item;
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        public bool Contains(T item)
        {
            return Equals(items[item.Index], item);
        }

        public void SortUp(T item)
        {
            int parentIndex = (item.Index - 1) / 2;
            T parentItem = items[parentIndex];

            while (true)
            {
                if (item.CompareTo(parentItem) > 0)
                    Swap(item, parentItem);
                else
                    break;

                parentIndex = (item.Index - 1) / 2;
                parentItem = items[parentIndex];
            }
        }

        public void SortDown(T item)
        {
            int childIndexLeft = item.Index * 2 + 1;
            int childIndexRight = item.Index * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < Count)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < Count)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        swapIndex = childIndexRight;
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                    Swap(item, items[swapIndex]);
                else
                    return;
            }
            else
                return;
            
        }

        public void Swap(T itemA, T itemB)
        {
            items[itemA.Index] = itemB;
            items[itemB.Index] = itemA;

            itemA.Index ^= itemB.Index;
            itemB.Index ^= itemA.Index;
            itemA.Index ^= itemB.Index;
        }
    }

    public interface IHeapItem<T> : IComparable<T>
    {
        int Index { get; set; }
    }
    
}