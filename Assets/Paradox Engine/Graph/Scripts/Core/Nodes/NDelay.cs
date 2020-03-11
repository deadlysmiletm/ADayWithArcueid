using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;

[System.Serializable]
public class NDelay : NTemplate {

    public float delay;
    [System.NonSerialized] private float _delay;

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Delay;
        myRect = new Rect(10f, 10f, 110f, 55f);
    }

#if UNITY_EDITOR
    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("Effect"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("EffectSelected"));
    }

    public override void UpdateNode(Event e, Rect viewRect) => base.UpdateNode(e, viewRect);

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin) => base.UpdateNodeGUI(e, viewRect, viewSkin);
#endif

    public override void Execute()
    {
        if (!endedInstruction)
        {
            _delay = delay;
            endedInstruction = true;
        }

        if (_delay > 0)
            _delay -= Time.deltaTime;

        else
        {
            _delay = delay;
            endedInstruction = false;
            DialogueDatabase.activeGraphPlayer.ChangeNode(output.outputNode);
        }
    }
}
