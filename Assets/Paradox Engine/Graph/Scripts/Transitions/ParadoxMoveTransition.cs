using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ParadoxEngine.Utilities;

namespace ParadoxEngine.Transitions
{
    public class ParadoxMoveTransition
    {
        public static void InstanteToPosition(RectTransform trm, EnumPosition position, NTemplate node, Vector2 point)
        {
            var canvas = DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform;

            Vector2 pos;

            if (position != EnumPosition.Custom)
                pos = new Vector2(EngineGraphUtilities.GetXPosition(position), .5f);
            else
                pos = point;

            pos = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(trm, pos, canvas);

            trm.anchoredPosition = pos;

            node.endedInstruction = true;
        }

        public static IEnumerator SlideToPosition(RectTransform trm, EnumPosition position, NTemplate node, Vector2 point, float duration)
        {
            var canvas = DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform;

            Vector2 posA = trm.anchoredPosition;
            Vector2 posB;

            if (position != EnumPosition.Custom)
                posB = new Vector2(EngineGraphUtilities.GetXPosition(position), .5f);
            else
                posB = point;

            posB = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(trm, posB, canvas);

            float lerpValue = 0;
            float result = 0;

            yield return new WaitUntil(() =>
            {
                trm.anchoredPosition = Vector2.Lerp(posA, posB, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / duration;

                return result >= 1;
            });

            node.endedInstruction = true;
        }

        public static IEnumerator FadeToPosition(Image image, EnumPosition position, NTemplate node, Vector2 point, float duration)
        {
            var canvas = DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform;

            Vector2 pos;

            if (position != EnumPosition.Custom)
                pos = new Vector2(EngineGraphUtilities.GetXPosition(position), .5f);
            else
                pos = point;

            pos = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image.rectTransform, pos, canvas);

            var wait = new WaitForSeconds(duration / 2);

            image.CrossFadeAlpha(0, duration / 2, false);

            yield return wait;

            image.rectTransform.anchoredPosition = pos;
            image.CrossFadeAlpha(1, duration / 2, false);

            yield return wait;

            node.endedInstruction = true;
        }
    }
}