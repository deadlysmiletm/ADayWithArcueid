using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
using ParadoxEngine.Utilities;
using System;
using System.Linq;

namespace ParadoxEngine.Localization.Editor
{
    public class LocalizationManagerWindow : EditorWindow
    {
        internal static LocalizationManagerWindow _window;
        private Vector2 _pos = new Vector2();
        private float _width = 632;
        private float _height = 723;
        private float _heightMin = 466;

        public LanguageData dataPack;
        private List<ParadoxLocalizationData> _cacheData;

        private bool _addingGraph;
        private EngineGraph _graphToAdd;
        private bool _addingText;
        private TextLocalizationSetter _textToAdd;
        private bool _addingDropdown;
        private DropdownLocalizationSetter _dropwdownToAdd;

        private bool _toDelete = false;
        private bool _creatingNewData = false;
        private string _newName = string.Empty;
        private string _filterName = string.Empty;
        private List<string> _objectNames;

        private int _startElement = 0;
        private int _endElement;

        private GUIStyle _description;

        private Action OnRepaint = delegate { };


        [MenuItem("Paradox Engine/Localization/Manager")]
        public static void InitLocalizationManagerWindow()
        {
            _window = (LocalizationManagerWindow)EditorWindow.GetWindow<LocalizationManagerWindow>();
            _window.titleContent = new GUIContent("Paradox Engine: Localization");

            _window.maxSize = new Vector2(_window._width, _window._height);
            _window.minSize = new Vector2(_window._width, _window._heightMin);
            _window.dataPack = null;
            _window._cacheData = new List<ParadoxLocalizationData>();
            _window._objectNames = new List<string>();
        }

        private void OnGUI()
        {
            _description = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic, wordWrap = true };

            GUILayout.Space(20);

            GUILayout.BeginHorizontal("BOX");

            if (_creatingNewData)
            {
                if (GUILayout.Button("Create"))
                {
                    if (_newName == "" || string.IsNullOrWhiteSpace(_newName) || _filterName == "" || string.IsNullOrWhiteSpace(_filterName))
                    {
                        Debug.LogError("The name and filter name can't be null.");
                        return;
                    }

                    if (Resources.LoadAll<LanguageData>("LanguagePacks").Select(x => x.name).Any(x => x == _newName))
                    {
                        Debug.LogError($"The given name ({_newName}) is already taken.");
                        return;
                    }

                    LocalizationEditorUtilities.CreateLanguageData(_newName, _filterName, x =>
                    {
                        _cacheData.AddRange(x.answerNodeData);
                        _cacheData.AddRange(x.dropdownData);
                        _cacheData.AddRange(x.textData);
                        _cacheData.AddRange(x.textNodeData);
                        
                        _cacheData = _cacheData.OrderBy(y => y.id).ToList();
                    });
                    _creatingNewData = false;
                    _cacheData = new List<ParadoxLocalizationData>();
                    _startElement = _endElement = 0;
                    _objectNames = new List<string>();
                    
                    Repaint();
                }

                if (GUILayout.Button("Cancel"))
                    _creatingNewData = false;
            }

            else if (dataPack == null)
            {
                if (GUILayout.Button("Create Language data"))
                    _creatingNewData = true;

                if (GUILayout.Button("Load Language data"))
                {
                    LocalizationEditorUtilities.LoadLanguageData(x =>
                        {
                            _cacheData.AddRange(x.answerNodeData);
                            _cacheData.AddRange(x.dropdownData);
                            _cacheData.AddRange(x.textData);
                            _cacheData.AddRange(x.textNodeData);

                            _objectNames.AddRange(x.objectsName);

                            _endElement = _cacheData.Count - 5 <= 0 ? _cacheData.Count : 5;
                            _startElement = 0;
                        });
                }
            }

            else if (_toDelete)
            {
                if (GUILayout.Button("Delete"))
                {
                    _cacheData.Clear();
                    _objectNames.Clear();
                    _startElement = _endElement = 0;
                    LocalizationEditorUtilities.DeleteLanguage();
                    _toDelete = false;
                    Repaint();
                }

                if (GUILayout.Button("Cancel"))
                    _toDelete = false;
            }

            else
            {
                if (GUILayout.Button("Save Language data"))
                {
                    var tempString = new List<string>(_objectNames);

                    dataPack.answerNodeData = _cacheData.Where(x => x.filterType == ELanguageGraphFilter.AnswerNode).Select(x => x as ParadoxAnswerNodeLocalizationData).ToList();
                    dataPack.dropdownData = _cacheData.Where(x => x.filterType == ELanguageGraphFilter.Dropdown).Select(x => x as ParadoxDropdownLocalizationData).ToList();
                    dataPack.textData = _cacheData.Where(x => x.filterType == ELanguageGraphFilter.Text).Select(x => x as ParadoxTextLocalizationData).ToList();
                    dataPack.textNodeData = _cacheData.Where(x => x.filterType == ELanguageGraphFilter.TextNode).Select(x => x as ParadoxTextNodeLocalizationData).ToList();

                    dataPack.objectsName = tempString;

                    EditorUtility.SetDirty(dataPack);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                if (GUILayout.Button("Delete Language data"))
                    _toDelete = true;
            }

            GUILayout.EndHorizontal();

            if (dataPack != null)
            {
                GUILayout.Space(5);

                if (GUILayout.Button("Unload Language data"))
                {
                    dataPack = null;
                    _startElement = _endElement = 0;
                    _cacheData.Clear();
                    _objectNames.Clear();
                    Repaint();
                }
            }

            GUILayout.Space(10);

            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);


            if (_creatingNewData)
            {
                GUILayout.Space(10);

                GUILayout.Label(new GUIContent("Name:", "Name of the file"));
                _newName = GUILayout.TextField(_newName);

                GUILayout.Label(new GUIContent("Filter name:", "Language type of this data."));
                _filterName = GUILayout.TextField(_filterName);
            }

            GUILayout.Space(15);

            if (dataPack == null)
                DefaultView();
            else
                DataView();


            GUILayout.Space(15);
        }

        private void DefaultView()
        {
            GUILayout.BeginVertical("BOX");

            EditorGUILayout.LabelField("In the Location Manager you can create and administrate multilanguage versions of the text of yout novel, menus or all what you whant.", _description);

            GUILayout.EndVertical();
        }

        private void DataView()
        {
            GUILayout.Label($"Start: {_startElement}, End: {_endElement}");

            EditorGUILayout.BeginVertical();
            _pos = EditorGUILayout.BeginScrollView(_pos, GUILayout.Width(_width));

            if (_cacheData == null || !_cacheData.Any())
            {
                GUILayout.BeginVertical("BOX");

                GUILayout.Label("The Language data don't have any data.");

                GUILayout.EndVertical();
            }

            else
            {
                if (_cacheData.Count > 5)
                {
                    GUILayout.BeginHorizontal();

                    if (_startElement > 0)
                    {
                        if (GUILayout.Button("<< Previous"))
                        {
                            _endElement = _endElement < _cacheData.Count ? _endElement - 5 : _startElement;
                            _startElement -= 5;
                        }
                    }

                    GUILayout.FlexibleSpace();

                    if (_endElement < _cacheData.Count)
                    {
                        if (GUILayout.Button("Next >>"))
                        {
                            _startElement += 5;
                            _endElement = _cacheData.Count - (_endElement + 5) <= 0 ? _cacheData.Count : _endElement + 5;
                        }
                    }

                    GUILayout.EndHorizontal();
                }
                
                GUILayout.Space(10);

                string names = string.Empty;

                for (int i = 0; i < _objectNames.Count; i++)
                    names += i == _objectNames.Count ? $"{_objectNames[i]}.": $"{_objectNames[i]}, ";

                GUILayout.BeginVertical("BOX");
                GUILayout.Label($"Objects baked: {names}", _description);
                GUILayout.EndVertical();

                ParadoxAnswerNodeLocalizationData answerData;
                ParadoxTextNodeLocalizationData textData;
                ParadoxDropdownLocalizationData dropdownData;
                ParadoxTextLocalizationData simpleData;


                for (int i = _startElement; i < _endElement; i++)
                {
                    GUILayout.BeginVertical("BOX");

                    GUILayout.Label($"ID: {new Guid(_cacheData[i].id).ToString()}");

                    if (_cacheData[i].filterType == ELanguageGraphFilter.AnswerNode)
                    {
                        answerData = (ParadoxAnswerNodeLocalizationData)_cacheData[i];
                        answerData.data = GUILayout.TextField(answerData.data);
                    }

                    else if (_cacheData[i].filterType == ELanguageGraphFilter.TextNode)
                    {
                        textData = (ParadoxTextNodeLocalizationData)_cacheData[i];

                        GUILayout.BeginVertical("BOX");
                        ParadoxTextNodeData nodeData;

                        for (int x = 0; x < textData.data.Count; x++)
                        {
                            nodeData = textData.data[x];

                            if (nodeData.IsDialogue)
                                GUILayout.Label(textData.data[x].Character.name);

                            nodeData.Text = GUILayout.TextArea(nodeData.Text);
                            textData.data[x] = nodeData;

                            GUILayout.Space(2);
                        }

                        GUILayout.EndVertical();
                    }

                    else if (_cacheData[i].filterType == ELanguageGraphFilter.Dropdown)
                    {
                        dropdownData = (ParadoxDropdownLocalizationData)_cacheData[i];
                        
                        for (int y = 0; y < dropdownData.data.Length; y++)
                        {
                            dropdownData.data[y] = GUILayout.TextField(dropdownData.data[y]);
                            GUILayout.Space(2);
                        }
                    }

                    else
                    {
                        simpleData = (ParadoxTextLocalizationData)_cacheData[i];

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("Text:");
                        simpleData.data = GUILayout.TextField(simpleData.data);

                        GUILayout.EndHorizontal();
                    }

                    if (GUILayout.Button("Unsuscribe data"))
                    {
                        int tempI = i;
                        OnRepaint += () => _cacheData.RemoveAt(tempI);
                    }

                    GUILayout.EndVertical();

                    GUILayout.Space(5);
                }
            }

            GUILayout.Space(10);

            if (_addingGraph)
            {
                GUILayout.Label("Graph:");
                _graphToAdd = (EngineGraph)EditorGUILayout.ObjectField(_graphToAdd, typeof(EngineGraph), false);


                GUILayout.BeginHorizontal("BOX");

                if (GUILayout.Button("Bake graph data"))
                {
                    if (_graphToAdd == null)
                    {
                        Debug.Log("Paradox Engine: The graph field can't be null.");
                        return;
                    }

                    BakeGraph();
                }

                if (GUILayout.Button("Cancel"))
                {
                    _graphToAdd = null;
                    _addingGraph = false;
                }

                GUILayout.EndHorizontal();
            }

            else if (_addingText)
            {
                GUILayout.Label("Text component:");
                _textToAdd = (TextLocalizationSetter)EditorGUILayout.ObjectField(_textToAdd, typeof(TextLocalizationSetter), true);

                GUILayout.BeginHorizontal("BOX");

                if (GUILayout.Button("Bake text data"))
                {
                    if (_textToAdd == null)
                    {
                        Debug.Log("Paradox Engine: The text field can't be null.");
                        return;
                    }

                    if (_cacheData.Any(x => new Guid(x.id).Equals(_textToAdd.GetLocalizationID())))
                    {
                        Debug.LogError("Paradox Engine: The text is already in the data.");
                        return;
                    }

                    BakeText();
                }

                if (GUILayout.Button("Bake all"))
                {
                    var temp = GameObject.FindObjectsOfType<TextLocalizationSetter>().ToList();

                    for (int i = 0; i < temp.Count; i++)
                    {
                        if (_cacheData.Any(x => new Guid(x.id).Equals(temp[i].GetLocalizationID())))
                        {
                            Debug.LogError("Paradox Engine: The text is already in the data.");
                            continue;
                        }

                        BakeText(temp[i]);
                    }
                }

                if (GUILayout.Button("Cancel"))
                {
                    _textToAdd = null;
                    _addingText = false;
                }

                GUILayout.EndHorizontal();
            }

            else if (_addingDropdown)
            {
                GUILayout.Label("Dropdown component:");
                _dropwdownToAdd = (DropdownLocalizationSetter)EditorGUILayout.ObjectField(_dropwdownToAdd, typeof(DropdownLocalizationSetter), true);

                GUILayout.BeginHorizontal("BOX");

                if (GUILayout.Button("Bake dropdown data"))
                {
                    if (_dropwdownToAdd == null)
                    {
                        Debug.Log("Paradox Engine: The text field can't be null.");
                        return;
                    }

                    if (_cacheData.Any(x => new Guid(x.id).Equals(_dropwdownToAdd.GetLocalizationID())))
                    {
                        Debug.LogError("Paradox Engine: The dropdown is already in the data.");
                        return;
                    }

                    BakeDropDown();
                }

                if (GUILayout.Button("Bake all"))
                {
                    var temp = GameObject.FindObjectsOfType<DropdownLocalizationSetter>().ToList();

                    for (int i = 0; i < temp.Count; i++)
                    {
                        if (_cacheData.Any(x => new Guid(x.id).Equals(temp[i].GetLocalizationID())))
                        {
                            Debug.LogError("Paradox Engine: The dropdown is already in the data.");
                            continue;
                        }

                        BakeDropDown(temp[i]);
                    }
                }

                if (GUILayout.Button("Cancel"))
                {
                    _dropwdownToAdd = null;
                    _addingDropdown = false;
                }

                GUILayout.EndHorizontal();
            }

            else
            {
                if (GUILayout.Button("Add Graph"))
                    _addingGraph = true;

                GUILayout.Space(5);

                if (GUILayout.Button("Add Text component"))
                    _addingText = true;

                GUILayout.Space(5);

                if (GUILayout.Button("Add Dropddown component"))
                    _addingDropdown = true;
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            OnRepaint();
            OnRepaint = delegate { };
        }

        public void BakeGraph()
        {
            var nodes = _graphToAdd.nodes.Where(x => x.nodeType == EnumNodeType.Answer || x.nodeType == EnumNodeType.Text);

            bool alreadyArchived = _objectNames.Any(x => x == _graphToAdd.graphName);

            if (!alreadyArchived)
                _objectNames.Add(_graphToAdd.graphName);
            else
                Debug.Log("Paradox Engine: Added all the nodes not archived in the data.");


            NText textNode;
            NAnswer answerNode;

            foreach (var node in nodes)
            {
                if (_cacheData.Any(x => new Guid(x.id).Equals(node.GetLocalizationID())))
                {
                    Debug.Log($"The node {node.name}: InstanceID: {node.GetInstanceID()} is not added, because already exist someone with the same ID.");
                    return;
                }

                if (node.nodeType == EnumNodeType.Text)
                {
                    textNode = (NText)node;

                    var textData = new ParadoxTextNodeLocalizationData();
                    textData.id = textNode.GetLocalizationID().ToByteArray();
                    textData.data = textNode.data;
                    EditorUtility.SetDirty(textNode);
                    
                    
                    _cacheData.Add(textData);
                }

                else
                {
                    answerNode = (NAnswer)node;

                    var answerData = new ParadoxAnswerNodeLocalizationData();
                    answerData.id = answerNode.GetLocalizationID().ToByteArray();
                    answerData.data = answerNode.answer;
                    EditorUtility.SetDirty(answerNode);

                    _cacheData.Add(answerData);
                }
            }

            _graphToAdd = null;
            _addingGraph = false;

            _endElement = _cacheData.Count - (_endElement + 5) <= 0 ? _cacheData.Count : _endElement + 5;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Repaint();
        }

        public void BakeText()
        {
            var text = new ParadoxTextLocalizationData();

            _objectNames.Add(_textToAdd.name);

            text.id = _textToAdd.GetLocalizationID().ToByteArray();
            text.data = _textToAdd.GetTextValue();

            _textToAdd = null;
            _addingText = false;

            EditorUtility.SetDirty(_textToAdd);
            _cacheData.Add(text);

            _endElement = _cacheData.Count - (_endElement + 5) <= 0 ? _cacheData.Count : _endElement + 5;
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            Repaint();
        }

        public void BakeText(TextLocalizationSetter setter)
        {
            var text = new ParadoxTextLocalizationData();

            _objectNames.Add(setter.name);

            text.id = setter.GetLocalizationID().ToByteArray();
            text.data = setter.GetTextValue();

            EditorUtility.SetDirty(setter);
            setter = null;
            _addingText = false;

            _cacheData.Add(text);

            _endElement = _cacheData.Count - (_endElement + 5) <= 0 ? _cacheData.Count : _endElement + 5;
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            Repaint();
        }

        public void BakeDropDown()
        {
            var dropdown = new ParadoxDropdownLocalizationData();

            _objectNames.Add(_dropwdownToAdd.name);

            dropdown.data = _dropwdownToAdd.GetDropdownOptions().ToArray();
            dropdown.id = _dropwdownToAdd.GetLocalizationID().ToByteArray();

            _cacheData.Add(dropdown);
            EditorUtility.SetDirty(_dropwdownToAdd);
            _dropwdownToAdd = null;
            _addingDropdown = false;

            _endElement = _cacheData.Count - (_endElement + 5) <= 0 ? _cacheData.Count : _endElement + 5;
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            Repaint();
        }

        public void BakeDropDown(DropdownLocalizationSetter setter)
        {
            var dropdown = new ParadoxDropdownLocalizationData();

            _objectNames.Add(setter.name);

            dropdown.data = setter.GetDropdownOptions().ToArray();
            dropdown.id = setter.GetLocalizationID().ToByteArray();

            _cacheData.Add(dropdown);
            EditorUtility.SetDirty(setter);
            setter = null;
            _addingDropdown = false;

            _endElement = _cacheData.Count - (_endElement + 5) <= 0 ? _cacheData.Count : _endElement + 5;
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            Repaint();
        }
    }
}