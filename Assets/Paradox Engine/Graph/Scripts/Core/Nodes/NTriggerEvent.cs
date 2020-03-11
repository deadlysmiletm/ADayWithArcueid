using UnityEngine;
using ParadoxEngine.Utilities;
using ParadoxFramework.Events;


public class NTriggerEvent : NTemplate
{
    public GameEvent listener;

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Trigger_Event;
        myRect = new Rect(10f, 10f, 110f, 55f);
    }


#if UNITY_EDITOR
    public override void UpdateNode(Event e, Rect viewRect) => base.UpdateNode(e, viewRect);

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin) => base.UpdateNodeGUI(e, viewRect, viewSkin);

    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("System"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("SystemSelected"));
    }
#endif

    public override void Execute()
    {
        listener.ExecuteEvent();

        DialogueDatabase.activeGraphPlayer.ChangeNode(output.outputNode);
    }
}
