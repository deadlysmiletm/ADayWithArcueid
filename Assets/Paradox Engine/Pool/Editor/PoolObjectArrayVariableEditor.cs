using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using ParadoxFramework.Exceptions;

namespace ParadoxFramework.CustomUI
{
    [CustomEditor(typeof(PoolObjectArrayVariable))]
    public class PoolObjectArrayVariableEditor : Editor
    {
        private PoolObjectArrayVariable _target;
        private Action _RepaintRequest = delegate { };

        private void OnEnable()
        {
            _target = (PoolObjectArrayVariable)target;

            if (_target.poolObjects == null)
                _target.poolObjects = new List<PoolObjectArrayVariable.PoolObjectData>();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            GUILayout.BeginVertical("BOX");

            if (!_target.poolObjects.Any())
                EditorGUILayout.LabelField("No hay ningun PoolObject agregado. Pulsa en \"Add\" para agregar uno nuevo.", new GUIStyle() { fontStyle = FontStyle.Italic, wordWrap = true });

            EditorGUI.BeginChangeCheck();

            for (int i = 0; i < _target.poolObjects.Count; i++)
            {
                var data = _target.poolObjects[i];

                GUILayout.BeginVertical("BOX");

                data.Name = EditorGUILayout.TextField("Name:", data.Name);
                data.Amount = EditorGUILayout.IntField("Amount:", data.Amount);

                if (data.Amount <= 0)
                {
                    data.Amount = 1;
                    this.PrintException("La cantidad de elementos en el Pool debe ser mayor a 0.", LogType.Warning);
                }

                data.Prefab = (GameObject)EditorGUILayout.ObjectField("Prefab:", data.Prefab, typeof(GameObject), false);
                data.IsDinamyc = EditorGUILayout.Toggle("Is Dinamyc:", data.IsDinamyc);

                if (data.Prefab != null && data.Prefab.GetComponent<IPoolObject>() == null)
                {
                    data.Prefab = null;
                    this.PrintException("El Prefab debe tener un componente que herede de IPoolObject.", LogType.Warning);                    
                }

                if (GUILayout.Button("Remove"))
                {
                    int index = i;
                    _RepaintRequest += () => _target.poolObjects.RemoveAt(index);
                }

                 _target.poolObjects[i] = data;

                GUILayout.EndVertical();

                EditorGUILayout.Space();
            }

            GUILayout.EndVertical();

            _RepaintRequest();
            _RepaintRequest = delegate { };

            if (GUILayout.Button("Add"))
                _target.poolObjects.Add(new PoolObjectArrayVariable.PoolObjectData() { Name = "New GameObject", Prefab = null, Amount = 1, IsDinamyc = false });

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_target);
                AssetDatabase.SaveAssets();
            }
        }
    }
}