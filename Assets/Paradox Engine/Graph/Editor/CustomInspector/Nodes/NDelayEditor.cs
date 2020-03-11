using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NDelay))]
    public class NDelayEditor : Editor
    {
        private GUIStyle _title;
        private GUIStyle _bold;
        private NDelay _node;

        private void OnEnable() => _node = (NDelay)target;

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

            GUILayout.Label("PARADOX ENGINE" + "\n" + "DELAY STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Delay state delay in seconds the execution of the next state.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.Label(new GUIContent("Time:", "Delay time in seconds."), _bold);

            EditorGUI.BeginChangeCheck();
            _node.delay = EditorGUILayout.FloatField(_node.delay);
            
            if (EditorGUI.EndChangeCheck())
                EngineGraphUtilities.SetDirty(this);
        }
    }
}