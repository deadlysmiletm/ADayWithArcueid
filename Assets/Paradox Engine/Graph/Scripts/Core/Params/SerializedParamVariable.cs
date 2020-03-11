using System;
using UnityEngine.Events;


namespace ParadoxEngine.Utilities.Parameters
{
    [Serializable]
    public class SerializedParamVariable<T>
    {
        public string Name;
        public T Value;
    }

    [Serializable]
    public class IntSerializedParamVariable : SerializedParamVariable<int> { }

    [Serializable]
    public class FloatSerializedParamVariable : SerializedParamVariable<float> { }

    [Serializable]
    public class BoolSerializedParamVaraible : SerializedParamVariable<bool> { }

    [Serializable]
    public class StringSerializedParamVariable : SerializedParamVariable<string> { }

    [Serializable]
    public class EventSerializedParamVariable : SerializedParamVariable<UnityEvent> { }
}