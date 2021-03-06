﻿using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine.Transitions;
using ParadoxEngine.Utilities;
using UnityEngine.UI;
using System;

public class NShowCharacter : NTemplate
{
    public DCharacter character;
    public Sprite sprite;
    public EnumCharacterTransition transition = default;
    public EnumPosition position = default;
    public Vector2 customPoint = Vector2.zero;
    public bool pointClamp = true;
    [NonSerialized] private bool _inAction = false;

    public NShowCharacter()
    {
        output = new NodeOutput();
        input = new NodeInput() { inputNode = new List<NTemplate>() };
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Show_Character;
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

        if (DialogueDatabase.activeGraphPlayer.CharactersInScene.ContainsKey(character))
        {
            endedInstruction = true;
            Debug.LogWarning($"The character {character.name} is already in the scene");
            return;
        }

        Image charaImg = DialogueDatabase.activeGraphPlayer.TakeCharacterPool(character);
        charaImg.CrossFadeAlpha(1, 0, false);

        switch (transition)
        {
            case EnumCharacterTransition.Instante:
                ParadoxCharacterTransition.Instante(charaImg, sprite, position, this, customPoint);
                break;
            case EnumCharacterTransition.Fade:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxCharacterTransition.Fade(charaImg, sprite, position, this, customPoint, timeDuration));
                break;
            case EnumCharacterTransition.Slide_Up:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxCharacterTransition.SlideUp(charaImg, sprite, position, this, customPoint, timeDuration));
                break;
            case EnumCharacterTransition.Slide_Down:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxCharacterTransition.SlideDown(charaImg, sprite, position, this, customPoint, timeDuration));
                break;
            case EnumCharacterTransition.Slide_Right:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxCharacterTransition.SlideRight(charaImg, sprite, position, this, customPoint, timeDuration));
                break;
            case EnumCharacterTransition.Slide_Left:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxCharacterTransition.SlideLeft(charaImg, sprite, position, this, customPoint, timeDuration));
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
