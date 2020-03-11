using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace ParadoxEngine.Localization
{
    [System.Serializable]
    public class LanguageData : ScriptableObject
    {
        public string filterName;
        public List<string> objectsName = new List<string>();

        public List<ParadoxAnswerNodeLocalizationData> answerNodeData = new List<ParadoxAnswerNodeLocalizationData>();
        public List<ParadoxTextNodeLocalizationData> textNodeData = new List<ParadoxTextNodeLocalizationData>();
        public List<ParadoxTextLocalizationData> textData = new List<ParadoxTextLocalizationData>();
        public List<ParadoxDropdownLocalizationData> dropdownData = new List<ParadoxDropdownLocalizationData>();

        //public bool ContainsID(int id, ELanguageGraphFilter filter)
        //{
        //    if (filter == ELanguageGraphFilter.AnswerNode)
        //        return answerNodeData.Any(x => x.id == id);

        //    if (filter == ELanguageGraphFilter.Dropdown)
        //        return dropdownData.Any(x => x.id == id);

        //    if (filter == ELanguageGraphFilter.Text)
        //        return textData.Any(x => x.id == id);

        //    if (filter == ELanguageGraphFilter.TextNode)
        //        return textNodeData.Any(x => x.id == id);

        //    return false;
        //}
    }
}