using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParadoxEngine.Utilities;
using ParadoxEngine.Utilities.Parameters;

public class DebugPanelParameters : MonoBehaviour
{
    public Text playTime;
    public Text arcueidBond;
    public Text random;
    public Text ended;
    public Text archetypeDefeated;
    public Text catEnds;

    public Params parameters;

    void Awake() => parameters = EngineGraphUtilities.LoadParameterReference();

    void Update()
    {
        playTime.text = parameters.GetInt("PlayTimes").Value.ToString();
        arcueidBond.text = parameters.GetInt("Arcueid bond").Value.ToString();
        random.text = parameters.GetInt("Random").Value.ToString();
        ended.text = parameters.GetBool("Ended").Value.ToString();
        archetypeDefeated.text = parameters.GetBool("ArchetypeDefeated").Value.ToString();
        catEnds.text = parameters.GetInt("CatEnds").Value.ToString();
    }

    public void UpValue(string param)
    {
        if (param == "PlayTimes")
            parameters.GetInt("PlayTimes").Value++;

        else if (param == "Arcueid bond")
            parameters.GetInt("Arcueid bond").Value++;

        else if (param == "CatEnds")
            parameters.GetInt("CatEnds").Value++;

        SaveHelper.SaveChanges(EngineGraphUtilities.LoadParameterReference());
    }

    public void DownValue(string param)
    {
        if (param == "PlayTimes")
            parameters.GetInt("PlayTimes").Value--;

        else if (param == "Arcueid bond")
            parameters.GetInt("Arcueid bond").Value--;

        else if (param == "CatEnds")
            parameters.GetInt("CatEnds").Value--;

        SaveHelper.SaveChanges(EngineGraphUtilities.LoadParameterReference());
    }

    public void SwampValue(string param)
    {
        BoolSerializedParamVaraible temp;

        if (param == "Ended")
            temp = parameters.GetBool("Ended");

        else if (param == "ArchetypeDefeated")
            temp = parameters.GetBool("ArchetypeDefeated");

        else
            return;

        temp.Value = !temp.Value;

        SaveHelper.SaveChanges(EngineGraphUtilities.LoadParameterReference());
    }
}
