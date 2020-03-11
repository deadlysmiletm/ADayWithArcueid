using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;
using ParadoxEngine.Utilities.Parameters;
using System.Linq;
using System;

public class EditVariablePopupWindow : EditorWindow
{
    //Variables
    static EditVariablePopupWindow _editPopup;
    private static string _newName;
    private static bool _isPersistent;

    private static int _intValue;
    private static float _floatValue;
    private static bool _boolValue;
    private static string _stringValue;
    private static bool _cantSave;

    public static IntSerializedParamVariable intSerialized;
    public static FloatSerializedParamVariable floatSerialized;
    public static BoolSerializedParamVaraible boolSerialized;
    public static StringSerializedParamVariable stringSerialized;
    public static Params parameters;
    public static IEnumerable<string> ParamNames;
    public static Action<string> ExecuteSave;

    //Métodos principales
    public static void InitContainerPopup(IntSerializedParamVariable value, Action<string> OnSave)
    {
        StartWindow(value.Name);

        ParamNames = parameters.Select(x => x.Name).Where(x => x != value.Name);
        intSerialized = value;
        _intValue = value.Value;
        SetValues(value);
        
        ExecuteSave = OnSave;
    }

    public static void InitContainerPopup(FloatSerializedParamVariable value, Action<string> OnSave)
    {
        StartWindow(value.Name);
        floatSerialized = value;
        _floatValue = value.Value;
        SetValues(value);

        ParamNames = parameters.Select(x => x.Name).Where(x => x != value.Name);
        ExecuteSave = OnSave;
    }

    public static void InitContainerPopup(BoolSerializedParamVaraible value, Action<string> OnSave)
    {
        StartWindow(value.Name);
        boolSerialized = value;
        _boolValue = value.Value;
        SetValues(value);

        ParamNames = parameters.Select(x => x.Name).Where(x => x != value.Name);
        ExecuteSave = OnSave;
    }

    public static void InitContainerPopup(StringSerializedParamVariable value, Action<string> OnSave)
    {
        StartWindow(value.Name);
        stringSerialized = value;
        _stringValue = value.Value;
        SetValues(value);

        ParamNames = parameters.Select(x => x.Name).Where(x => x != value.Name);
        ExecuteSave = OnSave;
    }

    private static void SetValues<T>(SerializedParamVariable<T> value)
    {
        _newName = value.Name;
        _isPersistent = parameters[_newName].IsPersistent;
    }

    private static void StartWindow(string name)
    {
        _editPopup = (EditVariablePopupWindow)EditorWindow.GetWindow<EditVariablePopupWindow>();
        _editPopup.titleContent = new GUIContent($"Edit {name}");
        parameters = EngineGraphUtilities.LoadParameterReference();
        _editPopup.minSize = new Vector2(427, 191);
    }


    private void OnGUI()
    {
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Edit parameter:", EditorStyles.boldLabel);

        _newName = EditorGUILayout.TextField("Name:", _newName);

        if (ParamNames.Any())
            _cantSave = ParamNames.Any(x => x == _newName);

        if (_cantSave)
            EditorGUILayout.HelpBox("You already have one parameter with this name. Use another name or you can't save.", MessageType.Error);


        GUILayout.Space(5);

        _isPersistent = EditorGUILayout.Toggle("Is Persistent:", _isPersistent);

        GUILayout.Space(5);

        if (intSerialized != null)
            _intValue = EditorGUILayout.IntField("Value:", _intValue);

        else if (floatSerialized != null)
            _floatValue = EditorGUILayout.FloatField("Value:", _floatValue);

        else if (boolSerialized != null)
            _boolValue = EditorGUILayout.Toggle("Value:", _boolValue);

        else
            _stringValue = EditorGUILayout.TextField("Value:", _stringValue);


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", GUILayout.Height(40)))
        {
            if (_cantSave)
                return;

            if (intSerialized != null)
            {
                parameters.UpdateName(intSerialized.Name, _newName);
                parameters.UpdateValue(_newName, _intValue);
            }

            else if (floatSerialized != null)
            {
                parameters.UpdateName(floatSerialized.Name, _newName);
                parameters.UpdateValue(_newName, _floatValue);
            }

            else if (boolSerialized != null)
            {
                parameters.UpdateName(boolSerialized.Name, _newName);
                parameters.UpdateValue(_newName, _boolValue);
            }

            else
            {
                parameters.UpdateName(stringSerialized.Name, _newName);
                parameters.UpdateValue(_newName, _stringValue);
            }

            parameters.SetAsPersistance(_newName, _isPersistent);

            EditorUtility.SetDirty(parameters);
            ExecuteSave(_newName);
            ResetValues();
            Close();
        }

        if (GUILayout.Button("Cancel", GUILayout.Height(40)))
        {
            ResetValues();
            Close();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void ResetValues()
    {
        ParamNames = null;
        intSerialized = null;
        floatSerialized = null;
        boolSerialized = null;
        stringSerialized = null;

        ExecuteSave = null;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
