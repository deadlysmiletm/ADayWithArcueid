using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;
using ParadoxEngine;

public enum OperationType
{
    Equal,
    Greater,
    Less,
}

[System.Serializable]
public class NConditionalBranch : NTemplate
{
    public MultiNodeOutput multiOutput;
    [System.NonSerialized] private bool _initialized = false;

    public NConditionalBranch()
    {
        input = new NodeInput();
        multiOutput = new MultiNodeOutput();

        multiOutput.outputNode = new List<NTemplate>();
        input.inputNode = new List<NTemplate>();
    }

#if UNITY_EDITOR
    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Branch_Condition;
        myRect = new Rect(10f, 10f, 110f, 55f);
    }

    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("Branch"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("BranchSelected"));
    }

    public override void UpdateNode(Event e, Rect viewRect) => base.UpdateNode(e, viewRect);

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin) => base.UpdateNodeGUI(e, viewRect, viewSkin);
#endif

    public override void Execute()
    {
        if (!_initialized)
            DialogueDatabase.activeGraphPlayer.ChangeNode(SelectCondition());
    }

    public override void EndState() => _initialized = false;

    private NTemplate SelectCondition()
    {
        _initialized = true;
        NCondition elseNode = default;

        foreach (NCondition node in multiOutput.outputNode)
        {
            if (node.elseConditional)
            {
                elseNode = node;
                continue;
            }

            if (node.IsConditionCompleted())
                return node;
        }

        return elseNode;

        throw new System.Exception("Paradox Engine: The Conditional Branch State (InstanceID: " + this.GetInstanceID() + "of the graph " + parentGraph.graphName + " don't have a condition for this situation.");
    }
}