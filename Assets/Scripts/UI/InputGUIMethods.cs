using UnityEngine;
using UnityEngine.UI;
using System;

public class InputGUIMethods : MonoBehaviour
{
    public InputField inputUI;
    public Animator animator;
    public GameObject canvas;
    private int _hash;

    public Action OnConfirmInput;

    private void Awake() => _hash = Animator.StringToHash("state");

    public void HideInputGUI()
    {
        ReturnStateValue();
        OnConfirmInput();
        canvas.SetActive(false);
    }

    public void CheckInputGUI()
    {
        if (CheckNameParameter())
        {
            animator.SetInteger(_hash, 1);
            return;
        }

        UpdateName();
        animator.SetInteger(_hash, 2);
    }

    public bool CheckNameParameter() => String.IsNullOrWhiteSpace(inputUI.text) || inputUI.text == "";

    public void ReturnStateValue() => animator.SetInteger(_hash, 0);

    private void UpdateName() => DialogueDatabase.parameters.UpdateValue("Name", inputUI.text);
}
