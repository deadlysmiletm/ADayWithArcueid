using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine.Utilities;
using ParadoxEngine.Transitions;
using UnityEngine.UI;

public class NChangeBackground : NTemplate
{
    public DLocation locationGroups;
    public Sprite locationBackground;
    public EnumImageTransition transition;
    [System.NonSerialized] private bool _inAction;

    public NChangeBackground()
    {
        output = new NodeOutput();
        input = new NodeInput() { inputNode = new List<NTemplate>() };
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Change_Background;
        myRect = new Rect(10f, 10f, 110f, 55f);
    }

#if UNITY_EDITOR
    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("Image"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("ImageSelected"));
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

        if (!_inAction)
        {
            var data = GetBackgrounds(DialogueDatabase.activeGraphPlayer.layer);

            switch (transition)
            {
                case EnumImageTransition.Instant:
                    ParadoxImageTransition.Instante(data.background1, data.background2, locationBackground, this);
                    break;
                case EnumImageTransition.Crossfade:
                    DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxImageTransition.Fade(data.background1, data.background2, locationBackground, this, timeDuration));
                    break;
                case EnumImageTransition.Slide_Down:
                    DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxImageTransition.SlideDown(data.background1, data.background2, locationBackground, this, timeDuration));
                    break;
                case EnumImageTransition.Slide_Up:
                    DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxImageTransition.SlideUp(data.background1, data.background2, locationBackground, this, timeDuration));
                    break;
                case EnumImageTransition.Slide_Left:
                    DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxImageTransition.SlideLeft(data.background1, data.background2, locationBackground, this, timeDuration));
                    break;
                case EnumImageTransition.Slide_Right:
                    DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxImageTransition.SlideRight(data.background1, data.background2, locationBackground, this, timeDuration));
                    break;
                case EnumImageTransition.Fade_Out_and_In:
                    DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxImageTransition.FadeOutAndIn(data.background1, data.background2, locationBackground, this, timeDuration));
                    break;
            }

            DialogueDatabase.activeGraphPlayer.layer = data.newLayer;
            _inAction = true;
        }
    }

    public override void EndState()
    {
        base.EndState();
        _inAction = false;
    }

    private (Image background1, Image background2, int newLayer) GetBackgrounds(int layer)
    {
        int newLayer = layer == 0 ? 1 : 0;
        return (DialogueDatabase.activeGraphPlayer.backgrounds[layer], DialogueDatabase.activeGraphPlayer.backgrounds[newLayer], newLayer);
    }
}
