using UnityEngine;
using UnityEditor;
using ParadoxEngine;
using ParadoxEngine.Utilities;
using System.Collections.Generic;
using System.Linq;

public class NChangeFlowChart : NTemplate
{
    public EngineGraph newGraph;

    public NChangeFlowChart()
    {
        input = new NodeInput();
        input.inputNode = new List<NTemplate>();
    }

#if UNITY_EDITOR
    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Change_Flow_Chart;
        myRect = new Rect(10f, 10f, 110f, 55f);
    }

    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("Story"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("StorySelected"));
    }

    public override void UpdateNode(Event e, Rect viewRect)
    {
        base.UpdateNode(e, viewRect);
    }

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
    {
        base.UpdateNodeGUI(e, viewRect, viewSkin);
    }
#endif

    protected override void OutputDefinition(GUISkin viewSkin) { }

    public override void Execute()
    {
        DialogueDatabase.activeGraphPlayer.cache.HasSave = true;
        DialogueDatabase.activeGraphPlayer.AssignBehaviour(newGraph);
    }
}
