using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;
using System.Linq;

namespace ParadoxEngine.CustomInspector
{
    [CustomEditor(typeof(NPlayVoice))]
    public class NPlayVoiceEditor : Editor
    {
        private GUIStyle _title;
        private NPlayVoice _node;
        private Container _voiceContainer;
        private int _voiceGroupSelected;
        private int _voiceClipSelected;

        private void OnEnable()
        {
            _node = (NPlayVoice)target;
            _voiceContainer = (Container)AssetDatabase.LoadAssetAtPath("Assets/Paradox Engine/Database/Data/Soundtracks.asset", typeof(Container));

            if (_voiceContainer.data == null || !_voiceContainer.data.Any() || !_voiceContainer.data.Contains(_node.voiceGroup))
            {
                _node.voiceGroup = null;
                _node.voiceClip = null;
                return;
            }

            if (_node.voiceGroup != null)
                _voiceGroupSelected = _voiceContainer.data.Select(x => (DSoundtrack)x).Where(x => x.Channel == SoundChannelEnum.Voice).IndexOf(x => x == _node.voiceGroup);

            else
            {
                _node.voiceGroup = _node.voiceGroup = (DSoundtrack)_voiceContainer.data.Select(x => x as DSoundtrack).Where(x => x.Channel == SoundChannelEnum.Voice).First();
                _voiceGroupSelected = 0;
            }


            if (_node.voiceClip != null)
            {
                if (_node.voiceGroup.ContainsSound(_node.voiceClip))
                    _voiceClipSelected = _node.voiceGroup.Foreach().IndexOf(x => x.Item2 == _node.voiceClip);

                else
                {
                    _node.voiceClip = _node.voiceGroup.Foreach().GetValue(0).Item2;
                    _voiceClipSelected = 0;
                }
            }

            else
            {
                _node.voiceClip = _node.voiceGroup.Foreach().GetValue(0).Item2;
                _voiceClipSelected = 0;
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

            GUILayout.Label("PARADOX ENGINE" + "\n" + "PLAY VOICE STATE", _title);

            GUILayout.Space(10);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
            GUILayout.Space(20);

            EditorGUILayout.LabelField("Info:");
            GUILayout.BeginVertical("BOX");
            EditorGUILayout.LabelField("\n The Play voice state play a clip in the voice channel.\n", new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Italic });
            GUILayout.EndVertical();

            GUILayout.Space(10);

            if (_voiceContainer.data == null || !_voiceContainer.data.Select(x => x as DSoundtrack).Where(x => x.Channel == SoundChannelEnum.Voice).Any())
            {
                GUILayout.BeginVertical("BOX");
                EditorGUILayout.HelpBox("The database don't have Soundtrack Groups of the Voice channel.", MessageType.Warning);

                _node.voiceGroup = null;
                _node.voiceClip = null;

                GUILayout.EndVertical();
                return;
            }

            if (_node.voiceGroup == null)
            {
                _node.voiceGroup = (DSoundtrack)_voiceContainer.data.Select(x => x as DSoundtrack).Where(x => x.Channel == SoundChannelEnum.Voice).First();
                _voiceGroupSelected = 0;

                _node.voiceClip = _node.voiceGroup.Foreach().GetValue(0).Item2;
                _voiceClipSelected = 0;
            }

            var allSoundGroups = _voiceContainer.data.Select(x => x as DSoundtrack).Where(x => x.Channel == SoundChannelEnum.Voice).Select(x => x.name).ToArray();

            GUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField(new GUIContent("Voice group:", "Select a Soundtrack Group defined has a Voice channel."));
            _voiceGroupSelected = EditorGUILayout.Popup(_voiceGroupSelected, allSoundGroups);


            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                _node.voiceGroup = (DSoundtrack)_voiceContainer.data.Select(x => x as DSoundtrack)
                                .Where(x => x.Channel == SoundChannelEnum.Voice)
                                .GetValue(_voiceGroupSelected);

                EngineGraphUtilities.SetDirty(_node);
            }

            GUILayout.Space(5);

            EditorGUI.BeginChangeCheck();

            var allVoiceClips = _node.voiceGroup.Foreach().Select(x => x.Item1).ToArray();

            GUILayout.BeginHorizontal();

            if (allVoiceClips.Any())
            {
                EditorGUILayout.LabelField(new GUIContent("Voice:", "Select a Audio Clip registed in the Soundtrack Group."));
                _voiceClipSelected = EditorGUILayout.Popup(_voiceClipSelected, allVoiceClips);
            }

            else
            {
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("BOX");
                EditorGUILayout.HelpBox("The database don't have Audio Clips in the Soundtrack Group.", MessageType.Warning);
                _node.voiceClip = null;
                _voiceClipSelected = 0;
                GUILayout.EndVertical();
                return;
            }

            GUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                _node.voiceClip = _node.voiceGroup[allVoiceClips[_voiceClipSelected]];
                EngineGraphUtilities.SetDirty(_node);
            }
        }
    }
}