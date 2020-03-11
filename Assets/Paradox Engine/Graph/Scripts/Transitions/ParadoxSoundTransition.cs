using System.Collections;
using UnityEngine;

namespace ParadoxEngine.Transitions
{
    public static class ParadoxSoundTransition
    {
        public static void Instante(AudioSource source1, AudioSource source2, AudioClip newClip, NTemplate node)
        {
            if (source1.isPlaying)
                source1.Stop();

            source1.volume = 0;

            source2.clip = newClip;
            source2.volume = 1;
            source2.Play();

            node.endedInstruction = true;
        }

        public static IEnumerator FadeIn(AudioSource source1, AudioSource source2, AudioClip newClip, NTemplate node, float duration)
        {
            if (source1.isPlaying)
                source1.Stop();

            source1.volume = 0;

            source2.clip = newClip;
            source2.Play();

            float lerpValue = 0;
            float result = 0;

            yield return new WaitUntil(() =>
            {
                source2.volume = Mathf.Lerp(0, 1, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / duration;

                return result > 1;
            });

            node.endedInstruction = true;
        }

        public static IEnumerator Crossfade(AudioSource source1, AudioSource source2, AudioClip newClip, NTemplate node, float duration)
        {
            source2.clip = newClip;
            source2.Play();

            float lerpValue = 0;
            float result = 0;

            yield return new WaitUntil(() =>
            {
                source1.volume = Mathf.Lerp(1f, 0f, result);
                source2.volume = Mathf.Lerp(0f, 1f, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / duration;

                return result > 1;
            });

            source1.volume = 0;
            source2.volume = 1;
            node.endedInstruction = true;
        }

        public static IEnumerator FadeOutAndIn(AudioSource source1, AudioSource source2, AudioClip newClip, NTemplate node, float duration)
        {
            source2.clip = newClip;
            source2.volume = 0;

            float lerpValue = 0;
            float result = 0;
            float midDuration = duration / 2;

            yield return new WaitUntil(() =>
            {
                source1.volume = Mathf.Lerp(1, 0, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / midDuration;

                return result > 1;
            });

            source1.Stop();
            source1.volume = 0;
            lerpValue = 0;
            result = 0;

            source2.Play();

            yield return new WaitUntil(() =>
            {
                source2.volume = Mathf.Lerp(0, 1, result);

                lerpValue += Time.deltaTime;
                result = lerpValue / midDuration;

                return result > 1;
            });

            node.endedInstruction = true;
        }
    }
}