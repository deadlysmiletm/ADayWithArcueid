using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine.Transitions;
using ParadoxEngine.Utilities;
using UnityEngine.UI;
using System;

public class NMoveCharacter : NTemplate
{
    public DCharacter character;
    public EnumPosition position = default;
    public EnumPositionTransition transition;
    public Vector2 customPoint = Vector2.zero;
    public bool pointClamp = true;
    [NonSerialized] private bool _inAction = false;

    public NMoveCharacter()
    {
        output = new NodeOutput();
        input = new NodeInput() { inputNode = new List<NTemplate>() };
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Move_Character;
        myRect = new Rect(10f, 10f, 110f, 55f);
    }

#if UNITY_EDITOR
    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("Character"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("CharacterSelected"));
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

        if (!DialogueDatabase.activeGraphPlayer.CharactersInScene.ContainsKey(character))
        {
            endedInstruction = true;
            Debug.LogWarning($"The character {character.name} is not in the scene");
            return;
        }

        Image charaImg = DialogueDatabase.activeGraphPlayer.TakeCharacterPool(character);

        switch (transition)
        {
            case EnumPositionTransition.Instante:
                ParadoxMoveTransition.InstanteToPosition(charaImg.rectTransform, position, this, customPoint);
                break;
            case EnumPositionTransition.Slide:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxMoveTransition.SlideToPosition(charaImg.rectTransform, position, this, customPoint, timeDuration));
                break;
            case EnumPositionTransition.Fade:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxMoveTransition.FadeToPosition(charaImg, position, this, customPoint, timeDuration));
                break;
        }

        _inAction = true;
    }

    public override void EndState()
    {
        base.EndState();
        _inAction = false;
    }
}
