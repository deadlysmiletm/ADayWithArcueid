using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;
using System.Linq;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NChangeBackground))]
    public class NChangeBackgroundEditor : Editor
    {
        private GUIStyle _title;
        private NChangeBackground _node;
        private Container _locationsContainer;
        private int _backgroundGroupSelected;
        private int _locationBackgroundSelected;

        private void OnEnable()
        {
            _node = (NChangeBackground)target;
            _locationsContainer = (Container)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Locations.asset", typeof(Container));

            if (_locationsContainer.data == null || !_locationsContainer.data.Any() || !_locationsContainer.data.Contains(_node.locationGroups))
            {
                _node.locationGroups = null;
                _node.locationBackground = null;
                return;
            }

            if (_node.locationGroups != null)
                _backgroundGroupSelected = _locationsContainer.data.Select(x => (DLocation)x).IndexOf(x => x == _node.locationGroups);

            else
            {
                _node.locationGroups = (DLocation)_locationsContainer.data.Select(x => x as DLocation).First();
                _backgroundGroupSelected = 0;
            }

            if (_node.locationBackground != null)
            {
                if (_node.locationGroups.ContainsBackground(_node.locationBackground))
                    _locationBackgroundSelected = _node.locationGroups.Foreach().IndexOf(x => x.Item2 == _node.locationBackground);
                else
                {
                    _node.locationBackground = _node.locationGroups.Foreach().GetValue(0).Item2;
                    _locationBackgroundSelected = 0;
                }
            }

            else
            {
                _node.locationBackground = _node.locationGroups.Foreach().GetValue(0).Item2;
                _locationBackgroundSelected = 0;
            }
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

            GUILayout.Label("PARADOX ENGINE" + "\n" + "CHANGE BACKGROUND STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The change background state remplace the actual background of the scene.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();

            GUILayout.Space(10);

            if (_locationsContainer.data == null || !_locationsContainer.data.Any())
            {
                GUILayout.BeginVertical("BOX");
                EditorGUILayout.HelpBox("The database don't have Location Groups.", MessageType.Warning);

                _node.locationGroups = null;
                _node.locationBackground = null;

                GUILayout.EndVertical();
                return;
            }

            if (_node.locationGroups == null)
            {
                _node.locationGroups = (DLocation)_locationsContainer.data.Select(x => x as DLocation).First();
                _backgroundGroupSelected = 0;

                _node.locationBackground = _node.locationGroups.Foreach().GetValue(0).Item2;
                _locationBackgroundSelected = 0;
            }

            var allBackgroundGroups = _locationsContainer.data.Select(x => x as DLocation).Select(x => x.name).ToArray();

            GUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField(new GUIContent("Locataion group:", "Select a Location Group."));
            _backgroundGroupSelected = EditorGUILayout.Popup(_backgroundGroupSelected, allBackgroundGroups);

            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                _node.locationGroups = (DLocation)_locationsContainer.data.Select(x => x as DLocation)
                                .GetValue(_backgroundGroupSelected);

                EngineGraphUtilities.SetDirty(_node);
            }

            GUILayout.Space(5);

            EditorGUI.BeginChangeCheck();

            var allLocationBackgrounds = _node.locationGroups.Foreach().Select(x => x.Item1).ToArray();

            GUILayout.BeginHorizontal();

            if (allLocationBackgrounds.Any())
            {
                EditorGUILayout.LabelField(new GUIContent("Background:", "Select a Background registed in the Location Group."));
                _locationBackgroundSelected = EditorGUILayout.Popup(_locationBackgroundSelected, allLocationBackgrounds);
            }

            else
            {
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("BOX");
                EditorGUILayout.HelpBox("The database don't have Background in the Location Group.", MessageType.Warning);
                _node.locationBackground = null;
                _locationBackgroundSelected = 0;
                GUILayout.EndVertical();
                return;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("BOX");


            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Duration time:", "Duration of the transition."));
            _node.timeDuration = EditorGUILayout.FloatField(_node.timeDuration);
            GUILayout.EndHorizontal();

            if (_node.timeDuration < 0)
                _node.timeDuration = 1;

            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Transition type:", "Type of transition to change Backgrounds."));
            _node.transition = (EnumImageTransition)EditorGUILayout.EnumPopup(_node.transition);

            if (EditorGUI.EndChangeCheck())
            {
                _node.locationBackground = _node.locationGroups[allLocationBackgrounds[_locationBackgroundSelected]];
                EngineGraphUtilities.SetDirty(_node);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}