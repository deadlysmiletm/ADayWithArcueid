using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;

namespace ParadoxEngine
{
    public class VToolbar : VTemplate
    {
        private string _graphName;
        public static bool showDialogue = false;
        private static GUIStyle _toolbar;
        private static GUIStyle _button;
        private static GUIStyle _label;
        private static GUIStyle _dropdown;
        private static GUIStyle _style;
        private static GUIStyle _textStyle;

        public VToolbar() : base("Toolbar") { }

        public override void Execute(Rect editorRect, Rect precentageRect, Event e, EngineGraph currentGraph)
        {
            _textStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                alignment = TextAnchor.UpperCenter,
                onNormal = new GUIStyleState() { textColor = Color.white }
            };

            base.Execute(editorRect, precentageRect, e, currentGraph);
            _style = new GUIStyle(GUI.skin.box);

            ShowCreatePanelGUI();
            GUILayout.BeginHorizontal(_toolbar);

            if (GUILayout.Button("File", _dropdown, GUILayout.Width(50)))
            {

                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent("Create Graph"), false, () => showDialogue = true);
                menu.AddItem(new GUIContent("Load Graph"), false, EngineGraphEditorUtilities.LoadGraph);
                
                menu.AddSeparator("");

                if (currentGraph == null)
                {
                    menu.AddDisabledItem(new GUIContent("Unload Graph"));
                    menu.AddDisabledItem(new GUIContent("Delete Graph"));
                }

                else
                {
                    currentGraph.selectedNode = null;

                    menu.AddItem(new GUIContent("Delete Graph/Confirm"), false, EngineGraphEditorUtilities.DeleteGraph);
                    menu.AddItem(new GUIContent("Unload Graph"), false, EngineGraphEditorUtilities.UnloadGraph);
                }

                menu.Show(new Vector2(5, 17));
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Insert", _dropdown, GUILayout.Width(100)))
            {
                GenericMenu menu = new GenericMenu();

                if (currentGraph != null)
                {
                    currentGraph.selectedNode = null;

                    menu.AddItem(new GUIContent("Story State/Add Text State", "Write dialogues and narrative text in the text container."), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Text, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Story State/Add Clear Text State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Clear, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Story State/Add Change Flow Chart State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Change_Flow_Chart, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Character State/Add Show Character State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Show_Character, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Character State/Add Hide Character State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Hide_Character, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Character State/Add Change Character Sprite State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Change_Character_Sprite, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Character State/Add Character Move State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Move_Character, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Image State/Add Change Background State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Change_Background, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Effect State/Add Delay State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Delay, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Effect State/Add Show Text Container"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Show_Text_Container, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Effect State/Add Hide Text Container"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Hide_Text_Container, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Sound State/Music/Add Play Music State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Play_Music, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Sound State/Music/Add Stop Music State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Stop_Music, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Sound State/Sound FX/Add Play Sound State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Play_Sound, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Sound State/Sound FX/Add Stop SouSoundnd State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Stop_Sound, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Sound State/Voice/Add Play Voice State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Play_Voice, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Sound State/Voice/Add Stop Voice State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Stop_Voice, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Branch State/Add Question Branch State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Branch_Question, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Branch State/Add Conditional Branch State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Branch_Condition, new Vector2(50, 50)));
                    menu.AddSeparator("Branch State/");
                    menu.AddItem(new GUIContent("Branch State/Add Answer State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Answer, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("Branch State/Add Condition State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Condition, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("System State/Add Set Parameter State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Set_Param, new Vector2(50, 50)));
                    menu.AddItem(new GUIContent("System State/Add Trigger Event State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Set_Param, new Vector2(50, 50)));
                }

                else
                {
                    menu.AddDisabledItem(new GUIContent("Story State/Add Text State"));
                    menu.AddDisabledItem(new GUIContent("Story State/Add Clear Text State"));
                    menu.AddDisabledItem(new GUIContent("Story State/Add Change Flow Chart State"));
                    menu.AddDisabledItem(new GUIContent("Character State/Add Show Character State"));
                    menu.AddDisabledItem(new GUIContent("Character State/Add Hide Character State"));
                    menu.AddDisabledItem(new GUIContent("Character State/Add Change Character Sprite State"));
                    menu.AddDisabledItem(new GUIContent("Character State/Add Character Move State"));
                    menu.AddDisabledItem(new GUIContent("Image State/Add Change Background State"));
                    menu.AddDisabledItem(new GUIContent("Effect State/Add Delay State"));
                    menu.AddDisabledItem(new GUIContent("Effect State/Add Show Text Container"));
                    menu.AddDisabledItem(new GUIContent("Effect State/Add Hide Text Container"));
                    menu.AddDisabledItem(new GUIContent("Sound State/Music/Add Play Music State"));
                    menu.AddDisabledItem(new GUIContent("Sound State/Music/Add Stop Music State"));
                    menu.AddDisabledItem(new GUIContent("Sound State/Sound FX/Add Play Sound State"));
                    menu.AddDisabledItem(new GUIContent("Sound State/Sound FX/Add Stop Sound State"));
                    menu.AddDisabledItem(new GUIContent("Sound State/Voice/Add Play Voice State"));
                    menu.AddDisabledItem(new GUIContent("Sound State/Voice/Add Stop Voice State"));
                    menu.AddDisabledItem(new GUIContent("Branch State/Add Question Branch State"));
                    menu.AddDisabledItem(new GUIContent("Branch State/Add Conditional Brach State"));
                    menu.AddSeparator("Branch State/");
                    menu.AddDisabledItem(new GUIContent("Branch State/Add Answer State"));
                    menu.AddDisabledItem(new GUIContent("Branch State/Add Condition State"));
                    menu.AddDisabledItem(new GUIContent("System State/Add Set Parameter State"));
                    menu.AddDisabledItem(new GUIContent("System State/Add Trigger Event State"));
                }

                menu.Show(new Vector2(65, 17));
            }

            if (GUILayout.Button("Tools", _dropdown, GUILayout.Width(80)))
            {
                GenericMenu menu = new GenericMenu();

                //menu.AddDisabledItem(new GUIContent("Import/XML"));
                //menu.AddDisabledItem(new GUIContent("Import/TXT"));
                //menu.AddDisabledItem(new GUIContent("Export/XML"));
                //menu.AddDisabledItem(new GUIContent("Export/TXT"));
                menu.AddItem(new GUIContent("Clear cache", "Clear the cache information of the last session."), false, EngineGraphCacheUtilities.ClearSessionCache);

                menu.Show(new Vector2(135, 17));
            }

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();

            if (showDialogue)
                CreateGraphDialogueGUI();
        }

        private void CreateGraphDialogueGUI()
        {
            GUILayout.BeginArea(new Rect(20, 40, 250, 70), _style);

            GUILayout.Label("Create Graph", _textStyle);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Name:");
            _graphName = GUILayout.TextField(_graphName, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Create"))
            {
                if (!string.IsNullOrEmpty(_graphName))
                {
                    EngineGraphEditorUtilities.CreateNewGraph(_graphName);
                    showDialogue = false;
                }
                else
                    EditorUtility.DisplayDialog("Paradox Engine", "The name isn't valid.", "OK");
            }

            if (GUILayout.Button("Cancel"))
            {
                _graphName = "";
                showDialogue = false;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private static void ShowCreatePanelGUI()
        {
            _toolbar = GUI.skin.FindStyle("toolbar");
            _button = GUI.skin.FindStyle("toolbarButton");
            _label = GUI.skin.FindStyle("toolbarButton");
            _dropdown = GUI.skin.FindStyle("toolbarDropdown");

            if (_toolbar == null || _button == null || _label == null || _dropdown == null)
            {
                _toolbar = _viewSkin.box;
                _toolbar.border = new RectOffset(0, 0, 1, 1);
                _toolbar.margin = new RectOffset(0, 0, 0, 0);
                _toolbar.padding = new RectOffset(10, 10, 1, 1);

                _label = _viewSkin.box;
                _label.border = new RectOffset(2, 2, 0, 0);
                _label.margin = new RectOffset(-2, -2, 0, 0);
                _label.padding = new RectOffset(6, 6, 4, 4);

                _button = _label;
                _button.active.background = _viewSkin.GetStyle("ViewBG").normal.background;
                _dropdown = _button;
            }

        }
    }
}