using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;
using System.Linq;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NPlaySound))]
    public class NPlaySoundEditor : Editor
    {
        private GUIStyle _title;
        private NPlaySound _node;
        private Container _soundContainer;
        private int _soundGroupSelected;
        private int _soundClipSelected;

        private void OnEnable()
        {
            _node = (NPlaySound)target;
            _soundContainer = (Container)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Soundtracks.asset", typeof(Container));

            if (_soundContainer.data == null || !_soundContainer.data.Any() || !_soundContainer.data.Contains(_node.soundGroup))
            {
                _node.soundGroup = null;
                _node.soundClip = null;
                return;
            }

            if (_node.soundGroup != null)
                _soundGroupSelected = _soundContainer.data.Select(x => (DSoundtrack)x).Where(x => x.Channel == SoundChannelEnum.SoundFX).IndexOf(x => x == _node.soundGroup);

            else
            {
                _node.soundGroup = _node.soundGroup = (DSoundtrack)_soundContainer.data.Select(x => x as DSoundtrack).Where(x => x.Channel == SoundChannelEnum.SoundFX).First();
                _soundGroupSelected = 0;
            }


            if (_node.soundClip != null)
            {
                if (_node.soundGroup.ContainsSound(_node.soundClip))
                    _soundClipSelected = _node.soundGroup.Foreach().IndexOf(x => x.Item2 == _node.soundClip);

                else
                {
                    _node.soundClip = _node.soundGroup.Foreach().GetValue(0).Item2;
                    _soundClipSelected = 0;
                }
            }

            else
            {
                _node.soundClip = _node.soundGroup.Foreach().GetValue(0).Item2;
                _soundClipSelected = 0;
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

            GUILayout.Label("PARADOX ENGINE" + "\n" + "PLAY SOUND FX STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Play Sound FX state play a audioclip in the sound fx channel.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();

            GUILayout.Space(10);


            if (_soundContainer.data == null || !_soundContainer.data.Select(x => x as DSoundtrack).Where(x => x.Channel == SoundChannelEnum.SoundFX).Any())
            {
                GUILayout.BeginVertical("BOX");
                EditorGUILayout.HelpBox("The database don't have Soundtrack Groups of the Sound FX channel.", MessageType.Warning);

                _node.soundGroup = null;
                _node.soundClip = null;

                GUILayout.EndVertical();
                return;
            }

            if (_node.soundGroup == null)
            {
                _node.soundGroup = (DSoundtrack)_soundContainer.data.Select(x => x as DSoundtrack).Where(x => x.Channel == SoundChannelEnum.SoundFX).First();
                _soundGroupSelected = 0;

                _node.soundClip = _node.soundGroup.Foreach().GetValue(0).Item2;
                _soundClipSelected = 0;
            }

            var allSoundGroups = _soundContainer.data.Select(x => x as DSoundtrack).Where(x => x.Channel == SoundChannelEnum.SoundFX).Select(x => x.name).ToArray();

            GUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField(new GUIContent("Sound FX group:", "Select a Soundtrack Group defined has a Sound FX channel."));
            _soundGroupSelected = EditorGUILayout.Popup(_soundGroupSelected, allSoundGroups);


            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                _node.soundGroup = (DSoundtrack)_soundContainer.data.Select(x => x as DSoundtrack)
                                .Where(x => x.Channel == SoundChannelEnum.SoundFX)
                                .GetValue(_soundGroupSelected);

                EngineGraphUtilities.SetDirty(_node);
            }

            GUILayout.Space(5);

            EditorGUI.BeginChangeCheck();

            var allSoundClips = _node.soundGroup.Foreach().Select(x => x.Item1).ToArray();

            GUILayout.BeginHorizontal();

            if (allSoundClips.Any())
            {
                EditorGUILayout.LabelField(new GUIContent("Sound FX:", "Select a Audio Clip registed in the Soundtrack Group."));
                _soundClipSelected = EditorGUILayout.Popup(_soundClipSelected, allSoundClips);
            }

            else
            {
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("BOX");
                EditorGUILayout.HelpBox("The database don't have Audio Clips in the Soundtrack Group.", MessageType.Warning);
                _node.soundClip = null;
                _soundClipSelected = 0;
                GUILayout.EndVertical();
                return;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Loop:", "If tue the Sound FX will be played in a loop."));
            _node.canLoop = EditorGUILayout.Toggle(_node.canLoop);
            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                _node.soundClip = _node.soundGroup[allSoundClips[_soundClipSelected]];
                EngineGraphUtilities.SetDirty(_node);
            }
        }
    }
}