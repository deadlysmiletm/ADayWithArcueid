using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using ParadoxEngine.Utilities;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NAnswer))]
    public class NAnswerEditor : Editor
    {
        private GUIStyle _title;
        private NAnswer _node;
        private AnimBool _keyBool;

        private void OnEnable()
        {
            _node = (NAnswer)target;

            _keyBool = new AnimBool();
            _keyBool.valueChanged.AddListener(Repaint);
        }

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

            GUILayout.Label("PARADOX ENGINE" + "\n" + "ANSWER STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Answer state is one of the desitions that can be selected derived of a Question node.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();

            GUILayout.Space(10);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField(new GUIContent("Desition text:", "Text that appears on the decision button"));
            _node.answer = EditorGUILayout.TextField(_node.answer);

            GUILayout.Space(5);

            EditorGUILayout.LabelField(new GUIContent("Button canvas position:", "Absolute position of the button on the canvas."));

            GUILayout.BeginVertical("BOX");
            _node.buttonPosition.x = EditorGUILayout.FloatField(new GUIContent("X:", "X absolute value from 0 to 1, of left to right. Center is .5f"), _node.buttonPosition.x);
            _node.buttonPosition.y = EditorGUILayout.FloatField(new GUIContent("Y:", "Y absolute value from 0 to 1, of down to up. Center is .5f"), _node.buttonPosition.y);
            GUILayout.EndVertical();

            if (_node.buttonPosition.x > 1)
                _node.buttonPosition.x = 1;
            else if (_node.buttonPosition.x < 0)
                _node.buttonPosition.x = 0;

            if (_node.buttonPosition.y > 1)
                _node.buttonPosition.x = 1;
            else if (_node.buttonPosition.y < 0)
                _node.buttonPosition.y = 0;

            GUILayout.Space(10);

            _keyBool.target = _node.customKey;

            _node.customKey = GUILayout.Toggle(_node.customKey, new GUIContent("Custom input key", "Use an input key to select this decision."));

            if (EditorGUILayout.BeginFadeGroup(_keyBool.faded))
            {
                GUILayout.BeginVertical("BOX");
                _node.myKey = (KeyCode)EditorGUILayout.EnumFlagsField("Input key", _node.myKey);
                GUILayout.EndVertical();
            }
            EditorGUILayout.EndFadeGroup();
            

            if (EditorGUI.EndChangeCheck())
                EngineGraphUtilities.SetDirty(_node);
        }

        private void OnSceneGUI()
        {
            Handles.BeginGUI();
            var scene = EditorWindow.GetWindow<SceneView>().camera;

            Panel();

            Vector3 pos = Vector3.zero;
            Handles.color = Color.black;
            var tempPos = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(scene.transform as RectTransform, _node.buttonPosition, DialogueDatabase.activeGraphPlayer.GetComponentInParent<Canvas>().transform as RectTransform);
            pos.x = tempPos.x;
            pos.y = tempPos.y;

            Handles.RectangleHandleCap(0, tempPos, Quaternion.identity, 10, EventType.Ignore);

            Debug.Log(tempPos);

            Handles.EndGUI();
        }

        public void Panel()
        {
            GUILayout.BeginArea(new Rect(20, 20, 250, 175));
            var rec = EditorGUILayout.BeginVertical();
            GUI.color = new Color32(200, 200, 200, 230);
            GUI.Box(rec, GUIContent.none);

            GUILayout.Space(5);

            GUILayout.Label("Button Position", _title);

            _node.buttonPosition.x = EditorGUILayout.FloatField("Absolute X:", _node.buttonPosition.x);
            _node.buttonPosition.y = EditorGUILayout.FloatField("Absolute Y:", _node.buttonPosition.y);

            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}