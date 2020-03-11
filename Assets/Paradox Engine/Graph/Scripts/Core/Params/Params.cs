using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using ParadoxFramework.IO;

using Object = System.Object;

namespace ParadoxEngine.Utilities.Parameters
{
    [CreateAssetMenu(menuName = "Paradox Engine/Parameter Setting")]
    public class Params : ScriptableObject, IEnumerable<(string Name, ParamVariableType Type, ParamAccessibility access, EngineGraph graph, bool IsPersistent)>
    {
        public List<ParamReferenceVariable> referenceData = new List<ParamReferenceVariable>();
        public List<IntSerializedParamVariable> intData = new List<IntSerializedParamVariable>();
        public List<FloatSerializedParamVariable> floatData = new List<FloatSerializedParamVariable>();
        public List<BoolSerializedParamVaraible> boolData = new List<BoolSerializedParamVaraible>();
        public List<StringSerializedParamVariable> stringData = new List<StringSerializedParamVariable>();
        
        public ParamReferenceVariable this[string name] => referenceData.Where(x => x.Name == name).First();

        public int Count
        {
            get
            {
                InitializeData();
                return referenceData.Count;
            }
        }

        public int IndexOf(string name) => referenceData.IndexOf(referenceData.Where(x => x.Name == name).First());

        public void SuscribeValue(string name, ParamVariableType type, EngineGraph parentGraph, ParamAccessibility accessibility)
        {
            int index = 0;

            if (referenceData.Any(x => x.Name == name))
            {
                string tempName = name.Replace("Parameter", "");
                int tempIndex = int.Parse(tempName);
                SuscribeValue("Parameter" + (tempIndex + 1), type, parentGraph, accessibility);
                return;
            }

            if (type == ParamVariableType.Int)
            {
                intData.Add(new IntSerializedParamVariable() { Name = name, Value = 0 });
                index = intData.Count - 1;
            }

            else if (type == ParamVariableType.Float)
            {
                floatData.Add(new FloatSerializedParamVariable() { Name = name, Value = 0f });
                index = floatData.Count - 1;
            }

            else if (type == ParamVariableType.String)
            {
                stringData.Add(new StringSerializedParamVariable() { Name = name, Value = default });
                index = stringData.Count - 1;
            }

            else
            {
                boolData.Add(new BoolSerializedParamVaraible() { Name = name, Value = false });
                index = boolData.Count - 1;
            }

            referenceData.Add(new ParamReferenceVariable { Name = name, Type = type, Accessibility = accessibility, EngineGraph = parentGraph, IndexInCollection = index, IsPersistent = false });

#if UNITY_EDITOR
            Dirty();
#endif
        }

        public void UpdateValue(string name, int newValue)
        {
            var index = referenceData.Where(x => x.Type == ParamVariableType.Int).Where(x => x.Name == name).First().IndexInCollection;
            intData[index].Value = newValue;
        }
        public void UpdateValue(string name, float newValue)
        {
            var index = referenceData.Where(x => x.Type == ParamVariableType.Float).Where(x => x.Name == name).First().IndexInCollection;
            floatData[index].Value = newValue;
        }
        public void UpdateValue(string name, bool newValue)
        {
            var index = referenceData.Where(x => x.Type == ParamVariableType.Bool).Where(x => x.Name == name).First().IndexInCollection;
            boolData[index].Value = newValue;
        }
        public void UpdateValue(string name, string newValue)
        {
            var index = referenceData.Where(x => x.Type == ParamVariableType.String).Where(x => x.Name == name).First().IndexInCollection;
            stringData[index].Value = newValue;
        }


        public void UpdateName(string name, string newName)
        {
            var temp = referenceData.Where(x => x.Name == name).First();

            if (temp.Type == ParamVariableType.Int)
                intData[temp.IndexInCollection].Name = newName;

            else if (temp.Type == ParamVariableType.Float)
                floatData[temp.IndexInCollection].Name = newName;

            else if (temp.Type == ParamVariableType.Bool)
                boolData[temp.IndexInCollection].Name = newName;

            else
                stringData[temp.IndexInCollection].Name = newName;

            temp.Name = newName;

#if UNITY_EDITOR
            Dirty();
#endif
        }


        public void SetAsPersistance(string name, bool value) => referenceData.Where(x => x.Name == name).First().IsPersistent = value;

        public void UnsuscribeValue(string name)
        {
            var temp = referenceData.Where(x => x.Name == name).First();
            referenceData.Remove(temp);

            if (temp.Type == ParamVariableType.Int)
            {
                intData.RemoveAt(temp.IndexInCollection);

                foreach (var item in referenceData.Where(x => x.Type == ParamVariableType.Int))
                    item.IndexInCollection = intData.IndexOf(intData.Where(x => x.Name == item.Name).First());
            }

            else if (temp.Type == ParamVariableType.Float)
            {
                floatData.RemoveAt(temp.IndexInCollection);

                foreach (var item in referenceData.Where(x => x.Type == ParamVariableType.Float))
                    item.IndexInCollection = floatData.IndexOf(floatData.Where(x => x.Name == item.Name).First());
            }

            else if (temp.Type == ParamVariableType.String)
            {
                stringData.RemoveAt(temp.IndexInCollection);

                foreach (var item in referenceData.Where(x => x.Type == ParamVariableType.String))
                    item.IndexInCollection = stringData.IndexOf(stringData.Where(x => x.Name == item.Name).First());
            }

            else
            {
                boolData.RemoveAt(temp.IndexInCollection);

                foreach (var item in referenceData.Where(x => x.Type == ParamVariableType.Bool))
                    item.IndexInCollection = boolData.IndexOf(boolData.Where(x => x.Name == item.Name).First());
            }

#if UNITY_EDITOR
            Dirty();
#endif
        }

        public IntSerializedParamVariable GetInt(string name) => intData[referenceData.Where(x => x.Type == ParamVariableType.Int).Where(x => x.Name == name).First().IndexInCollection];
        public IntSerializedParamVariable GetInt(int index) => intData[index];

        public FloatSerializedParamVariable GetFloat(string name) => floatData[referenceData.Where(x => x.Type == ParamVariableType.Float).Where(x => x.Name == name).First().IndexInCollection];
        public FloatSerializedParamVariable GetFloat(int index) => floatData[index];

        public BoolSerializedParamVaraible GetBool(string name) => boolData[referenceData.Where(x => x.Type == ParamVariableType.Bool).Where(x => x.Name == name).First().IndexInCollection];
        public BoolSerializedParamVaraible GetBool(int index) => boolData[index];

        public StringSerializedParamVariable GetString(string name) => stringData[referenceData.Where(x => x.Type == ParamVariableType.String).Where(x => x.Name == name).First().IndexInCollection];
        public StringSerializedParamVariable GetString(int index) => stringData[index];




        public ParamVariableType GetParamType(string name) => referenceData.Where(x => x.Name == name).First().Type;

        private void InitializeData()
        {
            if (referenceData == null)
                referenceData = new List<ParamReferenceVariable>();

            if (intData == null)
                intData = new List<IntSerializedParamVariable>();

            if (floatData == null)
                floatData = new List<FloatSerializedParamVariable>();

            if (boolData == null)
                boolData = new List<BoolSerializedParamVaraible>();

            if (stringData == null)
                stringData = new List<StringSerializedParamVariable>();
        }

        public IEnumerator<(string Name, ParamVariableType Type, ParamAccessibility access, EngineGraph graph, bool IsPersistent)> GetEnumerator()
        {
            foreach (var param in referenceData)
            {

                if (param.Type == ParamVariableType.Int)
                {
                    var intTemp = intData[param.IndexInCollection];
                    yield return (intTemp.Name, ParamVariableType.Int, param.Accessibility, param.EngineGraph, param.IsPersistent);
                }

                else if (param.Type == ParamVariableType.Float)
                {
                    var floatTemp = floatData[param.IndexInCollection];
                    yield return (floatTemp.Name, ParamVariableType.Float, param.Accessibility, param.EngineGraph, param.IsPersistent);
                }

                else if (param.Type == ParamVariableType.String)
                {
                    var stringTemp = stringData[param.IndexInCollection];
                    yield return (stringTemp.Name, ParamVariableType.String, param.Accessibility, param.EngineGraph, param.IsPersistent);
                }

                else
                {
                    var boolTemp = boolData[param.IndexInCollection];
                    yield return (boolTemp.Name, ParamVariableType.Bool, param.Accessibility, param.EngineGraph, param.IsPersistent);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return GetEnumerator();
        }

#if UNITY_EDITOR
        public void Dirty()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }

    public static class SaveHelper
    {
        private static SaveGlobal _dataInterface = default;

        public static void SaveChanges(Params parameters)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            SaveGlobal data = _dataInterface.LoadData($"{Application.dataPath}/Save/GlobalData.arcueid");
#else
            SaveGlobal data = _dataInterface.LoadData($"{Application.persistentDataPath}/GlobalData.arcueid");
#endif

            for (int i = 0; i < data.intData.Count; i++)
                data.intData[i] = parameters.intData[i].Value;

            for (int i = 0; i < data.floatData.Count; i++)
                data.floatData[i] = parameters.floatData[i].Value;

            for (int i = 0; i < data.boolData.Count; i++)
                data.boolData[i] = parameters.boolData[i].Value;

            for (int i = 0; i < data.stringData.Count; i++)
                data.stringData[i] = parameters.stringData[i].Value;


#if UNITY_STANDALONE || UNITY_EDITOR
            data.SaveData($"{Application.dataPath}/Save/GlobalData.arcueid", true);
#else
            data.SaveData($"{Application.persistentDataPath}/GlobalData.arcueid", true);
#endif
        }

        public static void LoadChanges(Params parameters)
        {
            string path;

#if UNITY_STANDALONE || UNITY_EDITOR
            path = Application.dataPath;
#else
            path = $"{Application.persistentDataPath}/GlobalData.arcueid";
#endif


#if UNITY_STANDALONE || UNITY_EDITOR
            if (!ParadoxSerialization.CheckFolder($"{path}/Save"))
            {
                ParadoxSerialization.CreateFolder(path, "Save");
                CreateData(parameters, $"{path}/Save/GlobalData.arcueid");
                return;
            }

            if (!ParadoxSerialization.CheckFile($"{path}/Save/GlobalData.arcueid"))
            {
                CreateData(parameters, $"{path}/Save/GlobalData.arcueid");
                return;
            }
#else
            if (!ParadoxSerialization.CheckFile(path))
            {
                CreateData(parameters, path);
                return;
            }
#endif

#if UNITY_STANDALONE || UNITY_EDITOR
            SaveGlobal data = _dataInterface.LoadData($"{path}/Save/GlobalData.arcueid");
#else
            SaveGlobal data = _dataInterface.LoadData(path);
#endif

            for (int i = 0; i < data.intData.Count; i++)
                parameters.intData[i].Value = data.intData[i];

            for (int i = 0; i < data.floatData.Count; i++)
                parameters.floatData[i].Value = data.floatData[i];

            for (int i = 0; i < data.boolData.Count; i++)
                parameters.boolData[i].Value = data.boolData[i];

            for (int i = 0; i < data.stringData.Count; i++)
                parameters.stringData[i].Value = data.stringData[i];
        }

        public static void SaveSettings(SaveGlobal data)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            data.SaveData($"{Application.dataPath}/Save/Settings.arcueid", true);
#else
            data.SaveData($"{Application.persistentDataPath}/Settings.arcueid", true);
#endif
        }

        public static void LoadSettings(System.Action<SaveGlobal> OnComplete, SettingsManager manager)
        {
            string path;

#if UNITY_STANDALONE || UNITY_EDITOR
            path = Application.dataPath;
#else
            path = $"{Application.persistentDataPath}/Settings.arcueid";
#endif


#if UNITY_STANDALONE || UNITY_EDITOR
            if (!ParadoxSerialization.CheckFolder($"{path}/Save"))
            {
                ParadoxSerialization.CreateFolder(path, "Save");
                manager.GetData().SaveData($"{path}/Save/Settings.arcueid", true);
                return;
            }

            if (!ParadoxSerialization.CheckFile($"{path}/Save/Settings.arcueid"))
            {
                manager.GetData().SaveData($"{path}/Save/Settings.arcueid", true);
                return;
            }
#else
            if (!ParadoxSerialization.CheckFile(path))
            {
                manager.GetData().SaveData($"{path}/Save/Settings.arcueid", true);
                return;
            }
#endif

#if UNITY_STANDALONE || UNITY_EDITOR
            SaveGlobal data = _dataInterface.LoadData($"{path}/Save/Settings.arcueid");
#else
            SaveGlobal data = _dataInterface.LoadData(path);
#endif

            OnComplete(data);
        }

        private static void CreateData(Params parameters, string path)
        {
            SaveGlobal data = new SaveGlobal() { intData = new List<int>(), floatData = new List<float>(), boolData = new List<bool>(), stringData = new List<string>() };


            for (int i = 0; i < parameters.intData.Count; i++)
                data.intData.Add(parameters.intData[i].Value);

            for (int i = 0; i < parameters.floatData.Count; i++)
                data.floatData.Add(parameters.floatData[i].Value);

            for (int i = 0; i < parameters.boolData.Count; i++)
                data.boolData.Add(parameters.boolData[i].Value);

            for (int i = 0; i < parameters.stringData.Count; i++)
                data.stringData.Add(parameters.stringData[i].Value);

            data.SaveData(path, true);
        }
    }
}

