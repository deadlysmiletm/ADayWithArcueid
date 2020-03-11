using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NShowTextContainer))]
    public class NShowTextContainerEditor : Editor
    {
        private GUIStyle _title;

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

            GUILayout.Label("PARADOX ENGINE" + "\n" + "SHOW TEXT CONTAINER STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Show Text Container state show the text container. The Text Container will be show when a Text state is called.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();
        }
    }
}