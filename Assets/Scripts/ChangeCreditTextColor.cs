using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCreditTextColor : MonoBehaviour
{
    public Text creditsText;

    public void ChangeColor() => creditsText.color = Color.white;
}
