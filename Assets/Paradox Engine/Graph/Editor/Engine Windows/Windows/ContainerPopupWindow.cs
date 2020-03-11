using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;

public class ContainerPopupWindow : EditorWindow
{
    //Variables
    static ContainerPopupWindow containerPopup;
    public string containerName = "Enter a name...";
    private GameObject _canvas;
    private GameObject _prefab;

    //Métodos principales
    public static void InitContainerPopup()
    {
        containerPopup = (ContainerPopupWindow)EditorWindow.GetWindow<ContainerPopupWindow>();
        containerPopup.titleContent = new GUIContent("Conteiner Popup");
        containerPopup._prefab = (GameObject)Resources.Load<GameObject>("Prefab/ParadoxPlayer");

        containerPopup.minSize = new Vector2(427, 191);
        containerPopup.maxSize = new Vector2(427, 191);
    }

    private void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("Create a new Graph Player:", EditorStyles.boldLabel);
        containerName = EditorGUILayout.TextField("Enter name: ", containerName);

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();
        _prefab = (GameObject)EditorGUILayout.ObjectField("Container Prefab", _prefab, typeof(GameObject), true);
        _canvas = (GameObject)EditorGUILayout.ObjectField("Canvas:", _canvas, typeof(GameObject), true);

        if (_canvas != null && !_canvas.GetComponent<Canvas>())
            _canvas = null;

        if (EditorGUI.EndChangeCheck())
            Repaint();

        if (_canvas == null)
            EditorGUILayout.HelpBox("The canvas can't be null.", MessageType.Error);

        if (_prefab == null)
            EditorGUILayout.HelpBox("The Container prefab can't be null.", MessageType.Error);

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();

        if (_prefab != null && _canvas != null)
        {
            if (GUILayout.Button("Create Paradox Player", GUILayout.Height(40)))
            {
                if (!string.IsNullOrEmpty(containerName) && containerName != "Enter a name..." && containerName != " ")
                {
                    EngineGraphEditorUtilities.CreateContainer(_prefab, _canvas, containerName);
                    containerPopup.Close();
                }

                else
                    EditorUtility.DisplayDialog("Container Message", "Please, insert a valid name.", "OK");
            }
        }

        if (GUILayout.Button("Cancel", GUILayout.Height(40)))
            containerPopup.Close();

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.Space(20);
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
    }
}
