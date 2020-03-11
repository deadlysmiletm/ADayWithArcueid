using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NText), true)]
    public class NTextEditor : Editor
    {
        private GUIStyle _title;
        private GUIStyle _bold;
        private NText _node;
        private List<bool> _avancedOptions;
        private Container _characterContainers;
        private List<int> _characterSelected;
        private Action _OnRepaint = delegate { };


        private void OnEnable()
        {
            _node = (NText)target;

            _characterContainers = (Container)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Characters.asset", typeof(Container));

            if (_characterContainers.data == null || !_characterContainers.data.Any())
                return;

            _characterSelected = new List<int>();
            _avancedOptions = new List<bool>();

            for (int i = 0; i < _node.data.Count; i++)
            {
                _avancedOptions.Add(_node.data[i].UseCustomKey || _node.data[i].UseDelay);

                if (_node.data[i].Character)
                    _characterSelected.Add(_characterContainers.data.IndexOf(_node.data[i].Character));

                else
                    _characterSelected.Add(0);
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

            _bold = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };

            GUILayout.Space(10);

            GUILayout.Label("PARADOX ENGINE" + "\n" + "TEXT STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Text state show text in the text container.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();

            GUILayout.Space(10);

            EditorGUILayout.BeginVertical("BOX");

            if (!_node.data.Any())
                EditorGUILayout.LabelField("The state don't have any data.", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic, wordWrap = true });

            else
            {
                for (int i = 0; i < _node.data.Count; i++)
                {
                    EditorGUILayout.BeginVertical("BOX");

                    var temp = _node.data[i];

                    EditorGUI.BeginChangeCheck();

                    temp.IsDialogue = EditorGUILayout.Toggle(new GUIContent("Is a dialogue: ", "Make this text a dialogue text."), temp.IsDialogue);

                    if (temp.IsDialogue)
                    {
                        var allCharacters = _characterContainers.data.Select(x => x as DCharacter).Select(x => x.name);
                        GUILayout.Label(new GUIContent("Character: ", "Select a character of the database."), _bold);

                        if (allCharacters.Any())
                        {
                            _characterSelected[i] = EditorGUILayout.Popup(_characterSelected[i], allCharacters.ToArray());
                            temp.Character = (DCharacter)_characterContainers.data[_characterSelected[i]];
                        }

                        else
                        {
                            GUILayout.BeginVertical("BOX");
                            EditorGUILayout.HelpBox("The database don't have charcters registed.", MessageType.Error);
                            GUILayout.EndVertical();
                        }
                    }

                    GUILayout.Space(2);

                    temp.ClearLastText = EditorGUILayout.Toggle(new GUIContent("Clear text: ", "Clear the previous text"), temp.ClearLastText);

                    if (!temp.ClearLastText)
                        temp.ContinueParagraph = EditorGUILayout.Toggle(new GUIContent("Continue paragraph: ", "The text will continue the last text, without clearing the canvas."), temp.ContinueParagraph);

                    GUILayout.Label(new GUIContent("Text: ", "Text that will be show on the canvas."), _bold);
                    temp.Text = GUILayout.TextArea(temp.Text);

                    GUILayout.Space(2);

                    _avancedOptions[i] = EditorGUILayout.Toggle("Avanced options: ", _avancedOptions[i]);

                    if (_avancedOptions[i])
                    {
                        temp.UseCustomKey = EditorGUILayout.Toggle(new GUIContent("Custom key", "This text use a custom key."), temp.UseCustomKey);

                        if (temp.UseCustomKey)
                            temp.CustomKey = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Key input: ", "The new key input of this node."), temp.CustomKey);

                        temp.UseDelay = EditorGUILayout.Toggle(new GUIContent("Use delay: ", "Delay the transition to the next text or node."), temp.UseDelay);

                        if (temp.UseDelay)
                        {
                            temp.DelayTime = EditorGUILayout.FloatField("Time: ", temp.DelayTime);

                            if (temp.DelayTime < 0)
                                temp.DelayTime = 0;
                        }
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        _node.data[i] = temp;
                        EngineGraphUtilities.SetDirty(_node);
                    }

                    if (GUILayout.Button("Delete"))
                    {
                        int index = i;
                        _OnRepaint += () => _node.data.RemoveAt(index);
                    }

                    EditorGUILayout.EndVertical();

                    GUILayout.Space(3);
                }
            }

            EditorGUILayout.EndVertical();

            _OnRepaint();

            if (GUILayout.Button("Add"))
            {
                _node.data.Add(new ParadoxTextNodeData()
                {
                    Text = "",
                    Character = null,
                    ClearLastText = true,
                    ContinueParagraph = false,
                    CustomKey = KeyCode.Return,
                    DelayTime = 0,
                    IsDialogue = false,
                    UseCustomKey = false,
                    UseDelay = false
                });

                _avancedOptions.Add(false);
                _characterSelected.Add(0);
            }

            Repaint();

            _OnRepaint = delegate { };
        }

        private void SaveChanges()
        {
            EditorUtility.SetDirty(_node);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}