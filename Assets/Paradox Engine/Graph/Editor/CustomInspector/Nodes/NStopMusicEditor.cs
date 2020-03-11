using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NStopMusic))]
    public class NStopMusicEditor : Editor
    {
        private GUIStyle _title;
        private NStopMusic _node;

        private void OnEnable() => _node = (NStopMusic)target;

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

            GUILayout.Label("PARADOX ENGINE" + "\n" + "STOP MUSIC STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Stop Muisc state stop the actual clip playing in the music channel.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.BeginVertical();

            EditorGUI.BeginChangeCheck();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Duration time:", "Duration of the transition."));
            _node.timeDuration = EditorGUILayout.FloatField(_node.timeDuration);

            if (_node.timeDuration <= 0)
                _node.timeDuration = 1;

            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Transition type:", "Type of transition to stopping the music."));
            string[] compareTag = new string[2] { "Fade Out", "Instant" };
            int selectedTag = (int)_node.transition;

            selectedTag = EditorGUILayout.Popup(selectedTag, compareTag);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                _node.transition = (EnumSoundEndTransition)selectedTag;
                EngineGraphUtilities.SetDirty(_node);
            }
        }
    }
}