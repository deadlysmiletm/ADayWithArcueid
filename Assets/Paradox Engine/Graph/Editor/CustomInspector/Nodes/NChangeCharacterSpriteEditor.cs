﻿using UnityEngine;
using UnityEditor;
using System.Linq;
using ParadoxEngine.Utilities;

[CustomEditor(typeof(NChangeCharacterSprite))]
public class NChangeCharacterSpriteEditor : Editor
{
    private GUIStyle _title;
    private NChangeCharacterSprite _node;
    private Container _characterContainer;
    private int _characterSelected;
    private int _spriteSelected;

    private void OnEnable()
    {
        _node = (NChangeCharacterSprite)target;
        _characterContainer = (Container)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Characters.asset", typeof(Container));

        if (_characterContainer.data == null || !_characterContainer.data.Any() || !_characterContainer.data.Contains(_node.character))
        {
            _node.character = null;
            _node.sprite = null;
            return;
        }

        if (_node.character != null)
            _characterSelected = _characterContainer.data.Select(x => (DCharacter)x).IndexOf(x => x == _node.character);

        else
        {
            _node.character = (DCharacter)_characterContainer.data.Select(x => x as DCharacter).First();
            _characterSelected = 0;
        }

        if (_node.sprite != null)
        {
            if (_node.character.ContainsSprite(_node.sprite))
                _spriteSelected = _node.character.SpriteForeach().IndexOf(x => x.Item2 == _node.sprite);
            else
            {
                _node.sprite = _node.character.SpriteForeach().GetValue(0).Item2;
                _spriteSelected = 0;
            }
        }

        else
        {
            _node.sprite = _node.character.SpriteForeach().GetValue(0).Item2;
            _spriteSelected = 0;
        }
    }

    public override void OnInspectorGUI()
    {
        _title = new GUIStyle(GUI.skin.label)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            stretchWidth = true
        };

        GUILayout.Space(10);

        GUILayout.Label("PARADOX ENGINE" + "\n" + "CHANGE CHARACTER SPRITE STATE", _title);

        GUILayout.Space(10);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
        GUILayout.Space(20);

        EditorGUILayout.LabelField("Info:");
        GUILayout.BeginVertical("BOX");
        EditorGUILayout.LabelField("\n The Change Character Sprite state change the sprite for a registed one of a character in the scene.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
        GUILayout.EndVertical();

        GUILayout.Space(10);

        if (_characterContainer.data == null || !_characterContainer.data.Any())
        {
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.HelpBox("The database don't have Characters.", MessageType.Warning);

            _node.character = null;
            _node.sprite = null;

            GUILayout.EndVertical();
            return;
        }

        if (_node.character == null)
        {
            _node.character = (DCharacter)_characterContainer.data.Select(x => x as DCharacter).First();
            _characterSelected = 0;

            _node.sprite = _node.character.SpriteForeach().GetValue(0).Item2;
            _spriteSelected = 0;
        }

        var allCharacaters = _characterContainer.data.Select(x => x as DCharacter).Select(x => x.name).ToArray();

        GUILayout.BeginHorizontal();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField(new GUIContent("Character:", "Select a Character."));
        _characterSelected = EditorGUILayout.Popup(_characterSelected, allCharacaters);

        GUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck())
        {
            _node.character = (DCharacter)_characterContainer.data.Select(x => x as DCharacter)
                            .GetValue(_characterSelected);

            EngineGraphUtilities.SetDirty(_node);
        }

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        var allSprites = _node.character.SpriteForeach().Select(x => x.Item1).ToArray();

        GUILayout.BeginHorizontal();

        if (allSprites.Any())
        {
            EditorGUILayout.LabelField(new GUIContent("Sprite:", "Select an Sprite registed in the Character."));
            _spriteSelected = EditorGUILayout.Popup(_spriteSelected, allSprites);
        }

        else
        {
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("BOX");
            EditorGUILayout.HelpBox("The database don't have Sprites in the Character.", MessageType.Warning);
            _node.sprite = null;
            _spriteSelected = 0;
            GUILayout.EndVertical();
            return;
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginVertical("BOX");


        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Duration time:", "Duration of the transition."));
        _node.timeDuration = EditorGUILayout.FloatField(_node.timeDuration);
        GUILayout.EndHorizontal();

        if (_node.timeDuration < 0)
            _node.timeDuration = 0;

        GUILayout.Space(3);

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(new GUIContent("Transition type:", "Type of transition to change sprite."));
        var allNames = new string[3] { "Instante", "Crossfade", "Fade Out and In" };

        _node.transition = (EnumImageTransition)EditorGUILayout.Popup((int)_node.transition, allNames);


        if (EditorGUI.EndChangeCheck())
        {
            _node.sprite = _node.character.GetSprite(allSprites[_spriteSelected]);
            EngineGraphUtilities.SetDirty(_node);
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}
