using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ParadoxEngine.Utilities;

namespace ParadoxEngine.Transitions
{
    public static partial class ParadoxImageTransition
    {
        public static void Instante(Image image1, Image image2, Sprite newImage, NTemplate node)
        {
            image2.gameObject.SetActive(true);
            image2.sprite = newImage;

            image1.CrossFadeAlpha(0, 0, false);
            image2.CrossFadeAlpha(1, 0, false);

            image1.gameObject.SetActive(false);
            node.endedInstruction = true;
        }

        public static IEnumerator Fade(Image image1, Image image2, Sprite newImage, NTemplate node, float duration)
        {
            image2.rectTransform.anchoredPosition = image1.rectTransform.anchoredPosition;
            image2.CrossFadeAlpha(0, 0, false);
            image2.gameObject.SetActive(true);
            image2.sprite = newImage;

            image1.CrossFadeAlpha(0, duration, false);
            image2.CrossFadeAlpha(1, duration, false);

            yield return new WaitForSeconds(duration);

            image1.gameObject.SetActive(false);
            node.endedInstruction = true;
        }

        public static IEnumerator FadeOutAndIn(Image image1, Image image2, Sprite newImage, NTemplate node, float duration)
        {
            image2.rectTransform.anchoredPosition = image1.rectTransform.anchoredPosition;
            image2.CrossFadeAlpha(0, 0, false);
            image2.gameObject.SetActive(true);
            image2.sprite = newImage;
            image2.CrossFadeAlpha(0, 0, false);
            var wait = new WaitForSeconds(duration / 2);

            image1.CrossFadeAlpha(0, duration / 2, false);

            yield return wait;

            image2.CrossFadeAlpha(1, duration / 2, false);

            yield return wait;

            image1.gameObject.SetActive(false);
            node.endedInstruction = true;
        }

        public static IEnumerator SlideUp(Image image1, Image image2, Sprite newImage, NTemplate node, float duration)
        {
            image2.CrossFadeAlpha(1, 0, false);
            image2.gameObject.SetActive(true);
            image2.sprite = newImage;

            var center = new Vector2(.5f, .5f);

            var trm1 = image1.rectTransform;
            var trm2 = image2.rectTransform;

            var pos1Begin = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image1.rectTransform, center, DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform); ;
            var pos1End = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image1.rectTransform, new Vector2(.5f, 1.5f), DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);

            var pos2End = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image2.rectTransform, center, DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);
            var pos2Begin = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image2.rectTransform, new Vector2(.5f, -.5f), DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);

            float lerpValue = 0;
            float result = 0;

            yield return new WaitUntil(() =>
            {
                trm1.anchoredPosition = Vector2.Lerp(pos1Begin, pos1End, result);
                trm2.anchoredPosition = Vector2.Lerp(pos2Begin, pos2End, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / duration;

                return result > 1.01;
            });


            image1.gameObject.SetActive(false);
            node.endedInstruction = true;
        }

        public static IEnumerator SlideDown(Image image1, Image image2, Sprite newImage, NTemplate node, float duration)
        {
            image2.CrossFadeAlpha(1, 0, false);
            image2.gameObject.SetActive(true);
            image2.sprite = newImage;

            var trm1 = image1.rectTransform;
            var trm2 = image2.rectTransform;
            var center = new Vector2(.5f, .5f);

            var pos1Begin = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image1.rectTransform, center, DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform); ;
            var pos1End = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image1.rectTransform, new Vector2(.5f, -.5f), DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);

            var pos2End = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image2.rectTransform, center, DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);
            var pos2Begin = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image2.rectTransform, new Vector2(.5f, 1.5f), DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);

            float lerpValue = 0;
            float result = 0;

            yield return new WaitUntil(() =>
            {
                trm1.anchoredPosition = Vector2.Lerp(pos1Begin, pos1End, result);
                trm2.anchoredPosition = Vector2.Lerp(pos2Begin, pos2End, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / duration;

                return result > 1.01;
            });


            image1.gameObject.SetActive(false);
            node.endedInstruction = true;
        }

        public static IEnumerator SlideRight(Image image1, Image image2, Sprite newImage, NTemplate node, float duration)
        {
            image2.CrossFadeAlpha(1, 0, false);
            image2.gameObject.SetActive(true);
            image2.sprite = newImage;

            var trm1 = image1.rectTransform;
            var trm2 = image2.rectTransform;
            var center = new Vector2(.5f, .5f);

            var pos1Begin = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image1.rectTransform, center, DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);
            var pos1End = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image1.rectTransform, new Vector2(1.5f, .5f), DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);

            var pos2End = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image2.rectTransform, center, DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);
            var pos2Begin = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image2.rectTransform, new Vector2(-.5f, .5f), DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);

            float lerpValue = 0;
            float result = 0;

            yield return new WaitUntil(() =>
            {
                trm1.anchoredPosition = Vector2.Lerp(pos1Begin, pos1End, result);
                trm2.anchoredPosition = Vector2.Lerp(pos2Begin, pos2End, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / duration;

                return result > 1.01;
            });

            trm2.anchoredPosition = pos2End;

            image1.gameObject.SetActive(false);
            node.endedInstruction = true;
        }

        public static IEnumerator SlideLeft(Image image1, Image image2, Sprite newImage, NTemplate node, float duration)
        {
            image2.CrossFadeAlpha(1, 0, false);
            image2.gameObject.SetActive(true);
            image2.sprite = newImage;

            var trm1 = image1.rectTransform;
            var trm2 = image2.rectTransform;
            var center = new Vector2(.5f, .5f);

            var pos1Begin = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image1.rectTransform, center, DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);
            var pos1End = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image1.rectTransform, new Vector2(-.5f, .5f), DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);

            var pos2End = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image2.rectTransform, center, DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);
            var pos2Begin = EngineGraphUtilities.FromAbsolutePositionToAnchoredPosition(image1.rectTransform, new Vector2(1.5f, .5f), DialogueDatabase.activeGraphPlayer.Canvas.transform as RectTransform);

            float lerpValue = 0;
            float result = 0;

            yield return new WaitUntil(() =>
            {
                trm1.anchoredPosition = Vector2.Lerp(pos1Begin, pos1End, result);
                trm2.anchoredPosition = Vector2.Lerp(pos2Begin, pos2End, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / duration;

                return result > 1.01;
            });


            image1.gameObject.SetActive(false);
            node.endedInstruction = true;
        }
    }
}