using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParadoxEngine.Utilities.Parameters;

public class ExtraGUIEndsChecker : MonoBehaviour
{
    public Text[] ends;
    public Color unlocked;

    void Start()
    {
        Params parameters = DialogueDatabase.parameters;
        List<int> indexes = new List<int>();

        if (parameters.GetBool("Birthday").Value)
            indexes.Add(0);

        if (parameters.GetBool("CityDate").Value)
            indexes.Add(1);

        if (parameters.GetBool("AYearTogether").Value)
            indexes.Add(2);

        if (parameters.GetBool("Desist").Value)
            indexes.Add(3);

        if (parameters.GetBool("ArcueidWars").Value)
            indexes.Add(4);

        if (parameters.GetBool("TrueAwaken").Value)
            indexes.Add(5);

        if (parameters.GetBool("BloodPath").Value)
            indexes.Add(6);

        if (parameters.GetBool("SuddenGoodbye").Value)
            indexes.Add(7);

        if (parameters.GetBool("Promise").Value)
            indexes.Add(8);

        if (parameters.GetBool("EternalPersuit").Value)
            indexes.Add(9);

        if (parameters.GetBool("BloodPrincess").Value)
            indexes.Add(10);

        if (parameters.GetBool("BrunestudHell").Value)
            indexes.Add(11);

        if (parameters.GetBool("ACatIsFineToo").Value)
            indexes.Add(12);

        if (parameters.GetBool("HappyBirthdayArc").Value)
            indexes.Add(13);


        foreach (int index in indexes)
        {
            ends[index].color = unlocked;
            ends[index].GetComponent<TextLocalizationSetter>().UnlockText();
        }
    }
}
