using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxEngine;

public class VNDatabaseMenu
{
    [MenuItem("Paradox Engine/Database")]
    public static void InitParadoxDatabaseWindow()
    {
        VNDatabaseWindow.InitParadoxDatabaseWindow();
    }
}
