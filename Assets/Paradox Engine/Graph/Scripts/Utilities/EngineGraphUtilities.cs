using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using System;

using ParadoxEngine.Utilities.Parameters;

namespace ParadoxEngine.Utilities
{
    public enum EnumNodeType
    {
        Text,
        Branch_Question,
        Branch_Condition,
        Condition,
        Answer,
        Start,
        End,
        Delay,

        Show_Character,
        Change_Character_Sprite,
        Move_Character,
        Hide_Character,

        Change_Background,

        Clear,
        Change_Flow_Chart,
        Set_Param,
        Trigger_Event,

        Play_Music,
        Stop_Music,

        Play_Sound,
        Stop_Sound,

        Play_Voice,
        Stop_Voice,

        Hide_Text_Container,
        Show_Text_Container,
    }

    public enum EnumCompareType
    {
        Equal,
        Greater,
        Less
    }

    public enum EnumModType
    {
        Swap,
        Add,
        Subtract
    }

    public enum EnumSoundTransition
    {
        Fade_In = 0,
        Fade_Out_And_In = 1,
        Instant = 2,
        Crossfade = 3
    }

    public enum EnumSoundEndTransition
    {
        FadeOut,
        Instant
    }

    public enum EnumImageTransition
    {
        Instant,
        Crossfade,
        Fade_Out_and_In,
        Slide_Left,
        Slide_Right,
        Slide_Up,
        Slide_Down
    }

    public enum EnumCharacterTransition
    {
        Instante,
        Fade,
        Slide_Left,
        Slide_Right,
        Slide_Up,
        Slide_Down
    }

    public enum EnumPosition
    {
        Center,
        Right,
        Left,
        Custom
    }

    public enum EnumPositionTransition
    {
        Instante,
        Slide,
        Fade
    }

    public static class EngineGraphUtilities
    {
        public static int IndexOf<TSource>(this IEnumerable<TSource> collection, TSource element)
        {
            int index = 0;

            foreach (var item in collection)
            {
                if (item.Equals(element))
                    return index;

                index++;
            }

            return -1;
        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> collection, Func<TSource, bool> predicate)
        {
            int index = 0;

            foreach (var item in collection)
            {
                if (predicate(item))
                    return index;

                index++;
            }

            return -1;
        }

        public static TValue GetValue<TValue>(this IEnumerable<TValue> collection, int index)
        {
            int temp = 0;

            foreach (var item in collection)
            {
                if (temp == index)
                    return item;

                temp++;
            }

            return default;
        }

        public static TResult GetValue<TSource, TResult>(this IEnumerable<ParadoxTuple<TSource, TResult>> collection, Func<TSource, bool> predicate)
        {
            foreach (var item in collection)
            {
                if (predicate(item.Item1))
                    return item.Item2;
            }

            return default;
        }

        public static TResult GetValue<TSource, TResult>(this IEnumerable<Tuple<TSource, TResult>> collection, Func<TSource, bool> predicate)
        {
            foreach (var item in collection)
            {
                if (predicate(item.Item1))
                    return item.Item2;
            }
            return default;
        }

        public static void Print(this object log)
        {
            Debug.Log(log.ToString());
        }

#if UNITY_EDITOR
        public static void SetDirty(this ScriptableObject asset)
        {
            //if (Application.isPlaying)
            //    return;

            UnityEditor.EditorUtility.SetDirty(asset);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            //UnityEditor.AssetDatabase.SaveAssets();
            //UnityEditor.AssetDatabase.Refresh();
        }
#endif

        public static float GetXPosition(EnumPosition position)
        {
            switch (position)
            {
                case EnumPosition.Center:
                    return .5f;
                case EnumPosition.Right:
                    return .75f;
                case EnumPosition.Left:
                    return .25f;
            }

            return 0f;
        }


        public static Params LoadParameterReference() => Resources.Load<Params>("Data/Settings/Parameters");

        /// <summary>
        /// Get the anchored position in a canvas from an absolute position.
        /// </summary>
        /// <param name="rect">Rect of the object.</param>
        /// <param name="point">A point of 0 to 1, left to right and down to up</param>
        /// <param name="canvasRect"></param>
        /// <returns></returns>
        public static Vector2 FromAbsolutePositionToAnchoredPosition(RectTransform rect, Vector2 point, RectTransform canvasRect)
        {
            Vector2 centerAnchor = (rect.anchorMax + rect.anchorMin) * .5f;
            return Vector2.Scale(point - centerAnchor, canvasRect.rect.size) + Vector2.Scale(rect.rect.size, Vector2.zero);
        }
    }

    [Serializable]
    public struct ParadoxTextNodeData
    {
        public string Text;

        public bool IsDialogue;
        public DCharacter Character;

        public bool UseDelay;
        public float DelayTime;

        public bool UseCustomKey;
        public KeyCode CustomKey;

        public bool ContinueParagraph;
        public bool ClearLastText;

        public void UpdateText(string newText) => Text = newText;
    }
}