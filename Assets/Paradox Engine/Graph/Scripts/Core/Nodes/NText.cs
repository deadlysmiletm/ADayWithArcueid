using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using ParadoxEngine.Utilities;
using ParadoxEngine.Utilities.Parameters;
using System;

[System.Serializable]
public class NText : NTemplate
{
    public List<ParadoxTextNodeData> data;
    [NonSerialized] private bool _inAction = false;
    [NonSerialized] public KeyCode key;

    public override void InitNode()
    {
        base.InitNode();
        nodeType = EnumNodeType.Text;
        myRect = new Rect(10f, 10f, 110f, 55f);
        data = new List<ParadoxTextNodeData>();
    }

#if UNITY_EDITOR
    public override void UpdateNode(Event e, Rect viewRect) => base.UpdateNode(e, viewRect);

    public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin) => base.UpdateNodeGUI(e, viewRect, viewSkin);
#endif


    public override void Execute()
    {
        if (endedInstruction)
            DialogueDatabase.activeGraphPlayer.ChangeNode(output.outputNode);

        if (_inAction)
            return;

        DialogueDatabase.activeGraphPlayer.StartCoroutine(ExecuteTextList());
        _inAction = true;
    }

    public override void EndState()
    {
        base.EndState();
        _inAction = false;
    }

    private IEnumerator ExecuteTextList()
    {
        var pauseWait = new WaitUntil(() => DialogueDatabase.activeGraphPlayer.IsPlaying);

        for (int i = 0; i < data.Count; i++)
        {

            if (data[i].IsDialogue)
            {
                DialogueDatabase.activeGraphPlayer.ShowDialogueBox(true);
                DialogueDatabase.activeGraphPlayer.ChangeNameDialogueText(BakeText(data[i].Character.name));
            }

            else
                DialogueDatabase.activeGraphPlayer.ShowDialogueBox(false);


            yield return WriteText(data[i]);
            yield return pauseWait;


            if (data[i].UseDelay)
            {
                var delay = data[i].DelayTime;

                yield return new WaitUntil(() =>
                {
                    delay -= Time.deltaTime;
                    return DialogueDatabase.activeGraphPlayer.IsPlaying && delay <= 0;
                });
            }

            else if (data[i].UseCustomKey)
                yield return new WaitUntil(() => Input.GetKeyDown(data[i].CustomKey));

            else
                yield return new WaitUntil(() => DialogueDatabase.activeGraphPlayer.settings.InputGetKeyDown());

            yield return new WaitForEndOfFrame();
            yield return pauseWait;
        }

        endedInstruction = true;
    }

    private IEnumerator WriteText(ParadoxTextNodeData dataPack)
    {
        dataPack.Text = BakeText(dataPack.Text);
        string text = "";
        float temp = DialogueDatabase.activeGraphPlayer.settings.TimeForChar;

        if (!dataPack.ClearLastText)
        {
            text = DialogueDatabase.activeGraphPlayer.GetText();

            if (dataPack.ContinueParagraph)
                text += " ";

            else
                text += "\n\n";
        }

        string tempText = text;

        int index = 0;

        var wait = new WaitUntil(() =>
        {
            temp -= Time.deltaTime;
            return DialogueDatabase.activeGraphPlayer.settings.InputGetKeyDown(() =>
            {
                tempText += dataPack.Text;
                text = tempText;
                DialogueDatabase.activeGraphPlayer.ChangeText(text);

                index = dataPack.Text.Length;
            }) || temp <= 0;
        });


        while (index < dataPack.Text.Length)
        {
            text += dataPack.Text[index];
            DialogueDatabase.activeGraphPlayer.ChangeText(text);

            yield return wait;

            temp = DialogueDatabase.activeGraphPlayer.settings.TimeForChar;
            index++;
        }


        if (!dataPack.ClearLastText)
            DialogueDatabase.activeGraphPlayer.cache.RemoveLast();

        if (dataPack.IsDialogue)
            DialogueDatabase.activeGraphPlayer.cache.AddText($"<i><b>{BakeText(dataPack.Character.GetIdentificator())}:</b></i> {text}");

        else
            DialogueDatabase.activeGraphPlayer.cache.AddText(text);

        yield return new WaitForEndOfFrame();
    }

    protected string BakeText(string text)
    {

        if (!text.Contains("{"))
            return text;

        string temp;
        string value;

        foreach (var item in parentGraph.parameters)
        {
            temp = "{" + item.Name + "}";

            if (text.Contains(temp))
            {
                if (item.Type == ParamVariableType.Int)
                    value = parentGraph.GetInt(item.Name).ToString();

                else if (item.Type == ParamVariableType.Float)
                    value = parentGraph.GetFloat(item.Name).ToString();

                else if (item.Type == ParamVariableType.Bool)
                    value = parentGraph.GetBool(item.Name).ToString();

                else
                    value = parentGraph.GetString(item.Name).ToString();


                text = text.Replace(temp, value);
            }
        }
        
        return text;
    }


#if UNITY_EDITOR
    protected override void NodeStyle(GUISkin viewSkin)
    {
        if (!isSelected)
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("Story"));
        else
            GUI.Box(myRect, nodeName, viewSkin.GetStyle("StorySelected"));
    }
#endif
}