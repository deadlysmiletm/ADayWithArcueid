using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NClearText))]
    public class NClearTextEditor : Editor
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

            GUILayout.Label("PARADOX ENGINE" + "\n" + "CLEAR TEXT STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Clear Text state clear the text container.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();
        }
    }
}