using System.Collections.Generic;
using ParadoxEngine.Utilities;

namespace ParadoxEngine.Localization
{
    public enum ELanguageGraphFilter
    { 
        Text = 0,
        TextNode = 1,
        AnswerNode = 2,
        Dropdown = 3
    }

    [System.Serializable]
    public class ParadoxLocalizationData
    {
        public byte[] id;
        public ELanguageGraphFilter filterType;

        public ParadoxLocalizationData(ELanguageGraphFilter type) => filterType = type;
    }

    [System.Serializable]
    public class ParadoxTextNodeLocalizationData : ParadoxLocalizationData
    {
        public List<ParadoxTextNodeData> data;
        public int parentReference;


        public ParadoxTextNodeLocalizationData() : base(ELanguageGraphFilter.TextNode) { }
    }

    [System.Serializable]
    public class ParadoxAnswerNodeLocalizationData : ParadoxLocalizationData
    {
        public string data;
        public int parentReference;


        public ParadoxAnswerNodeLocalizationData() : base(ELanguageGraphFilter.AnswerNode) { }
    }

    [System.Serializable]
    public class ParadoxTextLocalizationData : ParadoxLocalizationData
    {
        public string data;

        public ParadoxTextLocalizationData() : base(ELanguageGraphFilter.Text) { }
    }

    [System.Serializable]
    public class ParadoxDropdownLocalizationData : ParadoxLocalizationData
    {
        public string[] data;

        public ParadoxDropdownLocalizationData() : base(ELanguageGraphFilter.Dropdown) { }
    }
}