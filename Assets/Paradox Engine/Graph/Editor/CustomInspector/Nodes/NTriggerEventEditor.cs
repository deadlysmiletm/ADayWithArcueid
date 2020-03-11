using UnityEngine;
using UnityEditor;
using ParadoxFramework.Events;

[CustomEditor(typeof(NTriggerEvent))]
public class NTriggerEventEditor : Editor
{
    private GUIStyle _title;
    private NTriggerEvent _node;


    private void OnEnable() => _node = (NTriggerEvent)target;

    public override void OnInspectorGUI()
    {
        _title = new GUIStyle(GUI.skin.label)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            stretchWidth = true,
            wordWrap = true
        };

        GUILayout.Space(10);

        GUILayout.Label("PARADOX ENGINE" + "\n" + "TRIGGER EVENT STATE", _title);

        GUILayout.Space(10);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
        GUILayout.Space(20);

        EditorGUILayout.LabelField("Info:");
        GUILayout.BeginVertical("BOX");
        EditorGUILayout.LabelField("\n The Trigger event state can execute the events of the parameters or of a GameEvent.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
        GUILayout.EndVertical();

        GUILayout.Space(10);

        GUILayout.BeginVertical("BOX");

        EditorGUILayout.LabelField("GameEvent:");
        _node.listener = (GameEvent)EditorGUILayout.ObjectField(_node.listener, typeof(GameEvent), false);

        GUILayout.EndVertical();

        Repaint();
    }
}
