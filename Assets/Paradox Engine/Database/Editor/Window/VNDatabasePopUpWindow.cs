using UnityEngine;
using UnityEditor;
using ParadoxEngine;

public class VNDatabasePopUpWindow : EditorWindow
{
    private static VNDatabasePopUpWindow _currentPop;
    private string assetName = "Enter a name...";
    private DataContainerType _idToCreate;


    public static void InitNodePopup(DataContainerType id)
    {
        _currentPop = (VNDatabasePopUpWindow)EditorWindow.GetWindow<VNDatabasePopUpWindow>();
        _currentPop.titleContent = new GUIContent("Paradox Engine Assistante");
        _currentPop._idToCreate = id;
    }


    private void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("Create a new file:", EditorStyles.boldLabel);
        assetName = EditorGUILayout.TextField("Enter name: ", assetName);

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button($"Create a new {_idToCreate.ToString()}.", GUILayout.Height(40)) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(assetName) && assetName != "Enter a name...")
            {
                switch (_idToCreate)
                {
                    case DataContainerType.Character:
                        CreateCharacter();
                        break;
                    case DataContainerType.Location:
                        CreateBackground();
                        break;
                    case DataContainerType.Soundtrack:
                        CreateSoundtrack();
                        break;
                }

                _currentPop.Close();
            }

            else
                EditorUtility.DisplayDialog("Paradox Engine Message", "Please, insert a valid name.", "OK");
        }

        if (GUILayout.Button("Cancel", GUILayout.Height(40)))
            _currentPop.Close();

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.Space(20);
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
    }

    private void CreateCharacter()
    {
        DCharacter chara = (DCharacter)ScriptableObject.CreateInstance<DCharacter>();
        chara.SetIdentificator(assetName);
        chara.name = assetName;

        var container = (Container)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Characters.asset", typeof(Container));
        container.data.Add(chara);
        AssetDatabase.AddObjectToAsset(chara, container);
        EditorUtility.SetDirty(container);
        EditorUtility.SetDirty(chara);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        VNDatabaseWindow.AsignElem(chara);
    }

    private void CreateBackground()
    {
        DLocation back = (DLocation)ScriptableObject.CreateInstance<DLocation>();
        back.SetIdentificator(assetName);
        back.name = assetName;

        var container = (Container)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Locations.asset", typeof(Container));
        container.data.Add(back);
        AssetDatabase.AddObjectToAsset(back, container);
        EditorUtility.SetDirty(container);
        EditorUtility.SetDirty(back);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        VNDatabaseWindow.AsignElem(back);
    }

    private void CreateSoundtrack()
    {
        DSoundtrack sound = (DSoundtrack)ScriptableObject.CreateInstance<DSoundtrack>();
        sound.SetIdentificator(assetName);
        sound.name = assetName;

        var container = (Container)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Soundtracks.asset", typeof(Container));
        container.data.Add(sound);
        AssetDatabase.AddObjectToAsset(sound, container);
        EditorUtility.SetDirty(container);
        EditorUtility.SetDirty(sound);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        VNDatabaseWindow.AsignElem(sound);
    }
}
