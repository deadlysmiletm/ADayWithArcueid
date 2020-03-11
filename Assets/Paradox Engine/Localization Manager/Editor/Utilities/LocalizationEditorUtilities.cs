using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxEngine.Localization;
using System;


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
    }
}