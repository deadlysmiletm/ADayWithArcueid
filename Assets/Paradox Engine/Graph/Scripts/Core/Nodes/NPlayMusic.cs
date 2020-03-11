using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine.Utilities;
using ParadoxEngine.Transitions;

public class NPlayMusic : NTemplate
{
    public DSoundtrack musicGroup;
    public AudioClip musicClip;
    public EnumSoundTransition transition;
    [System.NonSerialized] private bool _inAction = false;

    public NPlayMusic()
    {
        input = new NodeInput { inputNode = new List<NTemplate>() };
        output = new NodeOutput();
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Play_Music;
        myRect = new Rect(10f, 10f, 120f, 55f);
        timeDuration = 2;
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

        if (_inAction)
            return;

        var data = GetAudioSources(DialogueDatabase.activeGraphPlayer.channel);

        switch (transition)
        {
            case EnumSoundTransition.Instant:
                ParadoxSoundTransition.Instante(data.source1, data.source2, musicClip, this);
                break;
            case EnumSoundTransition.Fade_In:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxSoundTransition.FadeIn(data.source1, data.source2, musicClip, this, timeDuration));
                break;
            case EnumSoundTransition.Crossfade:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxSoundTransition.Crossfade(data.source1, data.source2, musicClip, this, timeDuration));
                break;
            case EnumSoundTransition.Fade_Out_And_In:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxSoundTransition.FadeOutAndIn(data.source1, data.source2, musicClip, this, timeDuration));
                break;
        }

        DialogueDatabase.activeGraphPlayer.channel = data.newChannel;
        _inAction = true;
    }

    public override void EndState()
    {
        base.EndState();
        _inAction = false;
    }

    private (AudioSource source1, AudioSource source2, int newChannel) GetAudioSources(int channel)
    {
        int newChannel = channel == 0 ? 1 : 0;
        return (DialogueDatabase.activeGraphPlayer.musicChannel[channel], DialogueDatabase.activeGraphPlayer.musicChannel[newChannel], newChannel);
    }
}
