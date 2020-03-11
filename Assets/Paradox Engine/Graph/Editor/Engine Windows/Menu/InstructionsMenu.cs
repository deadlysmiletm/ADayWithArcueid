using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InstructionsMenu
{
    [MenuItem("Paradox Engine/Help")]
	public static void InitInstructions()
    {
        EngineInstructionWindow.InitInstructionWindow();
    }
}
