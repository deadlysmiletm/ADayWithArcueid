using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine.Transitions;
using ParadoxEngine.Utilities;
using UnityEngine.UI;
using System;

public class NChangeCharacterSprite : NTemplate
{
    [NonSerialized] private Image _charaImg;
    [NonSerialized] private Image _newCharaImg;

    public DCharacter character;
    public Sprite sprite;
    public EnumImageTransition transition = default;
    [NonSerialized] private bool _inAction = false;


    public NChangeCharacterSprite()
    {
        output = new NodeOutput();
        input = new NodeInput() { inputNode = new List<NTemplate>() };
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Change_Character_Sprite;
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
            DialogueDatabase.activeGraphPlayer.CharactersInScene[character] = _newCharaImg;
            DialogueDatabase.activeGraphPlayer.poolManager.DisposePoolObject("Character", _charaImg.gameObject);

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

        _charaImg = DialogueDatabase.activeGraphPlayer.TakeCharacterPool(character);
        _newCharaImg = DialogueDatabase.activeGraphPlayer.poolManager.GetPoolObject("Character").GetComponent<Image>();
        _newCharaImg.rectTransform.anchoredPosition = _charaImg.rectTransform.anchoredPosition;

        _charaImg.CrossFadeAlpha(1, 0, false);
        _newCharaImg.CrossFadeAlpha(0, 0, false);

        switch (transition)
        {
            case EnumImageTransition.Instant:
                ParadoxImageTransition.Instante(_charaImg, _newCharaImg, sprite, this);
                break;
            case EnumImageTransition.Crossfade:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxImageTransition.Fade(_charaImg, _newCharaImg, sprite, this, timeDuration));
                break;
            case EnumImageTransition.Fade_Out_and_In:
                DialogueDatabase.activeGraphPlayer.StartCoroutine(ParadoxImageTransition.FadeOutAndIn(_charaImg, _newCharaImg, sprite, this, timeDuration));
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
