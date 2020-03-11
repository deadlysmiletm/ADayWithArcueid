using UnityEngine;
using ParadoxEngine.Utilities;
using ParadoxEngine.Utilities.Parameters;


public class NSetParam : NTemplate
{
    [SerializeField] public SetterParam data;

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Set_Param;
        myRect = new Rect(10f, 10f, 110f, 55f);
        data = new SetterParam();
    }

#if UNITY_EDITOR
    public override void UpdateNode(Event e, Rect viewRect) => base.UpdateNode(e, viewRect);

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin) => base.UpdateNodeGUI(e, viewRect, viewSkin);

    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("System"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("SystemSelected"));
    }
#endif

    public override void Execute()
    {
        if (endedInstruction)
            DialogueDatabase.activeGraphPlayer.ChangeNode(output.outputNode);

        else
        {
            foreach (var param in data)
            {
                if (param.VarType == ParamVariableType.Int)
                {
                    if (param.ModType == EnumModType.Swap)
                        parentGraph.parameters.UpdateValue(param.Name, data.GetInt(param.Name));
                    else if (param.ModType == EnumModType.Add)
                        parentGraph.parameters.UpdateValue(param.Name, parentGraph.parameters.GetInt(param.Name).Value + data.GetInt(param.Name));
                    else
                        parentGraph.parameters.UpdateValue(param.Name, parentGraph.parameters.GetInt(param.Name).Value - data.GetInt(param.Name));
                }

                else if (param.VarType == ParamVariableType.Float)
                {
                    if (param.ModType == EnumModType.Swap)
                        parentGraph.parameters.UpdateValue(param.Name, data.GetFloat(param.Name));
                    else if (param.ModType == EnumModType.Add)
                        parentGraph.parameters.UpdateValue(param.Name, parentGraph.parameters.GetFloat(param.Name).Value + data.GetFloat(param.Name));
                    else
                        parentGraph.parameters.UpdateValue(param.Name, parentGraph.parameters.GetFloat(param.Name).Value - data.GetFloat(param.Name));
                }

                else if (param.VarType == ParamVariableType.Bool)
                    parentGraph.parameters.UpdateValue(param.Name, data.GetBool(param.Name));

                else
                {
                    if (param.ModType == EnumModType.Swap)
                        parentGraph.parameters.UpdateValue(param.Name, data.GetString(param.Name));
                    else if (param.ModType == EnumModType.Add)
                        parentGraph.parameters.UpdateValue(param.Name, parentGraph.parameters.GetString(param.Name).Value + data.GetString(param.Name));
                    else
                        parentGraph.parameters.UpdateValue(param.Name, parentGraph.parameters.GetString(param.Name).Value.Replace(data.GetString(param.Name), ""));
                    
                }
            }

            endedInstruction = true;
            SaveHelper.SaveChanges(parentGraph.parameters);
        }
    }
}