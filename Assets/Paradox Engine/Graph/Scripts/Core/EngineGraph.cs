using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;
using System.Linq;
using ParadoxEngine.Utilities;
using ParadoxEngine.Utilities.Parameters;

namespace ParadoxEngine
{
    [System.Serializable]
    public class EngineGraph : ScriptableObject
    {
#if UNITY_EDITOR
        private Editor edTemp;
#endif
        //Variables Public
        public string graphName;
        public List<NTemplate> nodes;
        public NTemplate selectedNode;
        public bool wantsConnection = false;
        public NTemplate connectionNode;
        public Params parameters;
        public Delegate delegado;

        public bool panningScreen;

        public Vector2 offset;
        public float zoom = 1;
        public Vector2 zoomPanAdjust;

#if UNITY_EDITOR
        //Métodos principales
        private void OnEnable()
        {
            if (nodes == null)
                nodes = new List<NTemplate>();

            InitializeParam();
        }

        public void Initialize()
        {
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                    nodes[i].InitNode();
            }
            
            InitializeParam();
        }
#endif


#if UNITY_EDITOR
        public void InitializeParam() => parameters = EngineGraphUtilities.LoadParameterReference();

        public void UpdateGraphGUI(Event e, Rect viewRect, GUISkin viewSkin)
        {
            EditorGUI.BeginChangeCheck();

            if (nodes.Count > 0)
            {
                ProcessEvents(e, viewRect);
                for (int i = 0; i < nodes.Count; i++)
                    nodes[i].UpdateNodeGUI(e, viewRect, viewSkin);
            }

            //if (edTemp != null)
            //{
            //    edTemp.OnInspectorGUI();
            //}
            //else
            //    Debug.Log("Nada");

            if (wantsConnection)
            {
                if (connectionNode != null)
                    DrawConnectionToMouse(e.mousePosition);
            }

            if (selectedNode && Selection.activeObject != selectedNode)
            {

            }

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(this);
        }

        //Métodos secundarios
        void ProcessEvents(Event e, Rect viewRect)
        {
            if (viewRect.Contains(e.mousePosition))
            {
                if (e.button == 2)
                {
                    if (e.type == EventType.MouseDown)
                    {
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            nodes[i].isPanning = true;
                        }

                        panningScreen = true;
                    }

                    if (e.type == EventType.MouseDrag && panningScreen)
                        offset += e.delta / zoom;

                    else if (e.type == EventType.MouseUp)
                    {
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            nodes[i].isPanning = false;
                        }

                        panningScreen = false;
                    }
                }

                else if (e.button == 0)
                {
                    if (e.type == EventType.MouseDown)
                    {
                        DeselectAllNodes();
                        bool setNode = false;
                        selectedNode = null;
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            if (nodes[i].myRect.Contains(e.mousePosition))
                            {
                                if (selectedNode == null)
                                {
                                    var temp = nodes[i];
                                    nodes.RemoveAt(i);
                                    temp.isSelected = true;
                                    selectedNode = temp;
                                    nodes.Add(temp);
                                    setNode = true;
                                    //edTemp = Editor.CreateEditor(selectedNode);
                                    
                                    Selection.activeObject = selectedNode;
                                    //AssetDatabase.Refresh();
                                }
                            }
                        }

                        if (!setNode)
                            DeselectAllNodes();

                        if (wantsConnection)
                            wantsConnection = false;
                    }
                }

                else
                {
                    if (e.type == EventType.MouseDown && !nodes.Any(x => x.myRect.Contains(e.mousePosition)))
                        DeselectAllNodes();
                }
            }
        }
#endif

        void DeselectAllNodes()
        {
            for (int i = 0; i < nodes.Count; i++)
                nodes[i].isSelected = false;
        }

#if UNITY_EDITOR
        void DrawConnectionToMouse(Vector2 mousePosition)
        {
            Vector3 startPos = new Vector3(connectionNode.myRect.x + connectionNode.myRect.width + 24f, connectionNode.myRect.y + (connectionNode.myRect.height * 0.5f), 0f);
            Vector3 endPos = new Vector3(mousePosition.x, mousePosition.y, 0f);
            Vector3 startTan = startPos + Vector3.right * (-50 + 100 * 2) + Vector3.up * (-50 + 100 * 0.5f);
            Vector3 endTan = endPos + Vector3.right * (-50 + 100 * -1) + Vector3.up * (-50 + 100 * 0.5f);
            Color shadowCol = new Color(0, 0, 0, 0.6f);

            for (int j = 0; j < 2; j++)
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (j + 1) * 5);

            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.red, null, 2);
        }
#endif

#region Parameter scripting
        public void SetInt(string paramName, int value) { parameters.UpdateValue(paramName, value); }
        public void SetFloat(string paramName, float value) { parameters.UpdateValue(paramName, value); }
        public void SetBool(string paramName, bool value) { parameters.UpdateValue(paramName, value); }
        public void SetString(string paramName, string value) { parameters.UpdateValue(paramName, value); }

        public int GetInt(string paramName) => parameters.GetInt(paramName).Value;
        public float GetFloat(string paramName) => parameters.GetFloat(paramName).Value;
        public bool GetBool(string paramName) => parameters.GetBool(paramName).Value;
        public string GetString(string paramName) => parameters.GetString(paramName).Value;
        #endregion
    }
}