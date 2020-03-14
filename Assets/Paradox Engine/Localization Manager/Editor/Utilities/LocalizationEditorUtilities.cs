using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxEngine.Localization;
using ParadoxFramework.IO;
using System;
using System.Linq;

namespace ParadoxEngine.Localization.Editor
{
    public static class LocalizationEditorUtilities
    {
        internal static void CreateLanguageData(string name, string filter, Action<LanguageData> OnCompleted)
        {
            LanguageData data = (LanguageData)ScriptableObject.CreateInstance<LanguageData>();

            if (data == null)
            {
                EditorUtility.DisplayDialog("Paradox Engine", "Language creation failed.", "OK");
                return;
            }

            data.name = name;
            data.filterName = filter;

            string path = $"Assets/Paradox Engine/Localization Manager/Resources/LanguagePacks/{name}.asset";

            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (LocalizationManagerWindow._window != null)
            {
                LocalizationManagerWindow._window.dataPack = data;
                OnCompleted(data);
            }

        }

        internal static void LoadLanguageData(Action<LanguageData> OnComplete)
        {
            string path = EditorUtility.OpenFilePanel("Load Language data", $"{Application.dataPath}/Paradox Engine/Localization Manager/Resources/LanguagePacks/", "");
            LoadSession(path, OnComplete);
        }

        private static void LoadSession(string path, Action<LanguageData> OnComplete)
        {
            LanguageData data;

            if (string.IsNullOrEmpty(path))
                return;

            string finalPath = path;

            int appPathLen = Application.dataPath.Length;
            finalPath = path.Substring(appPathLen - 6);

            data = (LanguageData)AssetDatabase.LoadAssetAtPath(finalPath, typeof(LanguageData));

            if (data == null)
            {
                EditorUtility.DisplayDialog("Paradox Engine", "Language data load failed.", "Ok");
                return;
            }

            if (LocalizationManagerWindow._window == null)
                return;

            LocalizationManagerWindow._window.dataPack = data;
            OnComplete(data);
        }

        internal static void UnloadLanguage()
        {
            if (LocalizationManagerWindow._window == null)
                return;

            LocalizationManagerWindow._window.dataPack = null;
        }

        internal static void DeleteLanguage()
        {
            if (LocalizationManagerWindow._window == null)
                return;

            var data = LocalizationManagerWindow._window.dataPack;
            LocalizationManagerWindow._window.dataPack = null;

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(data.GetInstanceID()));
            AssetDatabase.Refresh();
        }

        internal static void ExportJSONLanguageData(LanguageData data)
        {
            JSONExportData exportData = new JSONExportData
            {
                graphName = data.name,
                textData = data.textNodeData.Select(x =>
                {
                    return new JSONTextNodeData
                    {
                        LocalizationID = new Guid(x.id).ToString(),
                        FilterType = x.filterType,
                        Data = x.data.Select(n => new JSONTextNodeData.SerializableTextData
                        {
                            Text = n.Text,
                            CharacterName = n.IsDialogue? n.Character.name : "",
                        }).ToArray(),
                    };
                }).ToArray(),
                answerData = data.answerNodeData.Select(x =>
                {
                    return new JSONAnswerNodeData
                    {
                        LocalizationID = new Guid(x.id).ToString(),
                        FilterType = x.filterType,
                        Data = x.data
                    };
                }).ToArray(),
                menuTextData = data.textData.Select(x =>
                {
                    return new JSONAnswerNodeData
                    {
                        LocalizationID = new Guid(x.id).ToString(),
                        FilterType = x.filterType,
                        Data = x.data
                    };
                }).ToArray(),
                menuDropdownData = data.dropdownData.Select(x =>
                {
                    return new JSONDropdownData
                    {
                        LocalizationID = new Guid(x.id).ToString(),
                        FilterType = x.filterType,
                        Data = x.data
                    };
                }).ToArray()
            };


            exportData.SaveData($"Assets/Paradox Engine/{exportData.graphName}.json", false);
            AssetDatabase.Refresh();
        }

        internal static void ImportJSONLanguageData(Action<JSONExportData> OnComplete)
        {
            string path = EditorUtility.OpenFilePanel("Load JSON Lagunage", $"{Application.dataPath}/Paradox Engine/", "json");
            LoadJSON(path, OnComplete);
        }

        private static void LoadJSON(string path, Action<JSONExportData> OnComplete)
        {
            JSONExportData data = new JSONExportData();

            if (string.IsNullOrEmpty(path))
                return;

            //string finalPath = path;

            //int appPathLen = Application.dataPath.Length;
            //finalPath = path.Substring(appPathLen - 6);

            Debug.Log(path);
            data = new JSONExportData().LoadData(path);

            Debug.Log(data.answerData.Length);

            //if (data)
            //{
            //    EditorUtility.DisplayDialog("Paradox Engine", "Language data load failed.", "Ok");
            //    return;
            //}

            if (LocalizationManagerWindow._window == null)
                return;

            OnComplete(data);
        }
    }

    internal struct JSONExportData
    {
        public string graphName;
        public JSONTextNodeData[] textData;
        public JSONAnswerNodeData[] answerData;
        public JSONAnswerNodeData[] menuTextData;
        public JSONDropdownData[] menuDropdownData;
    }

    [Serializable]
    internal struct JSONAnswerNodeData
    {
        public string LocalizationID;
        public ELanguageGraphFilter FilterType;
        public string Data;
    }

    [Serializable]
    internal struct JSONDropdownData
    {
        public string LocalizationID;
        public ELanguageGraphFilter FilterType;
        public string[] Data;
    }

    [Serializable]
    internal struct JSONTextNodeData
    {
        [Serializable]
        public struct SerializableTextData
        {
            public string CharacterName;
            public string Text;
        }

        public string LocalizationID;
        public ELanguageGraphFilter FilterType;
        public SerializableTextData[] Data;
    }
}


