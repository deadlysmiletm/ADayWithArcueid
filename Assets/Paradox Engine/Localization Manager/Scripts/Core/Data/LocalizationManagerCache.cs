using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ParadoxEngine.Localization
{
    [CreateAssetMenu(fileName = "LocalizationCache", menuName = "Paradox Engine/Localization/Cache")]
    public class LocalizationManagerCache : ScriptableObject
    {
        public List<LanguageData> languageDataPacks;
        public string currentLanguage;
        public bool isLoaded = false;
    }
}