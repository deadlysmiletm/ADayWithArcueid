using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine;
using ParadoxEngine.Utilities;
using System;

using Object = System.Object;

[CreateAssetMenu(menuName = "Paradox Engine/Scene cache", fileName = "Scene cache")]
public class ParadoxSessionCache : ScriptableObject
{
    private List<TupleSystemObject> _data = new List<TupleSystemObject>();
    private EngineGraph _actualGraph;
    private List<string> _sceneText = new List<string>();
    [NonSerialized] public bool HasSave = false;

    public EngineGraph EngineGraph { get => _actualGraph; set => _actualGraph = value; }
    public List<TupleSystemObject> Data => _data;
    public List<string> LogText => _sceneText;

    public void AddText(string text) => _sceneText.Add(text);
    public void RemoveText(string text) => _sceneText.Remove(text);
    public void RemoveLast() => _sceneText.RemoveAt(_sceneText.Count - 1);
    public void ClearText() => _sceneText.Clear();

    public void AddData(TupleSystemObject data) => _data.Add(data);
    public void RemoveData(TupleSystemObject data) => _data.Remove(data);
    public void ClearData() => _data.Clear();
}
