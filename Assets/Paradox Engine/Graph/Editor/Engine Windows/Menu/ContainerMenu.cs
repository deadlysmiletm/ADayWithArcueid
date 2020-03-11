using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ContainerMenu
{
    [MenuItem("Paradox Engine/Scene Container/Create")]
	public static void OpenPopup()
    {
        ContainerPopupWindow.InitContainerPopup();
    }
}
