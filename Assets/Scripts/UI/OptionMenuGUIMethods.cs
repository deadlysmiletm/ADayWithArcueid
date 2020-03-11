using UnityEngine;
using UnityEngine.UI;

public class OptionMenuGUIMethods : MonoBehaviour
{
    public Animator animator;
    public InputGUIMethods inputUI;

    private int _hash;

    private void Awake()
    {
        _hash = Animator.StringToHash("state");
        inputUI.OnConfirmInput = () => gameObject.SetActive(true);
    }


    public void HideGUI() => animator.SetInteger(_hash, 1);

    private void DesactiveGUI()
    {
        animator.SetInteger(_hash, 0);
        this.gameObject.SetActive(false);
    }
}
