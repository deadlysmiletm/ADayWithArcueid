using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine.Utilities;

public class NPlayVoice : NTemplate
{
    public DSoundtrack voiceGroup;
    public AudioClip voiceClip;


    public NPlayVoice()
    {
        input = new NodeInput { inputNode = new List<NTemplate>() };
        output = new NodeOutput();
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Play_Voice;
        myRect = new Rect(10f, 10f, 120f, 55f);
    }

#if UNITY_EDITOR
    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("Sound"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("SoundSelected"));
    }

    public override void UpdateNode(Event e, Rect viewRect) => base.UpdateNode(e, viewRect);

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin) => base.UpdateNodeGUI(e, viewRect, viewSkin);
#endif


    public override void Execute()
    {
        if (endedInstruction)
        {
            DialogueDatabase.activeGraphPlayer.ChangeNode(output.outputNode);
            return;
        }

        PlayVoice();
    }

    private void PlayVoice()
    {
        AudioSource source = DialogueDatabase.activeGraphPlayer.voiceChannel;
        source.Stop();

        source.clip = voiceClip;

        source.Play();
        endedInstruction = true;
    }
}
