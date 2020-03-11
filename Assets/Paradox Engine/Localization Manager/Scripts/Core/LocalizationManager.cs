using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine.Utilities;
using System;
using System.Linq;


namespace ParadoxEngine.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        [SerializeField] private LocalizationManagerCache _cache = null;
        private Dictionary<string, List<LanguageData>> _avalibleLanguage = new Dictionary<string, List<LanguageData>>();
        public string currentLanguage = string.Empty;
        public event Action OnUpdateTranslation = delegate { };

        private void Awake()
        {
            if (_cache.isLoaded)
            {
                for (int i = 0; i < _cache.languageDataPacks.Count; i++)
                {
                    if (!_avalibleLanguage.ContainsKey(_cache.languageDataPacks[i].filterName))
                        _avalibleLanguage.Add(_cache.languageDataPacks[i].filterName, _cache.languageDataPacks.Where(x => x.filterName == _cache.languageDataPacks[i].filterName).ToList());
                }

                currentLanguage = _cache.currentLanguage;
                return;
            }

            LoadAvalibleLanguagePack();
        }

        private void Start() => OnUpdateTranslation();


        public void UpdateLanguage(string languageName)
        {
            if (!_avalibleLanguage.ContainsKey(languageName))
            {
                Debug.Log($"The given language {languageName} don't exist.");
                return;
            }

            currentLanguage = languageName;
            _cache.currentLanguage = languageName;
            OnUpdateTranslation();
        }

        public void UpdateLanguage() => OnUpdateTranslation();

        public List<string> GetAvalibleLanguages() => _avalibleLanguage.Select(x => x.Key).ToList();


        public void ResetLanguageCache()
        {
            _avalibleLanguage = null;
            _cache.currentLanguage = string.Empty;
            _cache.isLoaded = false;
        }


        private void LoadAvalibleLanguagePack()
        {
            var pack = Resources.LoadAll<LanguageData>("LanguagePacks");

            _avalibleLanguage = new Dictionary<string, List<LanguageData>>();

            for (int i = 0; i < pack.Length; i++)
            {
                if (!_avalibleLanguage.ContainsKey(pack[i].filterName))
                    _avalibleLanguage.Add(pack[i].filterName, pack.Where(x => x.filterName == pack[i].filterName).ToList());
            }

            _cache.languageDataPacks = new List<LanguageData>();

            foreach (var item in pack)
                _cache.languageDataPacks.Add(item);

            _cache.currentLanguage = currentLanguage;
            _cache.isLoaded = true;
        }

        /// <summary>
        /// Get a translation from a given ID. Only use this for the data type AnswerNode.
        /// </summary>
        /// <param name="id">ID from the given object</param>
        /// <returns></returns>
        public string GetAnswerNodeTranslation(Guid id)
        {
            var data = _avalibleLanguage[currentLanguage].SelectMany(x => x.answerNodeData);

            if (!data.Any(x => id.Equals(new Guid(x.id))))
            {
                Debug.LogError($"Error, the id: {id} is not definend in the requested language.");
                return default;
            }

            return data.Where(x => id.Equals(new Guid(x.id))).Select(x => x as ParadoxAnswerNodeLocalizationData).First().data;
        }

        /// <summary>
        /// Get a translation from a given ID. Only use this for the data type Text.
        /// </summary>
        /// <param name="id">ID from the given object</param>
        /// <returns></returns>
        public string GetTextTranslation(Guid id)
        {
            var data = _avalibleLanguage[currentLanguage].SelectMany(x => x.textData);

            if (!data.Any(x => new Guid(x.id).Equals(id)))
            {
                Debug.LogError($"Error, the id: {id} is not definend in the requested language.");
                return default;
            }

            return data.Where(x => new Guid(x.id).Equals(id)).Select(x => x as ParadoxTextLocalizationData).First().data;
        }

        /// <summary>
        /// Get a translation from a given ID. Only use this for the data type TextNode.
        /// </summary>
        /// <param name="id">ID from the given object.</param>
        /// <returns></returns>
        public List<ParadoxTextNodeData> GetTextNodeTranslation(Guid id)
        {
            var data = _avalibleLanguage[currentLanguage].SelectMany(x => x.textNodeData);

            if (!data.Any(x => new Guid(x.id).Equals(id)))
            {
                Debug.LogError($"Error, the id: {id} is not definend in the requested language.");
                return default;
            }

            return data.Where(x => new Guid(x.id).Equals(id)).Select(x => x as ParadoxTextNodeLocalizationData).First().data;
        }

        /// <summary>
        /// Get a translation from a given ID. Only use this for the data type Dropdown.
        /// </summary>
        /// <param name="id">ID from the given object.</param>
        /// <returns></returns>
        public string[] GetDropdownTranslation(Guid id)
        {
            var data = _avalibleLanguage[currentLanguage].SelectMany(x => x.dropdownData);

            if (!data.Any(x => new Guid(x.id).Equals(id)))
            {
                Debug.LogError($"Error, the id: {id} is not definend in the requested language.");
                return default;
            }

            return data.Where(x => new Guid(x.id).Equals(id)).Select(x => x as ParadoxDropdownLocalizationData).First().data;
        }
    }

#if UNITY_EDITOR
    public class TestTemp
    {
        [UnityEditor.MenuItem("Paradox Engine/Localization/Clear cache")]
        public static void ResetLanguageCache()
        {
            LocalizationManager temp = GameObject.FindObjectOfType<LocalizationManager>();
            temp.ResetLanguageCache();
        }
    }
#endif
}