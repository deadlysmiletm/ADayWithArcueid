using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using ParadoxEngine;
using ParadoxEngine.Utilities;

[CustomEditor(typeof(GraphPlayerBehaviour))]
public class GraphPlayerBehaviourEditor : Editor
{
    private GraphPlayerBehaviour _behaviour;
    private SceneView _scene;
    private GUIStyle _title;

    private void OnEnable()
    {
        _behaviour = (GraphPlayerBehaviour)target;
        _scene = EditorWindow.GetWindow<SceneView>();

        
    }

    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
            EditorGUILayout.HelpBox("You can't edit this in Play mode.", MessageType.Warning);

        else
            Inspector();
    }

    private void Inspector()
    {
        _title = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 14,
            fontStyle = FontStyle.Bold,
            wordWrap = true
        };

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("GraphPlayer Behaviour", _title);

        EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);

        GUILayout.Space(5);

        GUILayout.Label("GraphPlayer quick access", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });

        if (GUILayout.Button("Open FlowChart"))
        {
            EngineGraphWindow.InitEditorWindow();
            EngineGraphEditorUtilities.LoadSession(_behaviour.graph);
        }

        GUILayout.Space(2);

        if (GUILayout.Button("Open Database"))
            VNDatabaseWindow.InitParadoxDatabaseWindow();

        GUILayout.Space(2);

        if (GUILayout.Button("Help"))
            EngineInstructionWindow.InitInstructionWindow();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();

        string graphPath = "";

        if (_behaviour.graph != null)
        {
            EditorGUILayout.LabelField("Node Graph:");
            EditorGUILayout.LabelField(_behaviour.graph.name, new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
        }

        GUILayout.Space(2);

        if (GUILayout.Button("Load Graph"))
            graphPath = EditorUtility.OpenFilePanel("Load Graph", Application.dataPath + "/Paradox Engine/Graph/Resources/Data/", "");

        if (graphPath != "")
        {
            int appPathLen = Application.dataPath.Length;
            string finalPath = graphPath.Substring(appPathLen - 6);

            _behaviour.graph = (EngineGraph)AssetDatabase.LoadAssetAtPath(finalPath, typeof(EngineGraph));
        }

        GUILayout.Space(10);

        _behaviour.cache = (ParadoxSessionCache)EditorGUILayout.ObjectField("Session cache: ", _behaviour.cache, typeof(ParadoxSessionCache), false);
        _behaviour.settings = (DSetting)EditorGUILayout.ObjectField("Setting: ", _behaviour.settings, typeof(DSetting), false);
        _behaviour.localizationManager = (ParadoxEngine.Localization.LocalizationManager)EditorGUILayout.ObjectField("Localization manager: ", _behaviour.localizationManager, typeof(ParadoxEngine.Localization.LocalizationManager), true);
        _behaviour.characterContainer = (Transform)EditorGUILayout.ObjectField("Character container: ", _behaviour.characterContainer, typeof(Transform), true);

        GUILayout.Space(15);
        EditorGUILayout.LabelField("Background Containers:", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });
        GUILayout.Space(5);

        _behaviour.backgrounds[0] = (UnityEngine.UI.Image)EditorGUILayout.ObjectField("Background 1: ", _behaviour.backgrounds[0], typeof(UnityEngine.UI.Image), true);
        _behaviour.backgrounds[1] = (UnityEngine.UI.Image)EditorGUILayout.ObjectField("Background 2: ", _behaviour.backgrounds[1], typeof(UnityEngine.UI.Image), true);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Text Containers:", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });
        GUILayout.Space(5);

        _behaviour.textContainer = (UnityEngine.UI.Text)EditorGUILayout.ObjectField("Text container: ", _behaviour.textContainer, typeof(UnityEngine.UI.Text), true);
        _behaviour.characterNameContainer = (UnityEngine.UI.Text)EditorGUILayout.ObjectField("Character name container: ", _behaviour.characterNameContainer, typeof(UnityEngine.UI.Text), true);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Audio Containers:", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });
        GUILayout.Space(5);

        _behaviour.musicChannel[0] = (AudioSource)EditorGUILayout.ObjectField("Music channel 1: ", _behaviour.musicChannel[0], typeof(AudioSource), true);
        _behaviour.musicChannel[1] = (AudioSource)EditorGUILayout.ObjectField("Music channel 2: ", _behaviour.musicChannel[1], typeof(AudioSource), true);
        _behaviour.soundChannel = (AudioSource)EditorGUILayout.ObjectField("Sound channel: ", _behaviour.soundChannel, typeof(AudioSource), true);
        _behaviour.voiceChannel = (AudioSource)EditorGUILayout.ObjectField("Voice channel: ", _behaviour.voiceChannel, typeof(AudioSource), true);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(_behaviour);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        Repaint();
    }

    private void OnSceneGUI()
    {
        //Handles.BeginGUI();
        //Panel();
        //Handles.EndGUI();
    }

    public void Panel()
    {
        var pos = _scene.position;

        GUILayout.BeginArea(new Rect(pos.width - 260, pos.height - 120, 250, 175));
        var rec = EditorGUILayout.BeginVertical();
        GUI.color = new Color32(200, 200, 200, 230);
        GUI.Box(rec, GUIContent.none);

        GUILayout.Space(5);

        GUILayout.Label("GraphPlayer quick access", _title);

        if (GUILayout.Button("Open FlowChart"))
        {
            EngineGraphWindow.InitEditorWindow();
            EngineGraphEditorUtilities.LoadSession(_behaviour.graph);
        }

        GUILayout.Space(2);

        if (GUILayout.Button("Open Database"))
            VNDatabaseWindow.InitParadoxDatabaseWindow();

        GUILayout.Space(2);

        if (GUILayout.Button("Help"))
            EngineInstructionWindow.InitInstructionWindow();

        GUILayout.Space(5);        
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
