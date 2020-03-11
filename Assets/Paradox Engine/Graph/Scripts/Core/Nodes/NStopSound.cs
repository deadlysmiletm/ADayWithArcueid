using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine.Utilities;

public class NStopSound : NTemplate
{
    public NStopSound()
    {
        output = new NodeOutput();
        input = new NodeInput() { inputNode = new List<NTemplate>() };
    }

#if UNITY_EDITOR
    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Stop_Sound;
        myRect = new Rect(10f, 10f, 120f, 55f);
    }

    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("Sound"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("SoundSelected"));
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

    public override void Execute()
    {
        AudioSource source = DialogueDatabase.activeGraphPlayer.soundChannel;
        source.Stop();

        DialogueDatabase.activeGraphPlayer.ChangeNode(output.outputNode);
    }
}
