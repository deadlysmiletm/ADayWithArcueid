using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace ParadoxEngine.Utilities
{
    public static class EngineGraphEditorUtilities
    {
        public static void OpenNewGraphPanel()
        {
            VToolbar.showDialogue = true;
        }

        public static void CreateNewGraph(string graphName)
        {
            EngineGraph currentGraph = (EngineGraph)ScriptableObject.CreateInstance<EngineGraph>();
            if (currentGraph != null)
            {
                currentGraph.graphName = graphName;
                currentGraph.Initialize();

                string path = "Assets/Paradox Engine/Graph/Resources/Data/" + graphName + ".asset";
                AssetDatabase.CreateAsset(currentGraph, path);
                EngineGraphCacheUtilities.SaveSessionCache(path);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EngineGraphWindow currentWindow = (EngineGraphWindow)EditorWindow.GetWindow<EngineGraphWindow>();

                if (currentWindow != null)
                    currentWindow.currentGraph = currentGraph;

                CreateNode(currentWindow.currentGraph, EnumNodeType.Start, new Vector2(104, 136));
                CreateNode(currentWindow.currentGraph, EnumNodeType.End, new Vector2(307, 136));
            }
            else
                EditorUtility.DisplayDialog("Paradox Engine", "Graph creation failed.", "OK");
        }

        public static void LoadGraph()
        {
            string graphPath = EditorUtility.OpenFilePanel("Load Graph", Application.dataPath + "/Paradox Engine/Graph/Resources/Data/", "");
            LoadSession(graphPath);
        }

        public static void LoadSession(string graphPath, bool cache = false)
        {
            EngineGraph currentGraph = null;

            if (graphPath != "")
            {
                string finalPath = graphPath;

                if (!cache)
                {
                    int appPathLen = Application.dataPath.Length;
                    finalPath = graphPath.Substring(appPathLen - 6);
                }

                currentGraph = (EngineGraph)AssetDatabase.LoadAssetAtPath(finalPath, typeof(EngineGraph));

                if (currentGraph != null)
                {
                    EngineGraphWindow currentWindow = (EngineGraphWindow)EditorWindow.GetWindow<EngineGraphWindow>();

                    if (currentWindow != null)
                    {
                        if (!cache)
                            EngineGraphCacheUtilities.SaveSessionCache(finalPath);

                        currentWindow.currentGraph = currentGraph;
                    }
                }
                else
                    EditorUtility.DisplayDialog("Paradox Engine", "Graph load failed.", "OK");

                currentGraph.selectedNode = null;
            }
        }

        public static void LoadSession(EngineGraph graph)
        {
            var currentWindow = (EngineGraphWindow)EditorWindow.GetWindow<EngineGraphWindow>();

            if (currentWindow != null)
            {
                graph.selectedNode = null;
                currentWindow.currentGraph = graph;
            }
        }

        public static void UnloadGraph()
        {
            EngineGraphWindow currentWindow = (EngineGraphWindow)EditorWindow.GetWindow<EngineGraphWindow>();

            if (currentWindow != null)
                currentWindow.currentGraph = null;

            EngineGraphCacheUtilities.ClearSessionCache();
        }

        public static void DeleteGraph()
        {
            EngineGraphWindow currentWindow = (EngineGraphWindow)EditorWindow.GetWindow<EngineGraphWindow>();

            foreach (var item in currentWindow.currentGraph.nodes)
                GameObject.DestroyImmediate(item, true);

            foreach (var param in currentWindow.currentGraph.parameters.Where(x => x.access == Parameters.ParamAccessibility.IsLocal).Where(x => x.graph == currentWindow.currentGraph))
                currentWindow.currentGraph.parameters.UnsuscribeValue(param.Name);

            var temp = currentWindow.currentGraph;
            currentWindow.currentGraph = null;
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(temp));

            AssetDatabase.Refresh();
            EngineGraphCacheUtilities.ClearSessionCache();
        }

        public static void CreateNode(EngineGraph currentGraph, EnumNodeType nodeType, Vector2 mousePos)
        {
            if (currentGraph != null)
            {
                NTemplate currentNode = null;
                switch (nodeType)
                {
                    case EnumNodeType.Text:
                        currentNode = (NText)ScriptableObject.CreateInstance<NText>();
                        currentNode.nodeName = "Text State";
                        break;
                    case EnumNodeType.Branch_Condition:
                        currentNode = (NConditionalBranch)ScriptableObject.CreateInstance<NConditionalBranch>();
                        currentNode.nodeName = "Conditional Branch State";
                        break;
                    case EnumNodeType.Condition:
                        currentNode = (NCondition)ScriptableObject.CreateInstance<NCondition>();
                        currentNode.nodeName = "Condition State";
                        break;
                    case EnumNodeType.Branch_Question:
                        currentNode = (NQuestionBranch)ScriptableObject.CreateInstance<NQuestionBranch>();
                        currentNode.nodeName = "Question Branch State";
                        break;
                    case EnumNodeType.Answer:
                        currentNode = (NAnswer)ScriptableObject.CreateInstance<NAnswer>();
                        currentNode.nodeName = "Answer State";
                        break;
                    case EnumNodeType.Delay:
                        currentNode = (NDelay)ScriptableObject.CreateInstance<NDelay>();
                        currentNode.nodeName = "Delay State";
                        break;
                    case EnumNodeType.Start:
                        currentNode = (NStart)ScriptableObject.CreateInstance<NStart>();
                        currentNode.nodeName = "Start State";
                        break;
                    case EnumNodeType.End:
                        currentNode = (NEnd)ScriptableObject.CreateInstance<NEnd>();
                        currentNode.nodeName = "End State";
                        break;
                    case EnumNodeType.Clear:
                        currentNode = (NClearText)ScriptableObject.CreateInstance<NClearText>();
                        currentNode.nodeName = "Clear Text State";
                        break;
                    case EnumNodeType.Change_Flow_Chart:
                        currentNode = (NChangeFlowChart)ScriptableObject.CreateInstance<NChangeFlowChart>();
                        currentNode.nodeName = "Change Flow Chart State";
                        break;
                    case EnumNodeType.Set_Param:
                        currentNode = (NSetParam)ScriptableObject.CreateInstance<NSetParam>();
                        currentNode.nodeName = "Set Parameter State";
                        break;
                    case EnumNodeType.Play_Music:
                        currentNode = (NPlayMusic)ScriptableObject.CreateInstance<NPlayMusic>();
                        currentNode.nodeName = "Play Music State";
                        break;
                    case EnumNodeType.Stop_Music:
                        currentNode = (NStopMusic)ScriptableObject.CreateInstance<NStopMusic>();
                        currentNode.nodeName = "Stop Music State";
                        break;
                    case EnumNodeType.Play_Sound:
                        currentNode = (NPlaySound)ScriptableObject.CreateInstance<NPlaySound>();
                        currentNode.nodeName = "Play Sound FX State";
                        break;
                    case EnumNodeType.Stop_Sound:
                        currentNode = (NStopSound)ScriptableObject.CreateInstance<NStopSound>();
                        currentNode.nodeName = "Stop Sound FX State";
                        break;
                    case EnumNodeType.Play_Voice:
                        currentNode = (NPlayVoice)ScriptableObject.CreateInstance<NPlayVoice>();
                        currentNode.nodeName = "Play Voice State";
                        break;
                    case EnumNodeType.Stop_Voice:
                        currentNode = (NStopVoice)ScriptableObject.CreateInstance<NStopVoice>();
                        currentNode.nodeName = "Stop Voice State";
                        break;
                    case EnumNodeType.Hide_Text_Container:
                        currentNode = (NHideTextContainer)ScriptableObject.CreateInstance<NHideTextContainer>();
                        currentNode.nodeName = "Hide Text Container State";
                        break;
                    case EnumNodeType.Show_Text_Container:
                        currentNode = (NShowTextContainer)ScriptableObject.CreateInstance<NShowTextContainer>();
                        currentNode.nodeName = "Show Text Container State";
                        break;
                    case EnumNodeType.Change_Background:
                        currentNode = (NChangeBackground)ScriptableObject.CreateInstance<NChangeBackground>();
                        currentNode.nodeName = "Change Background State";
                        break;
                    case EnumNodeType.Show_Character:
                        currentNode = (NShowCharacter)ScriptableObject.CreateInstance<NShowCharacter>();
                        currentNode.nodeName = "Show Character State";
                        break;
                    case EnumNodeType.Hide_Character:
                        currentNode = (NHideCharacter)ScriptableObject.CreateInstance<NHideCharacter>();
                        currentNode.nodeName = "Hide Character State";
                        break;
                    case EnumNodeType.Change_Character_Sprite:
                        currentNode = (NChangeCharacterSprite)ScriptableObject.CreateInstance<NChangeCharacterSprite>();
                        currentNode.nodeName = "Change Character Sprite State";
                        break;
                    case EnumNodeType.Move_Character:
                        currentNode = (NMoveCharacter)ScriptableObject.CreateInstance<NMoveCharacter>();
                        currentNode.nodeName = "Move Character State";
                        break;
                    case EnumNodeType.Trigger_Event:
                        currentNode = (NTriggerEvent)ScriptableObject.CreateInstance<NTriggerEvent>();
                        currentNode.nodeName = "Trigger Event State";
                        break;
                }

                if (currentNode != null)
                {
                    currentNode.name = currentNode.nodeName;
                    currentNode.InitNode();
                    currentNode.myRect.x = mousePos.x;
                    currentNode.myRect.y = mousePos.y;
                    currentNode.parentGraph = currentGraph;
                    currentGraph.nodes.Add(currentNode);

                    if (currentGraph.wantsConnection)
                        currentNode.CallConnection();

                    AssetDatabase.AddObjectToAsset(currentNode, currentGraph);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }

        public static void DeleteNode(EngineGraph graph, int nodeID)
        {

            if (graph.nodes.Count <= nodeID)
                return;

            NTemplate deleteNode = graph.nodes[nodeID];

            if (deleteNode == null)
                return;

            if (deleteNode.nodeType == EnumNodeType.Answer)
            {
                for (int i = 0; i < deleteNode.input.inputNode.Count; i++)
                {
                    NQuestionBranch questionNode = (NQuestionBranch)deleteNode.input.inputNode[i];

                    questionNode.multiOutput.outputNode.Remove(graph.nodes[nodeID]);
                    questionNode.multiOutput.hasSomething = questionNode.multiOutput.outputNode.Count == 0 ? false : true;
                }
            }

            else
            {
                for (int i = 0; i < deleteNode.input.inputNode.Count; i++)
                {
                    deleteNode.input.inputNode[i].output.outputNode = null;
                    deleteNode.input.inputNode[i].output.isOccupied = false;
                }
            }

            if (deleteNode.nodeType == EnumNodeType.Branch_Question)
            {
                NQuestionBranch questionNode = (NQuestionBranch)deleteNode;

                for (int i = 0; i < questionNode.multiOutput.outputNode.Count; i++)
                {
                    questionNode.multiOutput.outputNode[i].input.inputNode.Remove(questionNode);
                    questionNode.multiOutput.outputNode[i].input.hasSomething = questionNode.multiOutput.outputNode[i].input.inputNode.Count > 0 ? true : false;
                }
            }

            else if (deleteNode.nodeType == EnumNodeType.Branch_Condition)
            {
                NConditionalBranch conditionalNode = (NConditionalBranch)deleteNode;

                for (int i = 0; i < conditionalNode.multiOutput.outputNode.Count; i++)
                {
                    conditionalNode.multiOutput.outputNode[i].input.inputNode.Remove(conditionalNode);
                    conditionalNode.multiOutput.outputNode[i].input.hasSomething = conditionalNode.multiOutput.outputNode[i].input.inputNode.Count > 0 ? true : false;
                }
            }

            else
            {
                if (deleteNode.output.outputNode != null)
                {
                    deleteNode.output.outputNode.input.inputNode.Remove(deleteNode);
                    deleteNode.output.outputNode.input.hasSomething = deleteNode.output.outputNode.input.inputNode.Count > 0 ? true : false;
                }
            }

            graph.nodes.RemoveAt(nodeID);
            GameObject.DestroyImmediate(deleteNode, true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void DisconnectInput(EngineGraph graph, int nodeID)
        {
            if (graph.nodes[nodeID] != null)
            {
                if (graph.nodes[nodeID].nodeType == EnumNodeType.Answer)
                {
                    for (int i = 0; i < graph.nodes[nodeID].input.inputNode.Count; i++)
                    {
                        NQuestionBranch questionNode = (NQuestionBranch)graph.nodes[nodeID].input.inputNode[i];

                        questionNode.multiOutput.outputNode.RemoveAt(questionNode.multiOutput.outputNode.IndexOf(graph.nodes[nodeID]));
                        questionNode.multiOutput.hasSomething = questionNode.multiOutput.outputNode.Count == 0 ? false : true;
                    }
                }

                else if (graph.nodes[nodeID].nodeType == EnumNodeType.Condition)
                {
                    for (int i = 0; i < graph.nodes[nodeID].input.inputNode.Count; i++)
                    {
                        NConditionalBranch conditionNode = (NConditionalBranch)graph.nodes[nodeID].input.inputNode[i];

                        conditionNode.multiOutput.outputNode.RemoveAt(conditionNode.multiOutput.outputNode.IndexOf(graph.nodes[nodeID]));
                        conditionNode.multiOutput.hasSomething = conditionNode.multiOutput.outputNode.Count == 0 ? false : true;
                    }
                }

                else
                {
                    for (int i = 0; i < graph.nodes[nodeID].input.inputNode.Count; i++)
                    {
                        graph.nodes[nodeID].input.inputNode[i].output.outputNode = null;
                        graph.nodes[nodeID].input.inputNode[i].output.isOccupied = false;
                    }
                }

                graph.nodes[nodeID].input.inputNode = new List<NTemplate>();
                graph.nodes[nodeID].input.hasSomething = false;
            }
        }

        public static void DisconnectOutput(EngineGraph graph, int nodeID)
        {
            if (graph.nodes[nodeID] != null)
            {
                if (graph.nodes[nodeID].nodeType == EnumNodeType.Branch_Question)
                {
                    NQuestionBranch multiNode = (NQuestionBranch)graph.nodes[nodeID];

                    if (multiNode.multiOutput.hasSomething)
                    {
                        for (int i = 0; i < multiNode.multiOutput.outputNode.Count; i++)
                        {
                            multiNode.multiOutput.outputNode[i].input.inputNode.Remove(multiNode);

                            multiNode.multiOutput.outputNode[i].input.hasSomething = multiNode.multiOutput.outputNode[i].input.inputNode.Count > 0 ? true : false;
                        }

                        multiNode.multiOutput.outputNode = new List<NTemplate>();
                        multiNode.multiOutput.hasSomething = false;
                    }
                }

                else if (graph.nodes[nodeID].nodeType == EnumNodeType.Branch_Condition)
                {
                    NConditionalBranch multiNode = (NConditionalBranch)graph.nodes[nodeID];

                    if (multiNode.multiOutput.hasSomething)
                    {
                        for (int i = 0; i < multiNode.multiOutput.outputNode.Count; i++)
                        {
                            multiNode.multiOutput.outputNode[i].input.inputNode.Remove(multiNode);

                            multiNode.multiOutput.outputNode[i].input.hasSomething = multiNode.multiOutput.outputNode[i].input.inputNode.Count > 0 ? true : false;
                        }

                        multiNode.multiOutput.outputNode = new List<NTemplate>();
                        multiNode.multiOutput.hasSomething = false;
                    }
                }

                else
                {
                    if (graph.nodes[nodeID].output.outputNode != null)
                    {
                        graph.nodes[nodeID].output.outputNode.input.inputNode.Remove(graph.nodes[nodeID]);
                        graph.nodes[nodeID].output.outputNode.input.hasSomething = graph.nodes[nodeID].output.outputNode.input.inputNode.Count > 0 ? true : false;

                        graph.nodes[nodeID].output.outputNode = null;
                        graph.nodes[nodeID].output.isOccupied = false;
                    }
                }
            }

        }

        public static void DisconnectAll(EngineGraph graph, int nodeID)
        {
            DisconnectInput(graph, nodeID);
            DisconnectOutput(graph, nodeID);
        }

        public static void DrawGrid(Rect viewRect, float gridSpacing, float gridOpacity, Color gridColor, EngineGraph currentGraph)
        {
            int widthDivs = Mathf.CeilToInt(10000 / gridSpacing);
            int heightDivs = Mathf.CeilToInt(10000 / gridSpacing);

            Vector2 offset = Vector2.zero;

            if (currentGraph != null)
                offset = currentGraph.offset;

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            for (int x = -widthDivs; x < widthDivs; x++)
                Handles.DrawLine(new Vector3((gridSpacing * x) + offset.x, 0, 0), new Vector3((gridSpacing * x) + offset.x, viewRect.height, 0));

            for (int y = -heightDivs; y < heightDivs; y++)
                Handles.DrawLine(new Vector3(0, (gridSpacing * y) + offset.y, 0), new Vector3(viewRect.width, (gridSpacing * y) + offset.y, 0));

            Handles.EndGUI();
        }

        public static void CreateContainer(GameObject prefab, GameObject canvas, string name)
        {
            var temp = GameObject.Instantiate(prefab);
            temp.transform.SetParent(canvas.transform, false);
            temp.name = name;
        }

        public static void Show(this GenericMenu menu, Vector2 pos, float minWidth = 40)
        {
            menu.DropDown(new Rect(pos, Vector2.zero));
        }

        public static void NodeSettingCallback(this NTemplate node)
        {
            EnginePopupWindow.InitEngineGraphPopupWindow(node);
        }
    }

}