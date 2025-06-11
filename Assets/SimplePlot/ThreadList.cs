using System.Collections.Generic;
using UnityEngine;

namespace CavePeople
{
    public class ThreadList : MonoBehaviour
    {
        static Dictionary<string, ThreadList> threads = new(10); 

        public static ThreadList GetList(string name)
        {
            return threads[name];
        }

        public List<GameObject> objects = new();

        void Awake()
        {
            threads.Add(name, this);
        }

        void OnDestroy()
        {
            if (threads.Count > 0) threads.Clear();
        }
    }
}
