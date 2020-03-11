using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using ParadoxEngine;
using System;
using System.Linq;

public class VNDatabaseWindow : EditorWindow
{
    private static VNDatabaseWindow _currentWindow;
    private DataContainerType _currentViewSelected;
    private Vector2 _pos;
    private float _width = 1200;
    private float _height = 723;
    private float _widthMin = 632;
    private float _heightMin = 466;
    private bool _toDelete = false;

    private Action _OnRepaint = delegate { };

    private int _characterSelected;
    private Container _characterContainer;
    private int _locationSelected;
    private Container _locationContainer;
    private int _soundSelected;
    private Container _soundContainer;
    private DSetting _settingData;

    private GUIStyle _title;
    private GUIStyle _subTitle;
    private GUIStyle _description;

    private AnimBool _useSprite;
    private AnimBool _useAnimation;


    public static void InitParadoxDatabaseWindow()
    {
        _currentWindow = (VNDatabaseWindow)EditorWindow.GetWindow<VNDatabaseWindow>();
        _currentWindow.titleContent = new GUIContent("Paradox Engine: Database");

        _currentWindow.maxSize = new Vector2(_currentWindow._width, _currentWindow._height);
        _currentWindow.minSize = new Vector2(_currentWindow._widthMin, _currentWindow._heightMin);

        _currentWindow._currentViewSelected = DataContainerType.Default;

        _currentWindow._characterSelected = 0;
        _currentWindow._characterContainer = (Container)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Characters.asset", typeof(Container));

        _currentWindow._locationSelected = 0;
        _currentWindow._locationContainer = (Container)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Locations.asset", typeof(Container));

        _currentWindow._soundSelected = 0;
        _currentWindow._soundContainer = (Container)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Soundtracks.asset", typeof(Container));

        _currentWindow._settingData = (DSetting)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Settings.asset", typeof(DSetting));

        if (_currentWindow._characterContainer.data == null)
            _currentWindow._characterContainer.data = new List<PEData>();

        if (_currentWindow._locationContainer.data == null)
            _currentWindow._locationContainer.data = new List<PEData>();

        if (_currentWindow._soundContainer.data == null)
            _currentWindow._soundContainer.data = new List<PEData>();

        _currentWindow._useSprite = new AnimBool();
        _currentWindow._useSprite.valueChanged.AddListener(_currentWindow.Repaint);

        _currentWindow._useAnimation = new AnimBool();
        _currentWindow._useAnimation.valueChanged.AddListener(_currentWindow.Repaint);
    }


    private void OnGUI()
    {
        _title = new GUIStyle(GUI.skin.label);
        _title.alignment = TextAnchor.MiddleCenter;
        _title.fontSize = 20;
        _title.fontStyle = FontStyle.Bold;

        _subTitle = new GUIStyle(GUI.skin.label);
        _subTitle.fontSize = 14;
        _subTitle.fontStyle = FontStyle.Bold;

        _description = new GUIStyle(GUI.skin.label);
        _description.fontStyle = FontStyle.Italic;
        _description.wordWrap = true;

        GUILayout.Space(20);

        GUILayout.BeginHorizontal("BOX");

        if (GUILayout.Button("Characters"))
        {
            _currentViewSelected = DataContainerType.Character;
            _toDelete = false;
        }

        if (GUILayout.Button("Locations"))
        {
            _currentViewSelected = DataContainerType.Location;
            _toDelete = false;
        }

        if (GUILayout.Button("Soundtrack"))
        {
            _currentViewSelected = DataContainerType.Soundtrack;
            _toDelete = false;
        }

        if (GUILayout.Button("Setting"))
        {
            _currentViewSelected = DataContainerType.Setting;
            _toDelete = false;
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);

        EditorGUILayout.BeginVertical();
        _pos = EditorGUILayout.BeginScrollView(_pos, GUILayout.Width(position.width));

        DrawView();

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        Repaint();
    }


    private void DrawView()
    {
        if (_currentViewSelected == DataContainerType.Character)
            CharacterView();

        else if (_currentViewSelected == DataContainerType.Location)
            LocationView();

        else if (_currentViewSelected == DataContainerType.Soundtrack)
            SoundView();

        else if (_currentViewSelected == DataContainerType.Setting)
            SettingView();

        else
            DefaultView();
    }


    private void DefaultView()
    {
        GUILayout.Space(4);
        GUILayout.Label("Paradox Engine: Database", _title);
        GUILayout.Space(10);

        GUILayout.Label("  Characters", _subTitle);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(25, 1), Color.black);
        GUILayout.Space(6);

        GUILayout.BeginVertical("BOX");
        EditorGUILayout.LabelField("La sección de «Characters» permite definir personajes para utilizarlos de forma sencilla a los personajes. \n\nA cada uno se le puede agregar sprites y/o animaciones, los cuales facilitarán su implementación en el grapho.", _description);
        GUILayout.EndVertical();

        GUILayout.Space(10);

        GUILayout.Label("  Locations", _subTitle);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(25, 1), Color.black);
        GUILayout.Space(6);

        GUILayout.BeginVertical("BOX");
        EditorGUILayout.LabelField("La sección de «Locations» permite definir los escenarios (backgrounds) para utilizar fácilmente en el grapho. \n\nA cada escenario se le puede agregar más de un sprite.", _description);
        GUILayout.EndVertical();

        GUILayout.Space(10);

        GUILayout.Label("  Soundtrack", _subTitle);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(25, 1), Color.black);
        GUILayout.Space(6);

        GUILayout.BeginVertical("BOX");
        EditorGUILayout.LabelField("La sección «Soundtracks» permite definir grupos de sonidos para organizarlos de forma predefinida. \n\nCada grupo puede ser colocado en uno de los canales disponibles: música, efectos de sonido y voces.", _description);
        GUILayout.EndVertical();

        GUILayout.Space(5);
    }

    private void CharacterView()
    {
        EditorGUILayout.LabelField("Characters", _title);
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal("BOX");

        if (GUILayout.Button("Define Character"))
            VNDatabasePopUpWindow.InitNodePopup(DataContainerType.Character);

        if (_characterContainer.data.Count == 0)
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("You don't have a character defined.", MessageType.Warning);
            return;
        }

        var characters = _characterContainer.data.Select(x => x as DCharacter).ToArray();
        var names = characters.Select(x => x.GetIdentificator()).ToArray();

        _characterSelected = EditorGUILayout.Popup("Character:", _characterSelected, names);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (_toDelete)
        {
            if (GUILayout.Button("Delete"))
            {
                _toDelete = false;

                var temp = characters[_characterSelected];

                _characterContainer.data.Remove(temp);
                EditorUtility.SetDirty(_characterContainer);
                _characterSelected = 0;

                var graphs = Resources.LoadAll("Data").Select(x => x as EngineGraph).Where(x => x != null);

                foreach (var item in graphs)
                {
                    var show = item.nodes.Where(x => x.nodeType == ParadoxEngine.Utilities.EnumNodeType.Show_Character)
                                         .Select(x => x as NShowCharacter)
                                         .Where(x => x.character == temp);

                    foreach (var node in show)
                    {
                        node.character = null;
                        EditorUtility.SetDirty(node);
                    }


                    var hide = item.nodes.Where(x => x.nodeType == ParadoxEngine.Utilities.EnumNodeType.Hide_Character)
                                         .Select(x => x as NHideCharacter)
                                         .Where(x => x.character == temp);

                    foreach (var node in hide)
                    {
                        node.character = null;
                        EditorUtility.SetDirty(node);
                    }


                    var change = item.nodes.Where(x => x.nodeType == ParadoxEngine.Utilities.EnumNodeType.Change_Character_Sprite)
                                           .Select(x => x as NChangeCharacterSprite)
                                           .Where(x => x.character == temp);

                    foreach (var node in change)
                    {
                        node.character = null;
                        EditorUtility.SetDirty(node);
                    }


                    var move = item.nodes.Where(x => x.nodeType == ParadoxEngine.Utilities.EnumNodeType.Move_Character)
                                         .Select(x => x as NMoveCharacter)
                                         .Where(x => x.character == temp);

                    foreach (var node in move)
                    {
                        node.character = null;
                        EditorUtility.SetDirty(node);
                    }
                }

                VNDatabaseUtilitie.DeleteElem(temp);
                return;
            }

            if (GUILayout.Button("Cancel"))
                _toDelete = false;
        }

        else
        {
            if (GUILayout.Button("Save Characters"))
            {
                EditorUtility.SetDirty(_characterContainer);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Delete Character"))
                _toDelete = true;
        }

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20);

        var name = characters[_characterSelected].GetIdentificator();
        characters[_characterSelected].SetIdentificator(EditorGUILayout.TextField("Character name:", name));

        GUILayout.Space(5);

        #region Sprite label
        GUILayout.Label("  Sprites", _subTitle);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(25, 2), Color.black);
        GUILayout.Space(6);

        EditorGUILayout.BeginVertical("BOX");

        var sprites = characters[_characterSelected].SpriteForeach();

        if (sprites.Any())
            foreach (var sprite in sprites)
            {
                EditorGUILayout.BeginHorizontal("BOX");

                EditorGUILayout.BeginVertical();
                sprite.Item2 = (Sprite)EditorGUILayout.ObjectField(sprite.Item2, typeof(Sprite), true);

                if (!sprite.Item2)
                    EditorGUILayout.HelpBox("The sprite can't be null", MessageType.Error);

                else if (sprites.Where(x => x != sprite).Any(x => x.Item2 == sprite.Item2))
                    EditorGUILayout.HelpBox("The character already contains this sprite", MessageType.Warning);

                EditorGUILayout.EndVertical();


                EditorGUILayout.BeginVertical();

                sprite.Item1 = EditorGUILayout.TextField("Name:", sprite.Item1);

                if (!sprite.Item1.Any() || sprite.Item1 == " ")
                    EditorGUILayout.HelpBox("The name can't be null or a backspace.", MessageType.Error);

                else if (sprites.Where(x => x != sprite).Any(x => x.Item1 == sprite.Item1))
                    EditorGUILayout.HelpBox("The character already contains this name.", MessageType.Warning);

                EditorGUILayout.EndVertical();

                if (GUILayout.Button("Unsuscribe"))
                {
                    characters[_characterSelected].UnsuscribeSprite(sprite);
                    return;
                }

                EditorGUILayout.EndHorizontal();
            }

        else
            EditorGUILayout.HelpBox("The character don't have sprites.", MessageType.Warning);

        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Suscribe sprite"))
            characters[_characterSelected].SuscribeSprite("", default(Sprite));
        #endregion
    }

    private void LocationView()
    {
        EditorGUILayout.LabelField("Backgrounds", _title);
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal("BOX");

        if (GUILayout.Button("Define Background"))
            VNDatabasePopUpWindow.InitNodePopup(DataContainerType.Location);

        if (_locationContainer.data.Count == 0)
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("You don't have a background defined.", MessageType.Warning);
            return;
        }

        var locations = _locationContainer.data.Select(x => x as DLocation).ToArray();
        var names = locations.Select(x => x.GetIdentificator()).ToArray();

        _locationSelected = EditorGUILayout.Popup("Locations:", _locationSelected, names);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (_toDelete)
        {
            if (GUILayout.Button("Delete"))
            {
                _toDelete = false;
                var temp = locations[_locationSelected];

                _locationContainer.data.Remove(temp);
                EditorUtility.SetDirty(_locationContainer);
                _locationSelected = 0;

                var graphs = Resources.LoadAll("Data").Select(x => x as EngineGraph).Where(x => x != null);

                foreach (var item in graphs)
                {
                    var change = item.nodes.Where(x => x.nodeType == ParadoxEngine.Utilities.EnumNodeType.Change_Background)
                                           .Select(x => x as NChangeBackground)
                                           .Where(x => x.locationGroups == temp);

                    foreach (var node in change)
                    {
                        node.locationGroups = null;
                        EditorUtility.SetDirty(node);
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                VNDatabaseUtilitie.DeleteElem(temp);
                return;
            }

            if (GUILayout.Button("Cancel"))
                _toDelete = false;
        }

        else
        {
            if (GUILayout.Button("Save Locations"))
            {
                EditorUtility.SetDirty(_locationContainer);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Delete Background"))
                _toDelete = true;
        }

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20);

        var name = locations[_locationSelected].GetIdentificator();
        name = EditorGUILayout.TextField("Background name:", name);
        locations[_locationSelected].SetIdentificator(name);

        GUILayout.Space(5);

        GUILayout.Label("  Sprites", _subTitle);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(25, 2), Color.black);
        GUILayout.Space(6);

        EditorGUILayout.BeginVertical("BOX");

        var sprites = locations[_locationSelected].Foreach();

        if (sprites.Any())
            foreach (var sprite in sprites)
            {
                EditorGUILayout.BeginHorizontal("BOX");

                EditorGUILayout.BeginVertical();

                sprite.Item2 = (Sprite)EditorGUILayout.ObjectField(sprite.Item2, typeof(Sprite), true);

                if (!sprite.Item2)
                    EditorGUILayout.HelpBox("The sprite can't be null", MessageType.Error);

                else if (sprites.Where(x => x != sprite).Any(x => x.Item2 == sprite.Item2))
                    EditorGUILayout.HelpBox("The location already contains this sprite", MessageType.Warning);

                EditorGUILayout.EndVertical();


                EditorGUILayout.BeginVertical();
                sprite.Item1 = EditorGUILayout.TextField("Name:", sprite.Item1);

                if (!sprite.Item1.Any() || sprite.Item1 == " ")
                    EditorGUILayout.HelpBox("The name can't be null or a backspace", MessageType.Error);

                else if (sprites.Where(x => x != sprite).Any(x => x.Item1 == sprite.Item1))
                    EditorGUILayout.HelpBox("The location already contains this name", MessageType.Warning);

                EditorGUILayout.EndVertical();

                if (GUILayout.Button("Unsuscribe"))
                {
                    locations[_locationSelected].UnsuscribeBackground(sprite);
                    return;
                }

                EditorGUILayout.EndHorizontal();
            }
        else
            EditorGUILayout.HelpBox("The background don't have sprites.", MessageType.Warning);

        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Suscribe sprite"))
            locations[_locationSelected].SuscribeBackground("", default(Sprite));
    }

    private void SoundView()
    {
        EditorGUILayout.LabelField("Sounds", _title);
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal("BOX");

        if (GUILayout.Button("Define Sound Group"))
            VNDatabasePopUpWindow.InitNodePopup(DataContainerType.Soundtrack);

        if (_soundContainer.data.Count == 0)
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("You don't have a sound group defined.", MessageType.Warning);
            return;
        }

        var sounds = _soundContainer.data.Select(x => x as DSoundtrack).ToArray();
        var names = sounds.Select(x => x.GetIdentificator()).ToArray();

        _soundSelected = EditorGUILayout.Popup("Sound group:", _soundSelected, names);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (_toDelete)
        {
            if (GUILayout.Button("Delete"))
            {
                _toDelete = false;

                var temp = sounds[_soundSelected];
                _soundContainer.data.Remove(temp);
                EditorUtility.SetDirty(_soundContainer);
                _soundSelected = 0;

                var graphs = Resources.LoadAll("Data").Select(x => x as EngineGraph).Where(x => x != null);

                foreach (var item in graphs)
                {
                    var playMusic = item.nodes.Where(x => x.nodeType == ParadoxEngine.Utilities.EnumNodeType.Play_Music)
                                              .Select(x => x as NPlayMusic)
                                              .Where(x => x.musicGroup == temp);

                    foreach (var node in playMusic)
                    {
                        node.musicGroup = null;
                        EditorUtility.SetDirty(node);
                    }


                    var playSound = item.nodes.Where(x => x.nodeType == ParadoxEngine.Utilities.EnumNodeType.Play_Sound)
                                              .Select(x => x as NPlaySound)
                                              .Where(x => x.soundGroup == temp);

                    foreach (var node in playSound)
                    {
                        node.soundGroup = null;
                        EditorUtility.SetDirty(node);
                    }


                    var playVoice = item.nodes.Where(x => x.nodeType == ParadoxEngine.Utilities.EnumNodeType.Play_Voice)
                                              .Select(x => x as NPlayVoice)
                                              .Where(x => x.voiceGroup == temp);

                    foreach (var node in playVoice)
                    {
                        node.voiceGroup = null;
                        EditorUtility.SetDirty(node);
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                VNDatabaseUtilitie.DeleteElem(temp);
                return;
            }

            if (GUILayout.Button("Cancel"))
                _toDelete = false;
        }

        else
        {
            if (GUILayout.Button("Save Sounds group"))
            {
                EditorUtility.SetDirty(_soundContainer);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Delete Sound Group"))
                _toDelete = true;
        }

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20);

        var name = sounds[_soundSelected].GetIdentificator();
        name = EditorGUILayout.TextField("Sound group name:", name);
        sounds[_soundSelected].SetIdentificator(name);

        GUILayout.Space(5);

        var soundsChannel = new string[3] { "Music", "SoundFX", "Voice" };
        sounds[_soundSelected].Channel = (SoundChannelEnum)EditorGUILayout.Popup("Channel:", (int)sounds[_soundSelected].Channel, soundsChannel);

        GUILayout.Label("  Sounds", _subTitle);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(25, 2), Color.black);
        GUILayout.Space(6);

        EditorGUILayout.BeginVertical("BOX");

        var clips = sounds[_soundSelected].Foreach();

        if (clips.Any())
            foreach (var clip in clips)
            {
                EditorGUILayout.BeginHorizontal("BOX");

                EditorGUILayout.BeginVertical();
                clip.Item2 = (AudioClip)EditorGUILayout.ObjectField(clip.Item2, typeof(AudioClip), true);

                if (!clip.Item2)
                    EditorGUILayout.HelpBox("The clip can't be null", MessageType.Error);

                else if (clips.Where(x => x != clip).Any(x => x.Item2 == clip.Item2))
                    EditorGUILayout.HelpBox("The sound group already contains this clip", MessageType.Warning);

                EditorGUILayout.EndVertical();


                EditorGUILayout.BeginVertical();
                clip.Item1 = EditorGUILayout.TextField("Sound name:", clip.Item1);

                if (!clip.Item1.Any() || clip.Item1 == " ")
                    EditorGUILayout.HelpBox("The name can't be null or a backspace", MessageType.Error);

                else if (clips.Where(x => x != clip).Any(x => x.Item1 == clip.Item1))
                    EditorGUILayout.HelpBox("The sound group already contains this name", MessageType.Warning);

                EditorGUILayout.EndVertical();

                if (GUILayout.Button("Unsuscribe"))
                {
                    sounds[_soundSelected].UnsuscribeSound(clip);
                    return;
                }

                EditorGUILayout.EndHorizontal();
            }
        else
            EditorGUILayout.HelpBox("The sound group don't have sounds.", MessageType.Warning);

        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Suscribe sound"))
            sounds[_soundSelected].SuscribeSound("", default(AudioClip));
    }

    private void SettingView()
    {
        EditorGUILayout.LabelField("Setting", _title);
        GUILayout.Space(10);

        if (GUILayout.Button("Save settings"))
        {
            EditorUtility.SetDirty(_settingData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        GUILayout.Space(20);

        GUILayout.Label("  Text", _subTitle);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(25, 2), Color.black);
        GUILayout.Space(6);

        _settingData.TimeForChar = EditorGUILayout.FloatField(new GUIContent("Time per char: ", "Time duration per char of text write in the canvas."), _settingData.TimeForChar);

        if (_settingData.Count > 0)
        {
            EditorGUILayout.BeginVertical("BOX");
            for (int i = 0; i < _settingData.Count; i++)
            {
                EditorGUILayout.BeginHorizontal("BOX");

                _settingData[i] = (KeyCode)EditorGUILayout.EnumPopup("Key: ", _settingData[i]);

                if (GUILayout.Button("Unsuscribe"))
                {
                    int index = i;
                    _OnRepaint += () => _settingData.RemoveAt(index);
                }

                EditorGUILayout.EndHorizontal();

                if (_settingData.Contains(_settingData[i], i))
                    EditorGUILayout.HelpBox("This key is already contained in the settings.", MessageType.Error);
            }
            EditorGUILayout.EndVertical();

            _OnRepaint();
            _OnRepaint = delegate { };

            if (GUILayout.Button("Suscribe Key"))
                _settingData.Add(KeyCode.A);
        }

        else
            EditorGUILayout.HelpBox("The Settings don't have keys registered.", MessageType.Warning);
    }


    public static void AsignElem(PEData elem)
    {
        if (elem as DCharacter)
        {
            _currentWindow._characterContainer.data = _currentWindow._characterContainer.data.OrderBy(x => x.GetIdentificator()[0]).ToList();
            _currentWindow._characterSelected = _currentWindow._characterContainer.data.IndexOf(elem);
        }

        else if (elem as DLocation)
        {
            _currentWindow._locationContainer.data = _currentWindow._locationContainer.data.OrderBy(x => x.GetIdentificator()[0]).ToList();
            _currentWindow._locationSelected = _currentWindow._locationContainer.data.IndexOf(elem);
        }

        else if (elem as DSoundtrack)
        {
            _currentWindow._soundContainer.data = _currentWindow._soundContainer.data.OrderBy(x => x.GetIdentificator()[0]).ToList();
            _currentWindow._soundSelected = _currentWindow._soundContainer.data.IndexOf(elem);
        }
    }
}
