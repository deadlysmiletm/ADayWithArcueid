using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;
using ParadoxEngine.Utilities.Parameters;
using System.Linq;
using System;


namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NCondition))]
    public class NConditionEditor : Editor
    {
        private GUIStyle _title;
        private GUIStyle _empty;
        private NCondition _node;
        private Action OnRepaint = delegate { };

        private void OnEnable() => _node = (NCondition)target;

        public override void OnInspectorGUI()
        {
            _title = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                stretchWidth = true
            };

            _empty = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Italic
            };

            GUILayout.Space(10);

            GUILayout.Label("PARADOX ENGINE" + "\n" + "CONDITION STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Condition state is one of the desitions that can be selected derived of a Conditional branch state.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();

            GUILayout.Space(10);

            _node.elseConditional = EditorGUILayout.Toggle("Is else condition", _node.elseConditional);

            GUILayout.Space(5);

            GUILayout.Label(new GUIContent("Conditions:", "The conditions to access to this branch."), new GUIStyle(GUI.skin.label) { fontSize = 13, fontStyle = FontStyle.Bold });

            GUILayout.BeginVertical("BOX");

            CheckCollection();

            if (!_node.data.Any())
                GUILayout.Label("Empty", _empty);

            else if (_node.elseConditional)
                GUILayout.Label("Else condition", _empty);

            else
            {
                EditorGUI.BeginChangeCheck();

                List<string> tempArray = new List<string>();

                var parameters = _node.parentGraph.parameters.Where(x =>
                {
                    if (x.access == ParamAccessibility.IsLocal)
                        return x.graph == _node.parentGraph;

                    return true;
                });

                tempArray = parameters.Select(x => x.Name).ToList();


                foreach (var (Name, CompareType, VarType) in _node.data)
                {
                    GUILayout.BeginVertical("BOX");

                    var array = tempArray.Where(x => x == Name || !_node.data.Any(y => y.Name == x)).ToArray();
                    int arrayIndex = array.IndexOf(x => x == Name);

                    arrayIndex = EditorGUILayout.Popup(arrayIndex, array.ToArray());

                    GUILayout.Space(3);

                    if (Name != array[arrayIndex])
                    {
                        var node = _node.parentGraph.parameters.Where(x => x.Name == array[arrayIndex]).First();

                        _node.data.ChangeParameter(Name, array[arrayIndex], node.Type);
                        EngineGraphUtilities.SetDirty(_node);
                        break;
                    }

                    var graphParam = _node.parentGraph.parameters.Where(x => x.Name == Name)
                                        .First();

                    EnumCompareType currentTag = CompareType;

                    if (VarType != ParamVariableType.Bool && VarType != ParamVariableType.String)
                        _node.data.UpdateCompareType(Name, (EnumCompareType)EditorGUILayout.EnumPopup(new GUIContent("Compare type:", "Type of comparation of the condition."), currentTag));


                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent("Value", "Value to compare."));

                    switch (VarType)
                    {
                        case ParamVariableType.Int:
                            _node.data.UpdateValue(Name, EditorGUILayout.IntField(_node.data.GetInt(Name)));
                            break;
                        case ParamVariableType.Float:
                            _node.data.UpdateValue(Name, EditorGUILayout.FloatField(_node.data.GetFloat(Name)));
                            break;
                        case ParamVariableType.Bool:
                            _node.data.UpdateValue(Name, EditorGUILayout.Toggle(_node.data.GetBool(Name)));
                            break;
                        case ParamVariableType.String:
                            _node.data.UpdateValue(Name, EditorGUILayout.TextField(_node.data.GetString(Name)));
                            break;
                    }

                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Delete  Condition"))
                    {
                        OnRepaint += () => _node.data.UnsuscribeValue(array[arrayIndex]);
                        GUILayout.EndVertical();
                        break;
                    }

                    GUILayout.EndVertical();
                }

                if (EditorGUI.EndChangeCheck())
                    EngineGraphUtilities.SetDirty(_node);
            }

            GUILayout.EndVertical();

            var selectedParameters = _node.parentGraph.parameters.Where(x => !_node.data.Contains(x.Name))
                                                                 .Where(x =>
                                                                 {
                                                                     if (x.access == ParamAccessibility.IsLocal)
                                                                         return x.graph == _node.parentGraph;

                                                                     return true;
                                                                 });

            if (selectedParameters.Any())
            {
                if (GUILayout.Button("Add Condition"))
                {
                    var paramTemp = _node.parentGraph.parameters.Where(x =>
                                                                 {
                                                                     if (x.access == ParamAccessibility.IsLocal)
                                                                         return x.graph == _node.parentGraph;

                                                                     return true;
                                                                 })
                                                                 .Where(x => !_node.data.Any(y => y.Name == x.Name))
                                                                 .First();

                    _node.data.SuscribeValue(paramTemp.Name, EnumCompareType.Equal, paramTemp.Type);

                    EngineGraphUtilities.SetDirty(_node);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            else
                EditorGUILayout.HelpBox("The Scene graph don't have mode parameters.", MessageType.Warning);

            OnRepaint();
            OnRepaint = delegate { };
            Repaint();
        }

        private void CheckCollection()
        {
            string temp = "";

            foreach (var (Name, CompareType, VarType) in _node.data)
            {
                if (!_node.parentGraph.parameters.Any(x => x.Name == Name))
                {
                    temp = Name;
                    break;
                }
            }

            if (temp == "")
                return;

            _node.data.UnsuscribeValue(temp);
            CheckCollection();
        }

        private bool CompareValue(object one, object two, int type)
        {
            switch (type)
            {
                case 0:
                    return (int)one != (int)two;
                case 1:
                    return (float)one != (float)two;
                case 2:
                    return (bool)one != (bool)two;
            }

            return false;
        }
    }
}