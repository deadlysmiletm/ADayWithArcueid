using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using ParadoxEngine.Utilities;
using System;
using System.Linq;

public class NAnswer : NTemplate
{
    public Button answerButton;
    public string answer;
    public Vector2 buttonPosition;
    public bool customKey;
    public KeyCode myKey;

    //Uso exclusivo por parte del QuestionNode
    [HideInInspector] public int index;

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Answer;
        myRect = new Rect(10f, 10f, 110f, 55f);
    }


#if UNITY_EDITOR
    public override void UpdateNode(Event e, Rect viewRect) => base.UpdateNode(e, viewRect);

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin) => base.UpdateNodeGUI(e, viewRect, viewSkin);

    public override void CallConnection()
    {
        if (ApproveConnection())
        {
            NQuestionBranch connectionNode = (NQuestionBranch)parentGraph.connectionNode;

            input.inputNode.Add(connectionNode);
            input.hasSomething = input.inputNode != null ? true : false;

            if (!connectionNode.multiOutput.outputNode.Contains(this))
            {
                connectionNode.multiOutput.outputNode.Add(this);

                connectionNode.multiOutput.hasSomething = connectionNode.multiOutput.outputNode.Count > 0 ? true : false;
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

    protected override bool ApproveConnection()
    {
        if (parentGraph.connectionNode != null)
        {
            if (parentGraph.connectionNode && !input.inputNode.Any(x => x == parentGraph.connectionNode))
                return parentGraph.connectionNode.nodeType == EnumNodeType.Branch_Question;
        }

        return false;
    }
#endif


    public override void Execute() => DialogueDatabase.activeGraphPlayer.ChangeNode(output.outputNode);


#if UNITY_EDITOR
    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("Answer"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("AnswerSelected"));
    }

    protected override void DrawConnections(NodeInput input, float inputID)
    {
        Handles.BeginGUI();
        Handles.color = Color.white;

        for (int i = 0; i < input.inputNode.Count; i++)
        {
            NQuestionBranch output = (NQuestionBranch)input.inputNode[i];
            int outputID = output.multiOutput.outputNode.IndexOf(this);

            Vector3 startPos = new Vector3(input.inputNode[i].myRect.x + input.inputNode[i].myRect.width + 24f, input.inputNode[i].myRect.y + (input.inputNode[i].myRect.height * 0.5f), 0f);
            Vector3 endPos = endPos = new Vector3(myRect.x - 24f, (output.multiOutput.outputNode[outputID].myRect.y + (output.multiOutput.outputNode[outputID].myRect.height * 0.5f) * inputID), 0f);
            Vector3 startTan = startPos + Vector3.right * (-50 + 100 * 1.5f) + Vector3.up * (-50 + 100 * 0.5f);
            Vector3 endTan = endPos + Vector3.right * (-50 + 100 * -.5f) + Vector3.up * (-50 + 100 * 0.5f);
            Color shadowCol = new Color(0, 0, 0, 0.6f); ;

            for (int j = 0; j < 2; j++)
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (j + 1) * 5);

            Handles.DrawBezier(startPos, endPos, startTan, endTan, new Color(0.51f, 0.51f, 1), null, 2);
        }

        Handles.EndGUI();
    }
#endif
}
