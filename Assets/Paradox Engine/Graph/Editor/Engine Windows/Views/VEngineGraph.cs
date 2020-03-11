using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using ParadoxEngine.Utilities;

namespace ParadoxEngine
{
    public class VEngineGraph : VTemplate
    {
        protected Vector2 mousePos;
        int overNodeID = 0;
        Vector2 _zoomPos { get { return viewRect.size / 2; } }
        static Texture2D background;
        private static List<Matrix4x4> _GUIMatrices;
        private Vector2 _fixedMousePose;
        private Action OnShowMenu = delegate { };

        public VEngineGraph() : base("Paradox Engine: Flow chart")
        {
            _GUIMatrices = new List<Matrix4x4>();
        }

        public override void Execute(Rect editorRect, Rect precentageRect, Event e, EngineGraph currentGraph)
        {
            base.Execute(editorRect, precentageRect, e, currentGraph);

            GUI.Box(viewRect, viewTitle, _viewSkin.GetStyle("ViewBG"));

            if (Event.current.type == EventType.Repaint)
                GridGUI();

            if (currentGraph != null)
            {
                Rect canvasRect = viewRect;
                currentGraph.zoomPanAdjust = BeginScale(ref viewRect, _zoomPos, currentGraph.zoom);
            }

            GUILayout.BeginArea(viewRect);


            if (currentGraph != null)
                currentGraph.UpdateGraphGUI(e, viewRect, _viewSkin);

            GUILayout.EndArea();

            ProcessEvents(e);

            if (currentGraph != null)
                EndScale();

            _fixedMousePose = Event.current.mousePosition;
            OnShowMenu();
            OnShowMenu = delegate { };
        }

        public void GridGUI()
        {
            if (background == null)
                background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Graph/Resources/Textures/Editor/background.png", typeof(Texture2D));

            float zoom = currentGraph != null ? currentGraph.zoom : 1;

            Vector2 offset = currentGraph != null ? currentGraph.offset : Vector2.zero;

            Vector2 tileOffset = new Vector2(-(_zoomPos.x * zoom + offset.x) / background.width,
                    ((_zoomPos.y - viewRect.height) * zoom + offset.y) / background.height);

            Vector2 tileAmount = new Vector2(Mathf.Round(viewRect.width * zoom) / background.width,
                    Mathf.Round(viewRect.height * zoom) / background.height);

            GUI.DrawTextureWithTexCoords(viewRect, background, new Rect(tileOffset, tileAmount));
        }

        public override void ProcessEvents(Event e)
        {
            base.ProcessEvents(e);

            if (!viewRect.Contains(e.mousePosition))
                return;

            if (e.button == 1)
            {
                if (e.type == EventType.MouseUp)
                {
                    mousePos = e.mousePosition;

                    bool overNode = false;
                    overNodeID = 0;

                    if (currentGraph != null && currentGraph.nodes.Count > 0)
                    {
                        for (int i = 0; i < currentGraph.nodes.Count; i++)
                        {
                            if (currentGraph.nodes[i].myRect.Contains(mousePos))
                            {
                                overNode = true;
                                overNodeID = i;
                            }
                        }
                    }

                    if (!overNode)
                        ProcessContextMenu(e, 0);
                    else
                    {
                        if (currentGraph.nodes[overNodeID].nodeType == EnumNodeType.Start)
                            ProcessContextMenu(e, 2);

                        else if (currentGraph.nodes[overNodeID].nodeType == EnumNodeType.End)
                            ProcessContextMenu(e, 3);

                        else if (currentGraph.nodes[overNodeID].nodeType == EnumNodeType.Change_Flow_Chart)
                            ProcessContextMenu(e, 4);

                        else
                            ProcessContextMenu(e, 1);
                    }
                }

            }

            if (e.type == EventType.ScrollWheel && currentGraph != null)
                currentGraph.zoom = (float)Math.Round(Math.Min(4.0f, Math.Max(0.6f, currentGraph.zoom + e.delta.y / 15)), 2);

        }

        void ProcessContextMenu(Event e, int contextID)
        {
            GenericMenu menu = new GenericMenu();

            if (contextID == 0)
            {
                menu.AddItem(new GUIContent("Create Graph"), false, EngineGraphEditorUtilities.OpenNewGraphPanel);
                menu.AddItem(new GUIContent("Load Graph"), false, EngineGraphEditorUtilities.LoadGraph);

                if (currentGraph != null)
                {
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Unload Graph"), false, EngineGraphEditorUtilities.UnloadGraph);
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Story State/Add Text State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Text, mousePos));
                    menu.AddItem(new GUIContent("Story State/Add Clear Text State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Clear, mousePos));
                    menu.AddItem(new GUIContent("Story State/Add Change Flow Chart State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Change_Flow_Chart, mousePos));
                    menu.AddItem(new GUIContent("Character State/Add Show Character State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Show_Character, mousePos));
                    menu.AddItem(new GUIContent("Character State/Add Hide Character State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Hide_Character, mousePos));
                    menu.AddItem(new GUIContent("Character State/Add Change Character Sprite State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Change_Character_Sprite, mousePos));
                    menu.AddItem(new GUIContent("Character State/Add Move Character State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Move_Character, mousePos));
                    menu.AddItem(new GUIContent("Image State/Add Change Background State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Change_Background, mousePos));
                    menu.AddItem(new GUIContent("Effect State/Add Delay State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Delay, mousePos));
                    menu.AddItem(new GUIContent("Effect State/Add Show Text Container"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Show_Text_Container, mousePos));
                    menu.AddItem(new GUIContent("Effect State/Add Hide Text Container"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Hide_Text_Container, mousePos));
                    menu.AddItem(new GUIContent("Sound State/Music/Add Play Music State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Play_Music, mousePos));
                    menu.AddItem(new GUIContent("Sound State/Music/Add Stop Music State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Stop_Music, mousePos));
                    menu.AddItem(new GUIContent("Sound State/Sound FX/Add Play Sound State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Play_Sound, mousePos));
                    menu.AddItem(new GUIContent("Sound State/Sound FX/Add Stop Sound State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Stop_Sound, mousePos));
                    menu.AddItem(new GUIContent("Sound State/Voice/Add Play Voice State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Play_Voice, mousePos));
                    menu.AddItem(new GUIContent("Sound State/Voice/Add Stop Voice State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Stop_Voice, mousePos));
                    menu.AddItem(new GUIContent("Branch State/Add Question Branch State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Branch_Question, mousePos));
                    menu.AddItem(new GUIContent("Branch State/Add Conditional Branch State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Branch_Condition, mousePos));
                    menu.AddSeparator("Branch State/");
                    menu.AddItem(new GUIContent("Branch State/Add Answer State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Answer, mousePos));
                    menu.AddItem(new GUIContent("Branch State/Add Condition State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Condition, mousePos));
                    menu.AddItem(new GUIContent("System State/Add Set Parameter State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Set_Param, mousePos));
                    menu.AddItem(new GUIContent("System State/Add Trigger Event State"), false, () => EngineGraphEditorUtilities.CreateNode(currentGraph, EnumNodeType.Trigger_Event, mousePos));
                    
                }
            }

            if (contextID == 1)
            {
                if (currentGraph != null)
                {
                    menu.AddItem(new GUIContent("Delete State"), false, () => EngineGraphEditorUtilities.DeleteNode(currentGraph, overNodeID));
                    menu.AddItem(new GUIContent("Disconnect/Disconnect Input"), false, () => EngineGraphEditorUtilities.DisconnectInput(currentGraph, overNodeID));
                    menu.AddItem(new GUIContent("Disconnect/Disconnect Output"), false, () => EngineGraphEditorUtilities.DisconnectOutput(currentGraph, overNodeID));
                    menu.AddItem(new GUIContent("Disconnect/Disconnect All"), false, () => EngineGraphEditorUtilities.DisconnectAll(currentGraph, overNodeID));
                }
            }
            if (contextID == 2)
            {
                if (currentGraph != null)
                    menu.AddItem(new GUIContent("Disconnect Output"), false, () => EngineGraphEditorUtilities.DisconnectOutput(currentGraph, overNodeID));
            }
            if (contextID == 3)
            {
                if (currentGraph != null)
                    menu.AddItem(new GUIContent("Disconnect Input"), false, () => EngineGraphEditorUtilities.DisconnectInput(currentGraph, overNodeID));
            }
            if (contextID == 4)
            {
                if (currentGraph != null)
                {
                    menu.AddItem(new GUIContent("Delete State"), false, () => EngineGraphEditorUtilities.DeleteNode(currentGraph, overNodeID));
                    menu.AddItem(new GUIContent("Disconnect Input"), false, () => EngineGraphEditorUtilities.DisconnectInput(currentGraph, overNodeID));
                }
            }

            OnShowMenu += () =>
            {
                menu.Show(_fixedMousePose);
                e.Use();
            };
        }

        public static Vector2 BeginScale(ref Rect rect, Vector2 zoomPivot, float zoom)
        {
            Rect viewRect;

            GUI.EndGroup();
            viewRect = rect;

            viewRect.y += 23;

            rect = Scale(viewRect, viewRect.position + zoomPivot, new Vector2(zoom, zoom));

            GUI.BeginGroup(rect);
            rect.position = Vector2.zero;

            Vector2 zoomPosAdjust = rect.center - viewRect.size / 2 + zoomPivot;

            _GUIMatrices.Add(GUI.matrix);
            GUIUtility.ScaleAroundPivot(new Vector2(1 / zoom, 1 / zoom), zoomPosAdjust);

            return zoomPosAdjust;
        }

        public void EndScale()
        {
            if (_GUIMatrices.Count == 0)
                throw new UnityException("You have more scale regions that you are begining");

            GUI.matrix = _GUIMatrices.Last();
            _GUIMatrices.RemoveAt(_GUIMatrices.Count - 1);

            GUI.EndGroup();

            GUI.BeginClip(new Rect(0, 23, Screen.width, Screen.height - 23));
        }

        private static Rect Scale(Rect rect, Vector2 pivot, Vector2 scale)
        {
            rect.position = Vector2.Scale(rect.position - pivot, scale) + pivot;
            rect.size = Vector2.Scale(rect.size, scale);
            return rect;
        }
    }
}