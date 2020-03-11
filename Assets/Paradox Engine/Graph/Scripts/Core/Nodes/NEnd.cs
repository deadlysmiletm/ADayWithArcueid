using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine.Utilities;

[System.Serializable]
public class NEnd : NTemplate
{
    public NEnd()
    {
        input = new NodeInput();
        input.inputNode = new List<NTemplate>();
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.End;
        myRect = new Rect(10f, 10f, 110f, 55f);
    }

#if UNITY_EDITOR
    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("StartEnd"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("StartEndSelected"));
    }

    public override void UpdateNode(Event e, Rect viewRect) => base.UpdateNode(e, viewRect);

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin) => base.UpdateNodeGUI(e, viewRect, viewSkin);

    protected override void OutputDefinition(GUISkin viewSkin) {}
#endif

    public override void Execute() => DialogueDatabase.activeGraphPlayer.Stop();
}
