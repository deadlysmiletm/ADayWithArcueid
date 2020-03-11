using UnityEngine;
using UnityEditor;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NStopSound))]
    public class NStopSoundEditor : Editor
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

            GUILayout.Label("PARADOX ENGINE" + "\n" + "STOP PLAY STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Stop Sound state stop the audio clip played in the Sound FX channel.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();
        }
    }
}