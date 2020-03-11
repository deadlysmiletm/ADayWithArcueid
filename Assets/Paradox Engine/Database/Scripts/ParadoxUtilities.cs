using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ParadoxEngine.Utilities;

namespace ParadoxEngine
{
    public enum RolPriorityEnum
    {
        Primary,
        Secondary,
        Thirthy,
        Forthy
    }

    public enum CustomDataHolderEnum
    {
        Character,
        Location,
        Soundtrack
    }

    public enum SoundChannelEnum
    {
        Music,
        SoundFX,
        Voice
    }

    [Serializable]
    public class ParadoxTuple<T1, T2>
    {
        [SerializeField]
        public T1 Item1;
        [SerializeField]
        public T2 Item2;
        
        public ParadoxTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    [Serializable]
    public class ParadoxTuple<T1, T2, T3>
    {
        [SerializeField]
        public T1 Item1;
        [SerializeField]
        public T2 Item2;
        [SerializeField]
        public T3 Item3;

        public ParadoxTuple(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }
    }

    [Serializable]
    public class ParadoxTuple<T1, T2, T3, T4>
    {
        [SerializeField]
        public T1 Item1;
        [SerializeField]
        public T2 Item2;
        [SerializeField]
        public T3 Item3;
        [SerializeField]
        public T4 Item4;

        public ParadoxTuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }
    }

    [Serializable]
    public class TupleSprite : ParadoxTuple<string, Sprite>
    {
        public TupleSprite(string item1, Sprite item2) : base(item1, item2) { }
    }

    [Serializable]
    public class TupleAnimation : ParadoxTuple<string, AnimationClip>
    {
        public TupleAnimation(string item1, AnimationClip item2) : base(item1, item2) { }
    }

    [Serializable]
    public class TupleSound : ParadoxTuple<string, AudioClip>
    {
        public TupleSound(string item1, AudioClip item2) : base(item1, item2) { }
    }

    [Serializable]
    public class TupleObject : ParadoxTuple<string, UnityEngine.Object>
    {
        public TupleObject(string item1, UnityEngine.Object item2) : base(item1, item2) { }
    }

    [Serializable]
    public class TupleSystemObject : ParadoxTuple<string, System.Object>
    {
        public TupleSystemObject(string item1, System.Object item2) : base(item1, item2) { }
    }

    public abstract class PEData : ScriptableObject
    {
        [SerializeField]
        protected string _name;

        public string GetIdentificator() { return _name; }

        public abstract void SetIdentificator(string name);
    }
}