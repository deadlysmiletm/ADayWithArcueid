using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;
using ParadoxEngine.Utilities.Parameters;
using System.Linq;

namespace ParadoxEngine
{
    [System.Serializable]
    public class VProperty : VTemplate
    {
        //private NTemplate _currentNode;
        //private Object _myNode;
        //private GUIStyle _myStyleSmall;
        //private GUIStyle _myStyleBig;
        //private NTemplate[] _allNodes;
        private ParamAccessibility _paramAccess = ParamAccessibility.IsGlobal;
        //private List<string> _typeParam = new List<string>() { "int", "float", "bool", "event" };
        private Vector2 _pos;
        private bool _toDelete;
        private string _nameToDelete;
        [SerializeField] private Params parameters;

        private static GUIStyle _toolbar;
        private static GUIStyle _button;
        private static GUIStyle _label;
        private static GUIStyle _dropdown;
        //private static GUIStyle _textStyle;

        public VProperty() : base("Property View") { }

        protected override void OnEnable()
        {
            //_myStyleSmall = new GUIStyle
            //{
            //    fontSize = 10,
            //    fontStyle = FontStyle.Italic,
            //    alignment = TextAnchor.MiddleLeft
            //};

            //_myStyleBig = new GUIStyle
            //{
            //    fontSize = 15,
            //    fontStyle = FontStyle.Bold,
            //    alignment = TextAnchor.MiddleCenter,
            //};

            parameters = EngineGraphUtilities.LoadParameterReference();
        }

        public override void Execute(Rect editorRect, Rect precentageRect, Event e, EngineGraph currentGraph)
        {
            base.Execute(editorRect, precentageRect, e, currentGraph);
            if (currentGraph != null)
            {
                if (currentGraph.selectedNode)
                    viewTitle = currentGraph.selectedNode.nodeName;
            }

            else
                _paramAccess = ParamAccessibility.IsGlobal;

            GUILayout.BeginArea(viewRect);

            ShowToolbarGUI();

            GUILayout.BeginArea(new Rect(new Vector2(precentageRect.x, viewRect.y + 8), new Vector2(viewRect.width, 24)));
            GUILayout.BeginHorizontal(_toolbar);
            
            //Show the menu and add a new parameter.
            if (GUILayout.Button("Add parameter", _button, GUILayout.Width(100)))
            {
                GenericMenu menu = new GenericMenu();

                if (currentGraph == null)
                {
                    menu.AddDisabledItem(new GUIContent("Int"));
                    menu.AddDisabledItem(new GUIContent("Float"));
                    menu.AddDisabledItem(new GUIContent("Bool"));
                    menu.AddDisabledItem(new GUIContent("String"));
                }

                else
                {
                    menu.AddItem(new GUIContent("Int"), false, () => parameters.SuscribeValue($"Parameter {parameters.Count}", ParamVariableType.Int, currentGraph, _paramAccess));
                    menu.AddItem(new GUIContent("Float"), false, () => parameters.SuscribeValue($"Parameter {parameters.Count}", ParamVariableType.Float, currentGraph, _paramAccess));
                    menu.AddItem(new GUIContent("Bool"), false, () => parameters.SuscribeValue($"Parameter {parameters.Count}", ParamVariableType.Bool, currentGraph, _paramAccess));
                    menu.AddItem(new GUIContent("String"), false, () => parameters.SuscribeValue($"Parameter {parameters.Count}", ParamVariableType.String, currentGraph, _paramAccess));
                }

                menu.Show(new Vector2(precentageRect.x + 5, viewRect.y + 14));
            }

            GUILayout.Space(5);
            string accessName = _paramAccess == ParamAccessibility.IsGlobal ? "Global" : "Local";

            //Show the menu and select the parameters of the selected accessibility.
            if (GUILayout.Button(accessName, _button, GUILayout.Width(50)))
            {
                GenericMenu menu = new GenericMenu();

                if (currentGraph == null)
                {
                    menu.AddDisabledItem(new GUIContent("Show Global"));
                    menu.AddDisabledItem(new GUIContent("Show Local"));
                }

                else
                {
                    menu.AddItem(new GUIContent("Show Global"), false, () => _paramAccess = ParamAccessibility.IsGlobal);
                    menu.AddItem(new GUIContent("Show Local"), false, () => _paramAccess = ParamAccessibility.IsLocal);
                }

                menu.Show(new Vector2(precentageRect.x + 110, viewRect.y + 14));
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(precentageRect.x, viewRect.y * 2 + 24, viewRect.width, viewRect.height));
            GUILayout.BeginVertical("BOX");

            var selectedParams = _paramAccess == ParamAccessibility.IsGlobal ? parameters.Where(x => x.access == ParamAccessibility.IsGlobal) :
                                 parameters.Where(x => x.access == ParamAccessibility.IsLocal).Where(x => x.graph == currentGraph);

            if (!selectedParams.Any())
            {
                GUILayout.BeginVertical("BOX");
                GUILayout.Label("Empty", new GUIStyle()
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Italic
                });
                GUILayout.EndVertical();
            }

            else
            {
                GUILayout.BeginVertical();
                _pos =  GUILayout.BeginScrollView(_pos, GUILayout.Width(viewRect.width));

                foreach (var (Name, Type, Access, Graph, IsPersistent) in selectedParams)
                {
                    GUILayout.Space(5);

                    GUILayout.BeginHorizontal("BOX");

                    GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();

                    string itemIsPersist;
                    string itemValue;

                    if (Type == ParamVariableType.Int)
                    {
                        var item = parameters.GetInt(Name);
                        itemIsPersist = IsPersistent ? "P: " : "";
                        itemValue = $"Int: {item.Value}";
                    }

                    else if (Type == ParamVariableType.Float)
                    {
                        var item = parameters.GetFloat(Name);
                        itemIsPersist = IsPersistent ? "P: " : "";
                        itemValue = $"Float: {item.Value}";
                    }

                    else if (Type == ParamVariableType.String)
                    {
                        var item = parameters.GetString(Name);
                        itemIsPersist = IsPersistent ? "P:" : "";
                        itemValue = $"String: {item.Value}";
                    }

                    else
                    {
                        var item = parameters.GetBool(Name);
                        itemIsPersist = IsPersistent ? "P: " : "";
                        itemValue = $"Bool: {item.Value}";
                    }


                    GUILayout.Label($"{itemIsPersist} {Name}");
                    GUILayout.Label(itemValue);

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();

                    if (_toDelete && _nameToDelete.Equals(Name))
                    {
                        if (GUILayout.Button("Delete"))
                        {
                            EngineGraphWindow.OnRepaintRequest += () =>
                            {
                                parameters.UnsuscribeValue(_nameToDelete);
                                _nameToDelete = "";
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                            };
                        }

                        if (GUILayout.Button("Cancel"))
                        {
                            _toDelete = false;
                            _nameToDelete = "";
                        }
                    }

                    else
                    {
                        if (GUILayout.Button("Edit"))
                        {
                            System.Action<string> OnSave = name =>
                            {
                                var editingParam = Name;
                                var editedName = name;

                                EngineGraphWindow.OnRepaintRequest += () =>
                                {
                                    var graphs = Resources.LoadAll("Data").Select(x => x as EngineGraph).Where(x => x != null);

                                    if (graphs.Any())
                                    {
                                        foreach (var item in graphs)
                                        {
                                            var checkCondition = item.nodes.Where(x => x.nodeType == EnumNodeType.Condition);

                                            if (checkCondition.Any())
                                            {
                                                var conditions = checkCondition.Select(x => (NCondition)x)
                                                                               .Where(x => x.data.Contains(editingParam));

                                                foreach (var node in conditions)
                                                {
                                                    node.data.UpdateName(editingParam, editedName);
                                                    EditorUtility.SetDirty(node);
                                                }
                                            }


                                            var checkSetter = item.nodes.Where(x => x.nodeType == EnumNodeType.Set_Param);

                                            if (checkSetter.Any())
                                            {
                                                var setters = checkSetter.Select(x => (NSetParam)x)
                                                                         .Where(x => x.data.Any(y => y.Name == editingParam));

                                                foreach (var node in setters)
                                                {
                                                    node.data.UpdateName(editingParam, editedName);
                                                    EditorUtility.SetDirty(node);
                                                }
                                            }
                                        }
                                    }

                                    AssetDatabase.SaveAssets();
                                    AssetDatabase.Refresh();
                                };
                            };

                            if (Type == ParamVariableType.Int)
                                EditVariablePopupWindow.InitContainerPopup(parameters.GetInt(Name), OnSave);
                            else if (Type == ParamVariableType.Float)
                                EditVariablePopupWindow.InitContainerPopup(parameters.GetFloat(Name), OnSave);
                            else if (Type == ParamVariableType.Bool)
                                EditVariablePopupWindow.InitContainerPopup(parameters.GetBool(Name), OnSave);
                            else
                                EditVariablePopupWindow.InitContainerPopup(parameters.GetString(Name), OnSave);

                        }

                        if (GUILayout.Button("Delete"))
                        {
                            _toDelete = true;
                            _nameToDelete = Name;
                        }
                    }

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.Space(5);
                }

                GUILayout.Space(30);

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            }


            GUILayout.EndVertical();
            GUILayout.EndArea();

            GUILayout.EndArea();
        }

        private void ShowToolbarGUI()
        {
            _toolbar = GUI.skin.FindStyle("toolbar");
            _button = GUI.skin.FindStyle("toolbarButton");
            _label = GUI.skin.FindStyle("toolbarButton");
            _dropdown = GUI.skin.FindStyle("toolbarDropdown");

            if (_toolbar == null || _button == null || _label == null || _dropdown == null)
            {
                _toolbar = _viewSkin.box;
                _toolbar.border = new RectOffset(0, 0, 1, 1);
                _toolbar.margin = new RectOffset(0, 0, 0, 0);
                _toolbar.padding = new RectOffset(10, 10, 1, 1);

                _label = _viewSkin.box;
                _label.border = new RectOffset(2, 2, 0, 0);
                _label.margin = new RectOffset(-2, -2, 0, 0);
                _label.padding = new RectOffset(6, 6, 4, 4);

                _button = _label;
                _button.active.background = _viewSkin.GetStyle("ViewBG").normal.background;
                _dropdown = _button;
            }
        }
    }

    public class CustomDragData
    {
        public int originalIndex;
        public IList originalList;
    }
}