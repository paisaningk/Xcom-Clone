using System;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME/test", order = 0)]
    [Serializable]
    public class GameDataObject : ScriptableObject
    {
        [SerializeField] public List<A> myTest;
    }

    [Serializable]
    public struct A
    {
        public string name;
        public string b;
        public int c;
    }
}