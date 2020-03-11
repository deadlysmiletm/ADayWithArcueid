using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;
using ParadoxEngine.Utilities;

using Object = System.Object;

namespace ParadoxEngine.Utilities.Parameters
{
    public interface IParamIn<in T>
    {
        void SuscribeValue(string name, ParamVariableType type, EngineGraph parentGraph, ParamAccessibility accessibility);
        void UnsuscribeValue(string name);
    }

    //public interface IConditionIn<in T>
    //{
    //    void SuscribeValue(string reference, EnumCompareType compareType, int valueType, T value);
    //    void UnsuscribeValue(string reference);
    //}
    
    public class ParamTuple<T>
    {
        [UnityEngine.SerializeField]
        public string Name;
        [UnityEngine.SerializeField]
        public int Type;
        [UnityEngine.SerializeField]
        public T Value;

        public ParamTuple(string name, int type, T value)
        {
            Name = name;
            Type = type;
            Value = value;
        }
    }

    [Serializable]
    public class TriggerTuple
    {
        public string ReferenceName;
        public UnityEvent Value;

        public TriggerTuple(string reference, UnityEvent value)
        {
            ReferenceName = reference;
            Value = value;
        }
    }

    [Serializable]
    public class ConditionalTuple
    {
        public string ReferenceName;
        public EnumCompareType CompareType;
        public ParamVariableType ValueType;
        public int ValueIndex;

        public ConditionalTuple(string reference, EnumCompareType compareType, ParamVariableType valueType, int valueIndex)
        {
            ReferenceName = reference;
            CompareType = compareType;
            ValueType = valueType;
            ValueIndex = valueIndex;
        }

        public override string ToString()
        {
            return "Name: " + ReferenceName + " : " + "Comparation: " + CompareType + " : " + "Type: " + ValueType + " : " + "Index: " + ValueIndex + ".";
        }
    }

    [Serializable]
    public class SetterTuple
    {
        [SerializeField]
        public string ReferenceName;

        [SerializeField]
        public EnumModType ModType;

        [SerializeField]
        public ParamVariableType ValueType;

        [SerializeField]
        public int ValueIndex;

        public SetterTuple(string reference, EnumModType compareType, ParamVariableType valueType, int valueIndex)
        {
            ReferenceName = reference;
            ModType = compareType;
            ValueType = valueType;
            ValueIndex = valueIndex;
        }

        public override string ToString()
        {
            return "Name: " + ReferenceName + " : " + "Comparation: " + ModType + " : " + "Type: " + ValueType + " : " + "Index: " + ValueIndex + ".";
        }
    }

    [Serializable]
    public class ReferenceParam : ParamTuple<int>
    {
        public int Index { get { return Value; }  set { Value = value; } }

        public ReferenceParam(string name, int type, int index) : base(name, type, index) { }
    }

    public enum ParamVariableType { None, Int, Float, Bool, String }
    public enum ParamAccessibility { IsGlobal, IsLocal };


    [Serializable]
    public class ParamReferenceVariable
    {
        public string Name;
        public int IndexInCollection;
        public ParamVariableType Type;

        public ParamAccessibility Accessibility;
        public EngineGraph EngineGraph;
        public bool IsPersistent;
    }

    [Serializable]
    public class ConditionParam : IEnumerable<(string Name, EnumCompareType CompareType, ParamVariableType VarType)>
    {
        [SerializeField] private List<ConditionalTuple> _dataReference;
        [SerializeField] private List<int> _intData;
        [SerializeField] private List<float> _floatData;
        [SerializeField] private List<bool> _boolData;
        [SerializeField] private List<string> _stringData;

        public int Count
        {
            get
            {
                InitializeData(ParamVariableType.None);

                return _dataReference.Count;
            }
        }
        
        public void SuscribeValue(string reference, EnumCompareType compareType, ParamVariableType valueType)
        {
            int index = -1;

            if (valueType == ParamVariableType.Int)
            {
                index = _intData.Count;
                _intData.Add(0);
            }

            else if (valueType == ParamVariableType.Float)
            {
                index = _floatData.Count;
                _floatData.Add(0f);
            }

            else if (valueType == ParamVariableType.Bool)
            {
                index = _boolData.Count;
                _boolData.Add(false);
            }

            else
            {
                index = _stringData.Count;
                _stringData.Add(default);
            }

            _dataReference.Add(new ConditionalTuple(reference, compareType, valueType, index));
        }

        public void UnsuscribeValue(string reference)
        {
            if (!_dataReference.Any(x => x.ReferenceName == reference))
                return;

            var data = _dataReference.Where(x => x.ReferenceName == reference).First();

            _dataReference.Remove(data);

            if (data.ValueType == ParamVariableType.Int)
                _intData.RemoveAt(data.ValueIndex);

            else if (data.ValueType == ParamVariableType.Float)
                _floatData.RemoveAt(data.ValueIndex);

            else if (data.ValueType == ParamVariableType.Bool)
                _boolData.RemoveAt(data.ValueIndex);

            else
                _stringData.RemoveAt(data.ValueIndex);

            foreach (var item in _dataReference)
            {
                if (data.ValueType == item.ValueType && item.ValueIndex >= data.ValueIndex)
                    item.ValueIndex--;
            }
        }

        public bool Contains(string name)
        {
            return _dataReference.Any(x => x.ReferenceName == name);
        }

        public int GetInt(string name) => _intData[_dataReference.Where(x => x.ReferenceName == name).First().ValueIndex];
        public float GetFloat(string name) => _floatData[_dataReference.Where(x => x.ReferenceName == name).First().ValueIndex];
        public bool GetBool(string name) => _boolData[_dataReference.Where(x => x.ReferenceName == name).First().ValueIndex];
        public string GetString(string name) => _stringData[_dataReference.Where(x => x.ReferenceName == name).First().ValueIndex];

        public void ChangeParameter(string oldName, string newName, ParamVariableType valueType)
        {
            var oldParam = _dataReference.Where(x => x.ReferenceName == oldName).First();

            if (oldParam.ValueType == ParamVariableType.Int)
                _intData.RemoveAt(oldParam.ValueIndex);

            else if (oldParam.ValueType == ParamVariableType.Float)
                _floatData.RemoveAt(oldParam.ValueIndex);

            else if (oldParam.ValueType == ParamVariableType.Bool)
                _boolData.RemoveAt(oldParam.ValueIndex);

            else
                _stringData.RemoveAt(oldParam.ValueIndex);

            var index = _dataReference.IndexOf(oldParam);
            _dataReference[index].ReferenceName = newName;
            _dataReference[index].ValueType = valueType;
            _dataReference[index].CompareType = EnumCompareType.Equal;

            int paramIndex = -1;

            if (valueType == ParamVariableType.Int)
            {
                _intData.Add(0);
                paramIndex = _intData.Count - 1;
            }


            else if (valueType == ParamVariableType.Float)
            {
                _floatData.Add(0f);
                paramIndex = _floatData.Count - 1;
            }

            else if (valueType == ParamVariableType.Bool)
            {
                _boolData.Add(false);
                paramIndex = _boolData.Count - 1;
            }

            else
            {
                _stringData.Add(default);
                paramIndex = _stringData.Count - 1;
            }


            _dataReference[index].ValueIndex = paramIndex;
        }

        public void UpdateName(string oldName, string newName)
        {
            var index = _dataReference.IndexOf(_dataReference.Where(x => x.ReferenceName == oldName).First());
            _dataReference[index].ReferenceName = newName;
        }

        public void UpdateValue(string name, int value) => _intData[_dataReference.Where(x => x.ValueType == ParamVariableType.Int).Where(x => x.ReferenceName == name).First().ValueIndex] = value;
        public void UpdateValue(string name, float value) => _floatData[_dataReference.Where(x => x.ValueType == ParamVariableType.Float).Where(x => x.ReferenceName == name).First().ValueIndex] = value;
        public void UpdateValue(string name, bool value) => _boolData[_dataReference.Where(x => x.ValueType == ParamVariableType.Bool).Where(x => x.ReferenceName == name).First().ValueIndex] = value;
        public void UpdateValue(string name, string value) => _stringData[_dataReference.Where(x => x.ValueType == ParamVariableType.String).Where(x => x.ReferenceName == name).First().ValueIndex] = value;

        public void UpdateCompareType(string Name, EnumCompareType compareType) => _dataReference.Where(x => x.ReferenceName == Name).First().CompareType = compareType;


        public IEnumerator<(string Name, EnumCompareType CompareType, ParamVariableType VarType)> GetEnumerator()
        {
            for (int i = 0; i < 5; i++)
                InitializeData((ParamVariableType)i);

            for (int i = 0; i < _dataReference.Count; i++)
            {
                var temp = _dataReference[i];
                yield return (temp.ReferenceName, temp.CompareType, temp.ValueType);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return GetEnumerator();
        }

        private void InitializeData(ParamVariableType type)
        {
            if (type == ParamVariableType.None)
            {
                if (_dataReference == null)
                    _dataReference = new List<ConditionalTuple>();
            }

            else if (type == ParamVariableType.Int)
            {
                if (_intData == null)
                    _intData = new List<int>();
            }

            else if (type == ParamVariableType.Float)
            {
                if (_floatData == null)
                    _floatData = new List<float>();
            }

            else if (type == ParamVariableType.Bool)
            {
                if (_boolData == null)
                    _boolData = new List<bool>();
            }

            else
            {
                if (_stringData == null)
                    _stringData = new List<string>();
            }
        }
    }

    [Serializable]
    public class SetterParam : IEnumerable<(string Name, EnumModType ModType, ParamVariableType VarType)>
    {
        [SerializeField] private List<SetterTuple> _dataReference;
        [SerializeField] private List<int> _intData;
        [SerializeField] private List<float> _floatData;
        [SerializeField] private List<bool> _boolData;
        [SerializeField] private List<string> _stringData;

        public int Count
        {
            get
            {
                InitializeData(ParamVariableType.None);

                return _dataReference.Count;
            }
        }

        public object GetValue(string name)
        {
            if (!_dataReference.Any(x => x.ReferenceName == name))
                return null;

            var data = _dataReference.Where(x => x.ReferenceName == name).First();

            if (data.ValueType == ParamVariableType.Int)
                return _intData[data.ValueIndex];

            else if (data.ValueType == ParamVariableType.Float)
                return _floatData[data.ValueIndex];

            else if (data.ValueType == ParamVariableType.Bool)
                return _boolData[data.ValueIndex];

            else if (data.ValueType == ParamVariableType.String)
                return _stringData[data.ValueIndex];

            return null;
        }

        public int GetInt(string name) => _intData[_dataReference.Where(x => x.ValueType == ParamVariableType.Int).Where(x => x.ReferenceName == name).First().ValueIndex];
        public float GetFloat(string name) => _floatData[_dataReference.Where(x => x.ValueType == ParamVariableType.Float).Where(x => x.ReferenceName == name).First().ValueIndex];
        public bool GetBool(string name) => _boolData[_dataReference.Where(x => x.ValueType == ParamVariableType.Bool).Where(x => x.ReferenceName == name).First().ValueIndex];
        public string GetString(string name) => _stringData[_dataReference.Where(x => x.ValueType == ParamVariableType.String).Where(x => x.ReferenceName == name).First().ValueIndex];

        public void SuscribeValue(string reference, EnumModType compareType, ParamVariableType valueType)
        {
            int index = -1;

            InitializeData(valueType);

            if (valueType == ParamVariableType.Int)
            {
                index = _intData.Count;
                _intData.Add(0);
            }

            else if (valueType == ParamVariableType.Float)
            {
                index = _floatData.Count;
                _floatData.Add(0f);
            }

            else if (valueType == ParamVariableType.Bool)
            {
                index = _boolData.Count;
                _boolData.Add(false);
            }

            else
            {
                index = _stringData.Count;
                _stringData.Add(default);
            }

            _dataReference.Add(new SetterTuple(reference, compareType, valueType, index));
        }

        public void UnsuscribeValue(string reference)
        {
            if (!_dataReference.Any(x => x.ReferenceName == reference))
                return;

            var data = _dataReference.Where(x => x.ReferenceName == reference).First();

            _dataReference.Remove(data);

            if (data.ValueType == ParamVariableType.Int)
                _intData.RemoveAt(data.ValueIndex);

            else if (data.ValueType == ParamVariableType.Float)
                _floatData.RemoveAt(data.ValueIndex);

            else if (data.ValueType == ParamVariableType.Bool)
                _boolData.RemoveAt(data.ValueIndex);

            else
                _stringData.RemoveAt(data.ValueIndex);


            foreach (var item in _dataReference)
            {
                if (data.ValueType == item.ValueType && item.ValueIndex >= data.ValueIndex)
                    item.ValueIndex--;
            }
        }

        public bool Contains(string name) => _dataReference.Any(x => x.ReferenceName == name);

        public void ChangeParameter(string oldName, string newName, ParamVariableType valueType)
        {
            var oldParam = _dataReference.Where(x => x.ReferenceName == oldName).First();

            if (oldParam.ValueType == ParamVariableType.Int)
                _intData.RemoveAt(oldParam.ValueIndex);

            else if (oldParam.ValueType == ParamVariableType.Float)
                _floatData.RemoveAt(oldParam.ValueIndex);

            else if (oldParam.ValueType == ParamVariableType.Bool)
                _boolData.RemoveAt(oldParam.ValueIndex);

            else
                _stringData.RemoveAt(oldParam.ValueIndex);


            var index = _dataReference.IndexOf(oldParam);
            _dataReference[index].ReferenceName = newName;
            _dataReference[index].ValueType = valueType;
            _dataReference[index].ModType = EnumModType.Swap;

            int paramIndex = -1;

            if (valueType == ParamVariableType.Int)
            {
                _intData.Add(0);
                paramIndex = _intData.Count - 1;
            }

            else if (valueType == ParamVariableType.Float)
            {
                _floatData.Add(0f);
                paramIndex = _floatData.Count - 1;
            }

            else if (valueType == ParamVariableType.Bool)
            {
                _boolData.Add(false);
                paramIndex = _boolData.Count - 1;
            }

            else
            {
                _stringData.Add(default);
                paramIndex = _stringData.Count - 1;
            }

            _dataReference[index].ValueIndex = paramIndex;
        }

        public void UpdateName(string oldName, string newName)
        {
            var index = _dataReference.IndexOf(_dataReference.Where(x => x.ReferenceName == oldName).First());
            _dataReference[index].ReferenceName = newName;
        }

        public void UpdateValue(string name, int value) => _intData[_dataReference.Where(x => x.ValueType == ParamVariableType.Int).Where(x => x.ReferenceName == name).First().ValueIndex] = value;
        public void UpdateValue(string name, float value) => _floatData[_dataReference.Where(x => x.ValueType == ParamVariableType.Float).Where(x => x.ReferenceName == name).First().ValueIndex] = value;
        public void UpdateValue(string name, bool value) => _boolData[_dataReference.Where(x => x.ValueType == ParamVariableType.Bool).Where(x => x.ReferenceName == name).First().ValueIndex] = value;
        public void UpdateValue(string name, string value) => _stringData[_dataReference.Where(x => x.ValueType == ParamVariableType.String).Where(x => x.ReferenceName == name).First().ValueIndex] = value;

        public void UpdateCompareType(string Name, EnumModType modType) => _dataReference.Where(x => x.ReferenceName == Name).First().ModType = modType;


        public IEnumerator<(string Name, EnumModType ModType, ParamVariableType VarType)> GetEnumerator()
        {
            for (int i = 0; i < 5; i++)
                InitializeData((ParamVariableType)i);

            for (int i = 0; i < _dataReference.Count; i++)
            {
                var data = _dataReference[i];

                switch (data.ValueType)
                {
                    case ParamVariableType.Int:
                        yield return (data.ReferenceName, data.ModType, data.ValueType);
                        break;
                    case ParamVariableType.Float:
                        yield return (data.ReferenceName, data.ModType, data.ValueType);
                        break;
                    case ParamVariableType.Bool:
                        yield return (data.ReferenceName, data.ModType, data.ValueType);
                        break;
                    case ParamVariableType.String:
                        yield return (data.ReferenceName, data.ModType, data.ValueType);
                        break;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return GetEnumerator();
        }

        private void InitializeData(ParamVariableType type)
        {
            switch (type)
            {
                case ParamVariableType.None:
                    if (_dataReference == null)
                        _dataReference = new List<SetterTuple>();
                    break;
                case ParamVariableType.Int:
                    if (_intData == null)
                        _intData = new List<int>();
                    break;
                case ParamVariableType.Float:
                    if (_floatData == null)
                        _floatData = new List<float>();
                    break;
                case ParamVariableType.Bool:
                    if (_boolData == null)
                        _boolData = new List<bool>();
                    break;
                case ParamVariableType.String:
                    if (_stringData == null)
                        _stringData = new List<string>();
                    break;
            }
        }
    }

    [Serializable]
    public class TriggerParam : IEnumerable<string>
    {
        private List<string> _data;


        public string this[int index] { get => _data[index]; set => _data[index] = value; }

        public int Count
        {
            get
            {
                if (_data == null)
                    _data = new List<string>();

                return _data.Count;
            }
        }

        public void SuscribeValue(string reference)
        {
            if (_data == null)
                _data = new List<string>();

            if (_data.Contains(reference))
                return;

            _data.Add(reference);
        }

        public void UnsuscribeValue(string reference)
        {
            if (_data == null || !_data.Contains(reference))
                return;

            _data.Remove(reference);
        }

        public void UnsuscribeValue(int index)
        {
            if (_data == null)
                return;

            _data.RemoveAt(index);
        }

        public void UpdateName(string oldName, string newName)
        {
            int index = _data.IndexOf(oldName);
            _data[index] = newName;
        }

        public IEnumerator<string> GetEnumerator()
        {
            for (int i = 0; i < _data.Count; i++)
                yield return _data[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return GetEnumerator();
        }
    }
}