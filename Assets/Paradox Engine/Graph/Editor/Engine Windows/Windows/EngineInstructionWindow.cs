using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EngineInstructionWindow : EditorWindow
{
    static EngineInstructionWindow instruction;
    private Vector2 pos;

    private GUIStyle _title;
    private GUIStyle _subtitle;
    private GUIStyle _points;
    private GUIStyle _text;

    public float _width = 580;
    public float _height = 700;

    public static void InitInstructionWindow()
    {
        instruction = (EngineInstructionWindow)EditorWindow.GetWindow<EngineInstructionWindow>();
        instruction.titleContent = new GUIContent("Help");

        instruction._title = new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            wordWrap = true
        };

        instruction._subtitle = new GUIStyle()
        {
            fontSize = 14,
            fontStyle = FontStyle.Bold,
            wordWrap = true
        };

        instruction._points = new GUIStyle()
        {
            fontStyle = FontStyle.Bold,
            wordWrap = true
        };

        instruction._text = new GUIStyle() { wordWrap = true };


    }

    private void OnGUI()
    {
        GUI.skin.window.margin = new RectOffset(20, 20, 10, 10);

        EditorGUILayout.BeginVertical();
        pos = EditorGUILayout.BeginScrollView(pos);

        maxSize = new Vector2(_width, _height);
        minSize = maxSize / 2;

        GUILayout.Space(20);

        GUI.DrawTexture(GUILayoutUtility.GetRect(150, 150), (Texture)Resources.Load("Textures/Editor/logo"), ScaleMode.ScaleToFit);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Ayuda", _title);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Introducción:", _subtitle);
        GUILayout.Space(5);
        GUILayout.Label("«Paradox Engine» es una herramienta para Unity con la cual se pueden crear, principalmente, juegos del genero Novela visual. Sin embargo, debido a la versatilidad de «Paradox Engine» puede usarse como sistema de diálogos e incluso manejar eventos serializados.", _text);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Database", _subtitle);
        GUILayout.Space(5);
        GUILayout.Label("En la base de datos se pueden definir los personajes, escenarios y sonidos a utilizar, así como también modificar algunas configuraciones.", _text);
        GUILayout.Label("Abrir ventana: Paradox Engine -> Database.", _text);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("FlowChart", _subtitle);
        GUILayout.Space(5);
        GUILayout.Label("El mapa de flujo es creado y editado mediante un editor de nodos. Cada nodo cuenta con sus propios atributos editables intuitivamente desde el inspector.", _text);
        GUILayout.Label("Abrir ventana: Paradox Engine -> FlowChart -> Launch Graph.", _text);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("GraphPlayer", _subtitle);
        GUILayout.Space(5);
        GUILayout.Label("El GraphPlayer es un componente el cual se encarga de ejecutar los FlowCharts en la escena. Es recomendable utilizar el prefab completamente configurado para cargar el mapa de flujo y utilizar.", _text);
        GUILayout.Label("Abrir ventana: Paradox Engine -> Scene Container -> Create", _text);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Ventana de FlowChart", _subtitle);
        GUILayout.Space(5);
        GUILayout.Label("File", _points);
        GUILayout.Label(" Permite crear y borrar FlowCart, así como cargar los datos de uno o quitar el que esté cargado.", _text);

        GUILayout.Space(5);

        GUILayout.Label("Insert", _points);
        GUILayout.Label(" Permite crear todos los tipos de nodos disponibles dentro del FlowChart.", _text);

        GUILayout.Space(5);

        GUILayout.Label("Tools", _points);
        GUILayout.Label(" Cuando abrimos la ventana del FlowChart siempre cargará el último grapho cargado al cerrar, aquí se encuentra la opción que permite borrar el mencionado cache.", _text);

        GUILayout.Space(5);

        GUILayout.Label("Add Parameter", _points);
        GUILayout.Label(" En la subventana de parámetros se pueden agregar variables globales para el juego.", _text);

        GUILayout.Space(5);

        GUILayout.Label("Controles", _points);
        GUILayout.Label(" Clic izquierdo: Seleccionar nodos. Si se mantiene se pueden arrastrar.", _text);
        GUILayout.Label(" Clic derecho: Abre menú contextual. Si se preciona sobre un nodo te permite desconectar sus conecciones o borrar el nodo. Si se preciona sobre cualquier parte sobre la grilla ofrece las mismas opciones de «File» y «Insert» juntos, en el caso de que se tenga para conectar el nodo y creas otro mediante este menú contextual se conectarán automáticamente.", _text);
        GUILayout.Label(" Clic central: Si se mantiene puede realizarse paneos en el FlowChart.", _text);
        GUILayout.Label(" Rueda del mouse: Permite realizar zoom in y zoom out en el FlowChart.", _text);
        GUILayout.Label(" Flechas de desplazamiento laterales: Permiten agrandar y achicar la subventana de parámetros.", _text);

        GUILayout.Space(20);

        GUILayout.Label("Nodos", _subtitle);
        GUILayout.Label("Cada nodo en particular, a pesar de sus nombres descriptivos, tiene en el inspector una breve explicación de qué funcionalidad realizan.", _text);

        GUILayout.Space(20);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}
