﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParadoxLogButton : MonoBehaviour
{
    public Text logComponent;

    public void ShowLog()
    {
        string log = "\n\n";
        var logPack = DialogueDatabase.activeGraphPlayer.cache.LogText;

        for (int i = 0; i < logPack.Count; i++, log += "\n\n")
            log += logPack[i];

        log += "\n\n\n";

        logComponent.text = log;
    }
}
