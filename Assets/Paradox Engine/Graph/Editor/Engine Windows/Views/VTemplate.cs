using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ParadoxEngine
{
    [System.Serializable]
    public class VTemplate : Editor
    {
        public string viewTitle;
        public Rect viewRect;

        protected static GUISkin _viewSkin;
        protected EngineGraph currentGraph;

        public VTemplate(string title)
        {
            viewTitle = title;
        }

        protected virtual void OnEnable()
        {
            GetEditorSkin();
        }

        //Update
        public virtual void Execute(Rect editorRect, Rect precentageRect, Event e, EngineGraph currentGraph)
        {
            if (_viewSkin == null)
            {
                GetEditorSkin();
                return;
            }

            this.currentGraph = currentGraph;

            if (currentGraph != null)
                viewTitle = currentGraph.graphName;

            viewRect = new Rect(editorRect.x * precentageRect.x, editorRect.y * precentageRect.y, editorRect.width * precentageRect.width, editorRect.height * precentageRect.height);
        }

        public virtual void ProcessEvents(Event e) { }

        protected virtual void GetEditorSkin() => _viewSkin = (GUISkin)Resources.Load("GUISkins/Editor/GraphEditorSkin");
    }
}
