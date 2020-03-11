using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using ParadoxEngine.Utilities;
using ParadoxEngine.Utilities.Parameters;
using System.Linq;
using System;
using System.Collections.Generic;

using Object = System.Object;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NSetParam))]
    public class NSetParamEditor : Editor
    {
        private GUIStyle _title;
        private GUIStyle _empty;
        private NSetParam _node;
        private Action OnRepaint = delegate { };

        private void OnEnable() => _node = (NSetParam)target;

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

            _empty = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Italic
            };

            GUILayout.Space(10);

            GUILayout.Label("PARADOX ENGINE" + "\n" + "SET PARAMETER STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Set parameter state can set, add or substract values of the parameters.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.BeginVertical("BOX");

            CheckCollection();

            if (!_node.data.Any())
                GUILayout.Label("Empty", _empty);

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


                foreach (var item in parameters)
                    tempArray.Add(item.Name);


                foreach (var (Name, ModType, VarType) in _node.data)
                {
                    GUILayout.BeginVertical("BOX");

                    var array = tempArray.Where(x => x == Name || !_node.data.Any(y => y.Name == x)).ToArray();
                    int arrayIndex = array.IndexOf(x => x == Name);

                    arrayIndex = EditorGUILayout.Popup(arrayIndex, array.ToArray());

                    if (Name != array[arrayIndex])
                    {
                        var node = _node.parentGraph.parameters.Where(x => x.Name == array[arrayIndex]).First();

                        _node.data.ChangeParameter(Name, array[arrayIndex], node.Type);
                        EngineGraphUtilities.SetDirty(_node);
                        break;
                    }

                    if (VarType != ParamVariableType.Bool)
                        _node.data.UpdateCompareType(Name, (EnumModType)EditorGUILayout.EnumPopup(new GUIContent("Setter type:", "Type of setting to modify the parameter."), ModType));

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent("Value:", "Modification value for parameter setting."));

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

                    if (GUILayout.Button("Delete Setter"))
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
                if (GUILayout.Button("Add Setter"))
                {
                    var param = _node.parentGraph.parameters.Where(x => !_node.data.Contains(x.Name))
                                                            .Where(x =>
                                                            {
                                                                if (x.access == ParamAccessibility.IsLocal)
                                                                    return x.graph == _node.parentGraph;

                                                                return true;
                                                            }).First();

                    _node.data.SuscribeValue(param.Name, EnumModType.Swap, param.Type);
                    EngineGraphUtilities.SetDirty(_node);
                }
            }

            else
                EditorGUILayout.HelpBox("The Scene graph don't have more parameters.", MessageType.Warning);


            OnRepaint();
            OnRepaint = delegate { };
            Repaint();
        }

        //private bool CompareValue(object one, object two, int type)
        //{
        //    switch (type)
        //    {
        //        case 0:
        //            return (int)one != (int)two;
        //        case 1:
        //            return (float)one != (float)two;
        //        case 2:
        //            return (bool)one != (bool)two;
        //    }

        //    return false;
        //}

        private void CheckCollection()
        {
            string temp = "";

            foreach (var (Name, ModType, ValueType) in _node.data)
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
    }
}