using UnityEngine;
using ParadoxEngine.Utilities.Parameters;
using UnityEngine.SceneManagement;
using System;

public class ExtraUtilities : MonoBehaviour
{
    public Animator doorAnimator;
    private int _indexHash;


    public void ExitGame() => Application.Quit();

    public void ChangeScene(string scene) => SceneManager.LoadScene(scene, LoadSceneMode.Single);

    public void LoadSaveFile() => SaveHelper.LoadChanges(DialogueDatabase.parameters);

    public void BakeParameterHash() => _indexHash = Animator.StringToHash("Index");

    public void ChangeAnimatorClip(int index) => doorAnimator.SetInteger(_indexHash, index);

    public void SetLastSession()
    {
        var date = DateTime.Now.ToShortDateString();
        var dividedDate = date.Split('/');

        DialogueDatabase.activeGraphPlayer.graph.SetBool("ArcDay", dividedDate[0] == "25" && dividedDate[1] == "12");
    }
}