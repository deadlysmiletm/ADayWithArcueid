using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using ParadoxEngine.Utilities;

[System.Serializable]
public class MultiNodeOutput
{
    public bool hasSomething;
    public List<NTemplate> outputNode;
}

[System.Serializable]
public class NQuestionBranch : NTemplate
{
    public MultiNodeOutput multiOutput;
    [System.NonSerialized] private bool _initialized = false;

    public NQuestionBranch()
    {
        input = new NodeInput();
        multiOutput = new MultiNodeOutput();

        multiOutput.outputNode = new List<NTemplate>();
        input.inputNode = new List<NTemplate>();
    }

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Branch_Question;
        myRect = new Rect(10f, 10f, 110f, 55f);
    }

#if UNITY_EDITOR
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
        var temp = multiOutput.outputNode.Select(x => (NAnswer)x);

        if (!_initialized)
            SearchButtons(temp);
        else
            AnswerEventCallback(temp);
    }

    public override void EndState()
    {
        base.EndState();
        _initialized = false;
    }

    void SearchButtons(IEnumerable<NAnswer> nodes)
    {
        _initialized = true;
        DialogueDatabase.buttonsActive = new List<Button>();

        foreach (var node in nodes)
        {
            DialogueDatabase.buttonsActive.Add(DialogueDatabase.activeGraphPlayer.TakeButtonPool());

            var temp = DialogueDatabase.buttonsActive.Last().GetComponent<RectTransform>();

            temp.anchoredPosition = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(temp, node.buttonPosition, DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);
            DialogueDatabase.buttonsActive.Last().GetComponentInChildren<Text>().text = node.answer;
            DialogueDatabase.buttonsActive.Last().onClick.AddListener( () => SelectAnswer(multiOutput.outputNode.IndexOf(node)));
        }
    }

    private void AnswerEventCallback(IEnumerable<NAnswer> nodes)
    {
        foreach (var node in nodes)
            if (Input.GetKeyDown(node.myKey))
                SelectAnswer(multiOutput.outputNode.IndexOf(node));
    }

    void SelectAnswer(int id)
    {
        _initialized = false;
        var nodeSelected = multiOutput.outputNode[id];

        foreach (var item in DialogueDatabase.buttonsActive)
            DialogueDatabase.activeGraphPlayer.ReturnButtonPool(item);

        DialogueDatabase.buttonsActive = new List<Button>();

        DialogueDatabase.activeGraphPlayer.ChangeNode(nodeSelected);
    }
}
