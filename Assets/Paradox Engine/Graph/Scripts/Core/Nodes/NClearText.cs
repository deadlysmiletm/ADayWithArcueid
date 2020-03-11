using UnityEngine;
using ParadoxEngine.Utilities;

public class NClearText : NTemplate
{
    public NClearText() => output = new NodeOutput();

#if UNITY_EDITOR
    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Clear;
        myRect = new Rect(10f, 10f, 110f, 55f);
    }

    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("Story"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("StorySelected"));
    }

    public override void UpdateNode(Event e, Rect viewRect) => base.UpdateNode(e, viewRect);

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin) => base.UpdateNodeGUI(e, viewRect, viewSkin);
#endif

    public override void Execute()
    {
        DialogueDatabase.activeGraphPlayer.ChangeNameDialogueText("");
        DialogueDatabase.activeGraphPlayer.ChangeText("");
        DialogueDatabase.activeGraphPlayer.ChangeNode(output.outputNode);
    }
}
