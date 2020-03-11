using UnityEngine;
using ParadoxEngine.Utilities;

[System.Serializable]
public class NStart : NTemplate
{
    public KeyCode key;

    public NStart() => output = new NodeOutput();

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Start;
        myRect = new Rect(10f, 10f, 110f, 55f);
    }

#if UNITY_EDITOR
    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("StartEnd"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("StartEndSelected"));
    }

    public override void UpdateNode(Event e, Rect viewRect) => base.UpdateNode(e, viewRect);

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin) => base.UpdateNodeGUI(e, viewRect, viewSkin);

    protected override void InputDefinition(GUISkin viewSkin) {}    

    protected override void DrawConnections(NodeInput input, float inputID) {}
#endif


    public override void Execute()
    {
        if (!DialogueDatabase.activeGraphPlayer.cache)
            throw new UnityException("The cache is not asigned in the active GraphPlayer behaviour");

        if (!DialogueDatabase.activeGraphPlayer.cache.HasSave)
            SetSceneCache();

        DialogueDatabase.activeGraphPlayer.cache.EngineGraph = parentGraph;
        DialogueDatabase.activeGraphPlayer.cache.ClearText();

        if (output.outputNode != null)
            DialogueDatabase.activeGraphPlayer.ChangeNode(output.outputNode);
    }

    private void SetSceneCache()
    {
        var parameters = DialogueDatabase.parameters;
        var cache = DialogueDatabase.activeGraphPlayer.cache;

        cache.ClearData();
        cache.ClearText();
    }
}
