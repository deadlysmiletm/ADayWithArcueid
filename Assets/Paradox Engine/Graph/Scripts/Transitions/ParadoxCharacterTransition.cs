using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ParadoxEngine.Utilities;

namespace ParadoxEngine.Transitions
{
    public static class ParadoxCharacterTransition
    {
        public static void Instante(Image image, Sprite sprite, EnumPosition position, NTemplate node, Vector2 point, bool reverse = false)
        {
            if (!reverse)
                image.sprite = sprite;

            image.CrossFadeAlpha(reverse? 0 : 1, 0, false);

            if (!reverse)
            {
                Vector2 pos;

                if (position == EnumPosition.Custom)
                    pos = point;
                else
                    pos = new Vector2(EngineGraphUtilities.GetXPosition(position), .5f);

                image.rectTransform.anchoredPosition = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image.rectTransform, pos, DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);
            }

            node.endedInstruction = true;
        }

        public static IEnumerator Fade(Image image, Sprite sprite, EnumPosition position, NTemplate node, Vector2 point, float duration, bool reverse = false)
        {
            if (!reverse)
                image.sprite = sprite;

            if (!reverse)
                image.CrossFadeAlpha(0, 0, false);

            image.CrossFadeAlpha(reverse ? 0 : 1, duration, false);

            if (!reverse)
            {
                Vector2 pos;
            
                if (position == EnumPosition.Custom)
                    pos = point;
                else
                    pos = new Vector2(EngineGraphUtilities.GetXPosition(position), .5f);

                image.rectTransform.anchoredPosition = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image.rectTransform, pos, DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);
            }

            yield return new WaitForSeconds(duration);

            node.endedInstruction = true;
        }

        public static IEnumerator SlideUp(Image image, Sprite sprite, EnumPosition position, NTemplate node, Vector2 point, float duration, bool reverse = false)
        {
            if (!reverse)
                image.sprite = sprite;

            var canvas = DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform;
            var trm = image.rectTransform;

            Vector2 posA;
            Vector2 posB;

            if (position != EnumPosition.Custom)
            {
                posA = new Vector2(EngineGraphUtilities.GetXPosition(position), -.5f);
                posB = new Vector2(posA.x, .5f);
            }

            else
            {
                posA = new Vector2(point.x, -.5f);
                posB = point;
            }

            posA = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(trm, posA, canvas);
            posB = reverse ? posB : EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(trm, posB, canvas);

            if (reverse)
                posA.x = posB.x;

            float lerpValue = 0;
            float result = 0;

            yield return new WaitUntil(() =>
            {
                if (reverse)
                    trm.anchoredPosition = Vector2.Lerp(posB, posA, result);
                else
                    trm.anchoredPosition = Vector2.Lerp(posA, posB, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / duration;

                return result >= 1;
            });

            node.endedInstruction = true;
        }

        public static IEnumerator SlideDown(Image image, Sprite sprite, EnumPosition position, NTemplate node, Vector2 point, float duration, bool reverse = false)
        {
            if (!reverse)
                image.sprite = sprite;

            var canvas = DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform;
            var trm = image.rectTransform;

            Vector2 posA;
            Vector2 posB;

            if (position != EnumPosition.Custom)
            {
                posA = new Vector2(EngineGraphUtilities.GetXPosition(position), 1.5f);
                posB = new Vector2(posA.x, .5f);
            }

            else
            {
                posA = new Vector2(point.x, 1.5f);
                posB = point;
            }

            posA = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(trm, posA, canvas);
            posB = reverse ? posB : EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(trm, posB, canvas);

            if (reverse)
                posA.x = posB.x;

            float lerpValue = 0;
            float result = 0;

            yield return new WaitUntil(() =>
            {
                if (reverse)
                    trm.anchoredPosition = Vector2.Lerp(posB, posA, result);
                else
                    trm.anchoredPosition = Vector2.Lerp(posA, posB, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / duration;

                return result >= 1;
            });

            node.endedInstruction = true;
        }

        public static IEnumerator SlideLeft(Image image, Sprite sprite, EnumPosition position, NTemplate node, Vector2 point, float duration, bool reverse = false)
        {
            if (!reverse)
                image.sprite = sprite;

            var canvas = DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform;
            var trm = image.rectTransform;

            Vector2 posA;
            Vector2 posB;

            if (position != EnumPosition.Custom)
            {
                posA = new Vector2(1.5f, .5f);
                posB = new Vector2(EngineGraphUtilities.GetXPosition(position), .5f);
            }

            else
            {
                posA = new Vector2(1.5f, point.y);
                posB = point;
            }

            posA = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(trm, posA, canvas);
            posB = reverse? posB : EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(trm, posB, canvas);

            if (reverse)
                posA.y = posB.y;

            float lerpValue = 0;
            float result = 0;

            yield return new WaitUntil(() =>
            {
                if (reverse)
                    trm.anchoredPosition = Vector2.Lerp(posB, posA, result);
                else
                    trm.anchoredPosition = Vector2.Lerp(posA, posB, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / duration;

                return result >= 1;
            });

            node.endedInstruction = true;
        }

        public static IEnumerator SlideRight(Image image, Sprite sprite, EnumPosition position, NTemplate node, Vector2 point, float duration, bool reverse = false)
        {
            if (!reverse)
                image.sprite = sprite;

            var canvas = DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform;
            var trm = image.rectTransform;

            Vector2 posA;
            Vector2 posB;

            if (position != EnumPosition.Custom)
            {
                posA = new Vector2(-.5f, .5f);
                posB = new Vector2(EngineGraphUtilities.GetXPosition(position), .5f);
            }

            else
            {
                posA = new Vector2(-.5f, point.y);
                posB = point;
            }

            posA = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(trm, posA, canvas);
            posB = reverse ? posB : EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(trm, posB, canvas);

            if (reverse)
                posA.y = posB.y;

            float lerpValue = 0;
            float result = 0;

            yield return new WaitUntil(() =>
            {
                if (reverse)
                    trm.anchoredPosition = Vector2.Lerp(posB, posA, result);
                else
                    trm.anchoredPosition = Vector2.Lerp(posA, posB, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / duration;

                return result >= 1;
            });

            node.endedInstruction = true;
        }
    }
}