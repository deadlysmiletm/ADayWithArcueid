using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxEngine;
using ParadoxEngine.Utilities;
using System;

[System.Serializable]
public class NodeInput
{
    public bool hasSomething = false;
    public List<NTemplate> inputNode;
}

[System.Serializable]
public class NodeOutput
{
    public bool isOccupied = false;
    public NTemplate outputNode;
}

public class NTemplate : ScriptableObject
{
    public Rect myRect;
    public string nodeName;
    public float timeDuration;
    [System.NonSerialized] public bool isPanning;

    public bool isSelected = false;
    public EngineGraph parentGraph;
    public EnumNodeType nodeType;
    public NodeInput input;
    public NodeOutput output;
    public float inputPosition;
    public float outputPosition;

    [SerializeField] private byte[] _localizationID;

    protected GUISkin nodeSkin;
    [System.NonSerialized] public bool endedInstruction = false;
    [System.NonSerialized] protected bool _canDrag = false;

    public NTemplate()
    {
        input = new NodeInput();
        output = new NodeOutput();

        input.inputNode = new List<NTemplate>();
    }

    public virtual void InitNode() {}

    public virtual void UpdateNode(Event e, Rect viewRect)
    {
        ProcessEvent(e, viewRect);
    }


    public Guid GetLocalizationID()
    {
        if (_localizationID.Length == 0)
            _localizationID = Guid.NewGuid().ToByteArray();

        return new Guid(_localizationID);
    }

    public void RebuildLocalizationID() => _localizationID = Guid.NewGuid().ToByteArray();


#if UNITY_EDITOR
    public virtual void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
    {
        DrawInputLines();

        ProcessEvent(e, viewRect);

        NodeStyle(viewSkin);

        EditorUtility.SetDirty(this);

        InputDefinition(viewSkin);

        OutputDefinition(viewSkin);
    }
#endif

    protected virtual void InputDefinition(GUISkin viewSkin)
    {
        if (GUI.Button(new Rect(myRect.x - 24f, myRect.y + (myRect.height * 0.5f) - 12f, 24f, 24f), "", viewSkin.GetStyle("NodeInput")))
        {
            if (parentGraph != null)
                CallConnection();
        }
    }

    public virtual void CallConnection()
    {
        if (ApproveConnection())
        {
            if (parentGraph.connectionNode.output.isOccupied)
            {
                parentGraph.connectionNode.output.outputNode.input.inputNode.Remove(parentGraph.connectionNode);
            }

            input.inputNode.Add(parentGraph.connectionNode);
            input.hasSomething = input.inputNode.Count > 0 ? true : false;

            if (parentGraph.connectionNode.nodeType == EnumNodeType.Branch_Question)
            {
                NQuestionBranch connectionNode = (NQuestionBranch)parentGraph.connectionNode;

                connectionNode.multiOutput.outputNode.Add(this);
                connectionNode.multiOutput.hasSomething = true;
            }
            else
            {
                parentGraph.connectionNode.output.outputNode = this;
                parentGraph.connectionNode.output.isOccupied = true;
            }

            parentGraph.wantsConnection = false;
            parentGraph.connectionNode = null;
        }
        else
        {
            input.hasSomething = input.inputNode != null ? true : false;

            parentGraph.wantsConnection = false;
            parentGraph.connectionNode = null;
        }
    }

    protected virtual void OutputDefinition(GUISkin viewSkin)
    {
        if (GUI.Button(new Rect(myRect.x + myRect.width, myRect.y + (myRect.height * 0.5f) - 12f, 24f, 24f), " ", viewSkin.GetStyle("NodeOutput")))
        {
            if (parentGraph != null)
            {
                parentGraph.wantsConnection = true;
                parentGraph.connectionNode = this;
            }
        }
    }

    protected virtual bool ApproveConnection()
    {
        if (parentGraph.connectionNode != null)
        {
            if (parentGraph.connectionNode.nodeType == EnumNodeType.Branch_Question || parentGraph.connectionNode.nodeType == EnumNodeType.Branch_Condition)
                return false;
            else
                return true;
        }
        else
            return false;
    }

#if UNITY_EDITOR
    protected virtual void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("DefaultNode"));

        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("NodeSelected"));
    }
#endif

    void ProcessEvent(Event e, Rect viewRect)
    {
        if (isPanning)
        {
            myRect.x += e.delta.x;
            myRect.y += e.delta.y;
        }

        else if (isSelected && e.button == 0)
        {
            if (e.type == EventType.MouseDown)
            {
                _canDrag = true;
                GUI.FocusControl(null);
            }

            if (_canDrag && e.type == EventType.MouseDrag)
                myRect.position += e.delta;

            if (e.type == EventType.MouseUp)
                _canDrag = false;
        }
    }


#if UNITY_EDITOR
    void DrawInputLines()
    {
        if (input.hasSomething && input.inputNode != null)
        {
            GUI.depth = 0;
            DrawConnections(input, 1f);
        }
        else
        {
            input.hasSomething = false;
        }
    }

    protected virtual void DrawConnections(NodeInput input, float inputID)
    {
        for (int i = 0; i < input.inputNode.Count; i++)
        {
            Vector3 startPos = new Vector3(input.inputNode[i].myRect.x + input.inputNode[i].myRect.width + 24f, input.inputNode[i].myRect.y + (input.inputNode[i].myRect.height * 0.5f), 0f);
            Vector3 endPos = new Vector3(myRect.x - 24f, (input.inputNode[i].output.outputNode.myRect.y + (input.inputNode[i].output.outputNode.myRect.height * 0.5f) * inputID), 0f);
            Vector3 startTan = startPos + Vector3.right * (-50 + 100 * 1.5f) + Vector3.up * (-50 + 100 * 0.5f);
            Vector3 endTan = endPos + Vector3.right * (-50 + 100 * -.5f) + Vector3.up * (-50 + 100 * 0.5f);
            Color shadowCol = new Color(0, 0, 0, 0.6f);

            for (int j = 0; j < 2; j++)
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (j + 1) * 5);

            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.green, null, 2);
        }
    }
#endif

    public virtual void Execute() { }
    public virtual void EndState() => endedInstruction = false;

    public virtual void ChangeColor() { }

}