using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EngineGraphMenu
{
	[MenuItem("Paradox Engine/Flow Chart/Launch Graph")]
    public static void InitNodeEditor()
    {
        EngineGraphWindow.InitEditorWindow();
    }
}
