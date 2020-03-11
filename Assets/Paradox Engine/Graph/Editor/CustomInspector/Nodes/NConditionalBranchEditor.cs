using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NConditionalBranch))]
    public class NConditionalBranchEditor : Editor
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

            GUILayout.Label("PARADOX ENGINE" + "\n" + "CONDITIONAL BRANCH STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Conditional Branch is responsible for select conditions. The only state that can be connected to the Conditional Branch is the Condition State.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();
        }
    }
}