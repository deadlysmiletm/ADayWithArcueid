using UnityEngine;
using UnityEditor;
using ParadoxEngine;
using ParadoxEngine.Utilities;
using System.Linq;

[CustomEditor(typeof(NChangeFlowChart))]
public class NChangeFlowChartEditor : Editor
{
    private GUIStyle _title;
    private NChangeFlowChart _node;

    private void OnEnable()
    {
        _node = (NChangeFlowChart)target;
    }

    public override void OnInspectorGUI()
    {
        _title = new GUIStyle(GUI.skin.label)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            stretchWidth = true
        };

        GUILayout.Space(10);

        GUILayout.Label("PARADOX ENGINE" + "\n" + "CHANGE FLOW CHART STATE", _title);

        GUILayout.Space(10);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
        GUILayout.Space(20);

        EditorGUILayout.LabelField(new GUIContent("Flow chart:", "Flow chart that will be changed."));
        GUILayout.BeginVertical("BOX");

        var temp = Resources.LoadAll("Data", typeof(EngineGraph)).Select(x => (EngineGraph)x);

        if (temp.Any())
        {
            var array = temp.Select(x => x.graphName).ToArray();

            int arrayIndex = !_node.newGraph ? 0 : temp.IndexOf(x => x == _node.newGraph);

            arrayIndex = EditorGUILayout.Popup(arrayIndex, array);

            if (!_node.newGraph || array[arrayIndex] != _node.newGraph.name)
            {
                _node.newGraph = temp.ToArray()[arrayIndex];
                EngineGraphUtilities.SetDirty(_node);
            }
        }

        GUILayout.EndVertical();
    }
}
