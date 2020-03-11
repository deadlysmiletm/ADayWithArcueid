using System.Collections;
using UnityEngine;

public class ButtonGUI : MonoBehaviour
{
    [HideInInspector] public AudioSource fxSource;

    public void PlayOnShotSound(AudioClip soundClip) => fxSource.PlayOneShot(soundClip);
}
