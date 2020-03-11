using UnityEngine;

public class SelectMenuBackground : MonoBehaviour
{
    public InputGUIMethods inputCanvas;
    public GameObject menuCanvas;
    public GameObject optionCanvas;
    public GameObject extraCanvas;


    public void GetInterface()
    {
        if (System.String.IsNullOrEmpty(DialogueDatabase.parameters.GetString("Name").Value))
        {
            inputCanvas.OnConfirmInput = () => menuCanvas.gameObject.SetActive(true);

            ShowInputGUI();
            return;
        }

        ShowMenuGUI();
    }


    public void ShowInputGUI() => inputCanvas.transform.parent.gameObject.SetActive(true);


    public void ShowMenuGUI() => menuCanvas.SetActive(true);

    public void ShowOptionGUI() => optionCanvas.SetActive(true);

    public void ShowExtraGUI() => extraCanvas.SetActive(true);
}