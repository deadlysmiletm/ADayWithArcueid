using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;
using System.Linq;
using ParadoxEngine;
using ParadoxEngine.Utilities;

public class EngineGraphWindow : EditorWindow
{
    public static EngineGraphWindow currentWindow;

    public VProperty propertyView;
    public VEngineGraph graphView;
    public VToolbar toolView;
    public static event Action OnRepaintRequest = delegate { };

    public EngineGraph currentGraph = null;

    public float viewPrecentage = 0.75f;
    public float toolbarHeight = 20;

    public static void InitEditorWindow()
    {
        currentWindow = EditorWindow.GetWindow<EngineGraphWindow>();
        currentWindow.titleContent = new GUIContent("Flow Chart");
        CreateViews();
        EngineGraphCacheUtilities.LoadSessionCache();
        currentWindow.minSize = new Vector2(700, 400);
    }

    private void OnEnable()
    {
        if (propertyView == null || graphView == null || toolView == null)
        {
            CreateViews();
            return;
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;
        ProcessEvents(e);


        graphView.Execute(new Rect(new Vector2(0, toolbarHeight), position.size), new Rect(0f, 0.188f, viewPrecentage, 1f), e, currentGraph);
        propertyView.Execute(new Rect(position.width, toolbarHeight, position.width, position.size.y), new Rect(viewPrecentage, 0.188f, 1f - viewPrecentage, 1f), e, currentGraph);
        toolView.Execute(new Rect(Vector2.zero, new Vector2(position.size.x, toolbarHeight)), new Rect(0f, 0f, viewPrecentage, 1f), e, currentGraph);


        if (currentGraph != null)
            titleContent = new GUIContent(currentGraph.name);
        else
            titleContent = new GUIContent("Flow Chart");

        if (currentGraph != null && CheckAllInputsConnected())
        {
            GUILayout.BeginArea(new Rect(0, position.size.y - 90, toolView.viewRect.size.x, 40));
            EditorGUILayout.HelpBox("The Start state must connect to the End state in all paths.", MessageType.Error);
            GUILayout.EndArea();
        }

        else if (currentGraph != null && CheckAllElseCondition())
        {
            GUILayout.BeginArea(new Rect(0, position.size.y - 90, toolView.viewRect.size.x, 40));
            EditorGUILayout.HelpBox("One of the Conditional Branch state don't have an else condition.", MessageType.Error);
            GUILayout.EndArea();
        }

        if (e.type == EventType.Repaint)
        {
            OnRepaintRequest();
            OnRepaintRequest = delegate { };
        }

        Repaint();
    }

    private bool CheckAllInputsConnected()
    {
        return currentGraph.nodes.Where(x => x.nodeType != EnumNodeType.End && x.nodeType != EnumNodeType.Change_Flow_Chart).Where(x =>
        {
            if (x.nodeType == EnumNodeType.Branch_Condition)
                return !(x as NConditionalBranch).multiOutput.hasSomething;

            else if (x.nodeType == EnumNodeType.Branch_Question)
                return !(x as NQuestionBranch).multiOutput.hasSomething;

            return !x.output.isOccupied;
        }).Any();
    }

    private bool CheckAllElseCondition() => currentGraph.nodes.Where(x => x.nodeType == EnumNodeType.Branch_Condition).Select(x => x as NConditionalBranch).Any(x => !x.multiOutput.outputNode.Any(y => (y as NCondition).elseConditional));

    static void CreateViews()
    {
        if (currentWindow != null)
        {
            currentWindow.propertyView = CreateInstance<VProperty>();
            currentWindow.toolView = CreateInstance<VToolbar>();
            currentWindow.graphView = CreateInstance<VEngineGraph>();
        }

        else
            currentWindow = EditorWindow.GetWindow<EngineGraphWindow>();
    }

    private void OnDestroy()
    {
        Editor.DestroyImmediate(currentWindow.propertyView);
        Editor.DestroyImmediate(currentWindow.toolView);
        Editor.DestroyImmediate(currentWindow.graphView);
    }

    void ProcessEvents(Event e)
    {
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftArrow && viewPrecentage > 0.24f)
            viewPrecentage -= 0.01f;

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.RightArrow && viewPrecentage < 0.75f)
            viewPrecentage += 0.01f;
    }
}
