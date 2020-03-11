using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine.Utilities;

public class NPlaySound : NTemplate
{
    public DSoundtrack soundGroup;
    public AudioClip soundClip;
    public bool canLoop = false;

    public NPlaySound()
    {
        input = new NodeInput { inputNode = new List<NTemplate>() };
        output = new NodeOutput();
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Play_Sound;
        myRect = new Rect(10f, 10f, 120f, 55f);
        canLoop = false;
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

        PlaySound();
    }

    private void PlaySound()
    {
        AudioSource source = DialogueDatabase.activeGraphPlayer.soundChannel;

        if (canLoop)
        {
            source.Stop();
            source.loop = true;
            source.clip = soundClip;
            source.Play();
        }

        else
        {
            source.loop = false;
            source.PlayOneShot(soundClip);
        }

        endedInstruction = true;
    }
}
