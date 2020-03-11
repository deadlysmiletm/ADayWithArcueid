using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine.Utilities;
using ParadoxEngine.Transitions;

public class NStopMusic : NTemplate
{
    public EnumSoundEndTransition transition;
    [System.NonSerialized] private bool _inAction = false;

    public NStopMusic()
    {
        output = new NodeOutput();
        input = new NodeInput() { inputNode = new List<NTemplate>() };
        timeDuration = 2;
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Stop_Music;
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

        if (_inAction)
            return;

        var data = GetAudioSource(DialogueDatabase.activeGraphPlayer.channel);

        switch (transition)
        {
            case EnumSoundEndTransition.FadeOut:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxSoundTransition.Crossfade(data.source1, data.source2, null, this, timeDuration));
                break;
            case EnumSoundEndTransition.Instant:
                ParadoxSoundTransition.Instante(data.source1, data.source2, null, this);
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

    private (AudioSource source1, AudioSource source2, int newChannel) GetAudioSource(int channel)
    {
        int newChannel = channel == 1 ? 0 : 1;
        return (DialogueDatabase.activeGraphPlayer.musicChannel[channel], DialogueDatabase.activeGraphPlayer.musicChannel[newChannel], newChannel);
    }
}