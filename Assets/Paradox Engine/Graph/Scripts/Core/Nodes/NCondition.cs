using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;
using ParadoxEngine.Utilities.Parameters;
using System.Linq;

public class NCondition : NTemplate
{
    [SerializeField] public ConditionParam data;
    public bool elseConditional = false;

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Condition;
        myRect = new Rect(10f, 10f, 110f, 55f);
        data = new ConditionParam();
    }

#if UNITY_EDITOR
    public override void UpdateNode(Event e, Rect viewRect) => base.UpdateNode(e, viewRect);

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin) => base.UpdateNodeGUI(e, viewRect, viewSkin);

    public override void CallConnection()
    {
        if (ApproveConnection())
        {
            NConditionalBranch connectionNode = (NConditionalBranch)parentGraph.connectionNode;

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
            if (parentGraph.connectionNode.nodeType == EnumNodeType.Branch_Condition && !input.inputNode.Any(x => x == parentGraph.connectionNode))
                return true;
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
            NConditionalBranch output = (NConditionalBranch)input.inputNode[i];
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

    public bool IsConditionCompleted()
    {
        if (elseConditional)
            return true;

        int completed = 0;

        foreach (var condition in data)
        {
            if (!parentGraph.parameters.Any(x => x.Name == condition.Name))
                return false;

            var param = parentGraph.parameters.Where(x => x.Name == condition.Name).First();


            if (param.Type == ParamVariableType.Int || param.Type == ParamVariableType.Float)
            {
                if (NumCondition(parentGraph.parameters.GetInt(param.Name).Value, condition.CompareType, data.GetInt(param.Name)))
                    completed++;
            }

            else if (param.Type == ParamVariableType.String)
            {
                if (string.Equals(parentGraph.parameters.GetString(param.Name).Value, data.GetString(param.Name)))
                    completed++;
            }

            else if (data.GetBool(param.Name) == parentGraph.parameters.GetBool(param.Name).Value)
                completed++;
        }

        return completed == data.Count();
    }

    private bool NumCondition(float value, EnumCompareType compareType, float conditionValue)
    {
        if (compareType == EnumCompareType.Equal)
            return value == conditionValue;

        if (compareType == EnumCompareType.Greater)
            return value > conditionValue;

        else
            return value < conditionValue;
    }
}
