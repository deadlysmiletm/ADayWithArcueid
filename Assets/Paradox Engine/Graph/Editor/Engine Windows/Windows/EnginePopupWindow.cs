using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;

public class EnginePopupWindow : EditorWindow
{
    static EnginePopupWindow currentPopup;
    NTemplate _node;
    string text = "Text...";

    public static void InitEngineGraphPopupWindow(NTemplate node)
    {
        currentPopup = (EnginePopupWindow)EditorWindow.GetWindow<EnginePopupWindow>();
        currentPopup.titleContent = new GUIContent("Paradox Engine: Node setting");
        currentPopup._node = node;
    }

    private void OnGUI()
    {
        GUILayout.Space(40);

        switch (_node.nodeType)
        {
            case EnumNodeType.Text:
                TextNodeSetting();
                break;
        }

        GUILayout.Space(40);
    }

    private void TextNodeSetting()
    {
        EditorGUILayout.LabelField("Text:", EditorStyles.boldLabel);

        text = GUILayout.TextArea(text);

        if (string.IsNullOrEmpty(text))
            EditorGUILayout.HelpBox("The text is empty.", MessageType.Warning);

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save", GUILayout.Height(20)))
        {
            NText dialogueNode = _node as NText;

            //dialogueNode.text = text;
            currentPopup.Close();
        }

        if (GUILayout.Button("Cancel", GUILayout.Height(20)))
            currentPopup.Close();

        GUILayout.EndHorizontal();
    }

}
