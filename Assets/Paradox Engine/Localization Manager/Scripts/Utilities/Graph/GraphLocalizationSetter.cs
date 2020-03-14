using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ParadoxEngine.Localization
{
    public static class GraphLocalizationSetter
    {
        public static void TranslateGraph(EngineGraph graph, LocalizationManager manager)
        {
            IEnumerable<NTemplate> nodes = graph.nodes.Where(x => x.nodeType == Utilities.EnumNodeType.Answer || x.nodeType == Utilities.EnumNodeType.Text);

            NText textTemplate;
            NAnswer answerTemplate;

            foreach (var node in nodes)
            {
                if (node.nodeType == Utilities.EnumNodeType.Text)
                {
                    textTemplate = (NText)node;
                    textTemplate.data = manager.GetTextNodeTranslation(node.GetLocalizationID());
                }

                else
                {
                    answerTemplate = (NAnswer)node;
                    answerTemplate.answer = manager.GetAnswerNodeTranslation(node.GetLocalizationID());
                }
            }
        }
    }
}