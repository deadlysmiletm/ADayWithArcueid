using UnityEngine;
using UnityEditor;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NStart))]
    public class NStartEditor : Editor
    {
        private GUIStyle _title;


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

            GUILayout.Label("PARADOX ENGINE" + "\n" + "START STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Start state is the initial state of the canvas.\n\n All the connections derived of this state will need end in the End state or a Change Flow Chart state.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();
        }
    }
}