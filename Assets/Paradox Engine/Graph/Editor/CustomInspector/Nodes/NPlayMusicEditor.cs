using System.Linq;
using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NPlayMusic))]
    public class NPlayMusicEditor : Editor
    {
        private GUIStyle _title;
        private NPlayMusic _node;
        private Container _musicContainer;
        private int _musicGroupSelected;
        private int _musicClipSelected;

        private void OnEnable()
        {
            _node = (NPlayMusic)target;
            _musicContainer = (Container)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Soundtracks.asset", typeof(Container));

            if (_musicContainer.data == null || !_musicContainer.data.Any() || !_musicContainer.data.Contains(_node.musicGroup))
            {
                _node.musicGroup = null;
                _node.musicClip = null;
                return;
            }

            if (_node.musicGroup != null)
                _musicGroupSelected = _musicContainer.data.Select(x => (DSoundtrack)x).Where(x => x.Channel == SoundChannelEnum.Music).IndexOf(x => x == _node.musicGroup);

            else
            {
                _node.musicGroup = (DSoundtrack)_musicContainer.data.Select(x => x as DSoundtrack).Where(x => x.Channel == SoundChannelEnum.Music).First();
                _musicGroupSelected = 0;
            }

            if (_node.musicClip != null)
            {
                if (_node.musicGroup.ContainsSound(_node.musicClip))
                    _musicClipSelected = _node.musicGroup.Foreach().IndexOf(x => x.Item2 == _node.musicClip);

                else
                {
                    _node.musicClip = _node.musicGroup.Foreach().GetValue(0).Item2;
                    _musicClipSelected = 0;
                }
            }

            else
            {
                _node.musicClip = _node.musicGroup.Foreach().GetValue(0).Item2;
                _musicClipSelected = 0;
            }
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

            GUILayout.Label("PARADOX ENGINE" + "\n" + "PLAY MUSIC STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Play Music state play a loopeable sound in the music channel.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();

            GUILayout.Space(10);

            if (_musicContainer.data == null || !_musicContainer.data.Select(x => x as DSoundtrack).Where(x => x.Channel == SoundChannelEnum.Music).Any())
            {
                GUILayout.BeginVertical("BOX");
                EditorGUILayout.HelpBox("The database don't have Soundtrack Groups of the Music channel.", MessageType.Warning);

                _node.musicGroup = null;
                _node.musicClip = null;

                GUILayout.EndVertical();
                return;
            }

            if (_node.musicGroup == null)
            {
                _node.musicGroup = (DSoundtrack)_musicContainer.data.Select(x => x as DSoundtrack).Where(x => x.Channel == SoundChannelEnum.Music).First();
                _musicGroupSelected = 0;

                _node.musicClip = _node.musicGroup.Foreach().GetValue(0).Item2;
                _musicClipSelected = 0;
            }

            var allMusicGroups = _musicContainer.data.Select(x => x as DSoundtrack).Where(x => x.Channel == SoundChannelEnum.Music).Select(x => x.name).ToArray();

            GUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField(new GUIContent("Music group:", "Select a Soundtrack Group defined has a Music channel."));
            _musicGroupSelected = EditorGUILayout.Popup(_musicGroupSelected, allMusicGroups);

            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                _node.musicGroup = (DSoundtrack)_musicContainer.data.Select(x => x as DSoundtrack)
                                .Where(x => x.Channel == SoundChannelEnum.Music)
                                .GetValue(_musicGroupSelected);

                EngineGraphUtilities.SetDirty(_node);
            }

            GUILayout.Space(5);

            EditorGUI.BeginChangeCheck();

            var allMusicClips = _node.musicGroup.Foreach().Select(x => x.Item1).ToArray();

            GUILayout.BeginHorizontal();

            if (allMusicClips.Any())
            {
                EditorGUILayout.LabelField(new GUIContent("Music:", "Select a Audio Clip registed in the Soundtrack Group."));
                _musicClipSelected = EditorGUILayout.Popup(_musicClipSelected, allMusicClips);
            }

            else
            {
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("BOX");
                EditorGUILayout.HelpBox("The database don't have Audio Clips in the Soundtrack Group.", MessageType.Warning);
                _node.musicClip = null;
                _musicClipSelected = 0;
                GUILayout.EndVertical();
                return;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("BOX");


            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Duration time:", "Duration of the transition."));
            _node.timeDuration = EditorGUILayout.FloatField(_node.timeDuration);
            GUILayout.EndHorizontal();

            if (_node.timeDuration <= 0)
                _node.timeDuration = 1;

            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Transition type:", "Type of transition to start the Music."));
            string[] compareTag = new string[4] { "Fade In", "Fade Out and In", "Instant", "Crossfade" };
            int selectedTag = (int)_node.transition;

            selectedTag = EditorGUILayout.Popup(selectedTag, compareTag);

            if (EditorGUI.EndChangeCheck())
            {
                _node.musicClip = _node.musicGroup[allMusicClips[_musicClipSelected]];
                _node.transition = (EnumSoundTransition)selectedTag;
                EngineGraphUtilities.SetDirty(_node);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}