using UnityEngine;

public class TitleGUIMethod : MonoBehaviour
{
    public GraphPlayerBehaviour behaviour;

    private void PlayGame()
    {
        behaviour.Play();
        this.gameObject.SetActive(false);
    }
}
