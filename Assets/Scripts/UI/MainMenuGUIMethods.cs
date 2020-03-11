using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGUIMethods : MonoBehaviour
{
    public Animator animator;
    public Animator titleAnimator;
    public GameObject canvas;

    private int _hash;

    private void Awake() => _hash = Animator.StringToHash("state");


    public void HideTitle() => titleAnimator.SetInteger(_hash, 1);

    public void HideGUI() => animator.SetInteger(_hash, 1);

    private void DesactiveGUI()
    {
        animator.SetInteger(_hash, 0);
        canvas.SetActive(false);
    }
}
