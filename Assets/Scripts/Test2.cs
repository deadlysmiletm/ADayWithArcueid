using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine;
using ParadoxEngine.Utilities.Parameters;

public class Test2 : MonoBehaviour
{
    public enum ERandomChoice { Normal, Regazo, Tarde, Archetype }

    private Params _parameters;
    public List<(ERandomChoice, int)> values = new List<(ERandomChoice, int)>
    {
        (ERandomChoice.Normal, 80),
        (ERandomChoice.Regazo, 90),
        (ERandomChoice.Tarde, 40),
        (ERandomChoice.Archetype, 35)
    };
    private float[] _pvalues = new float[4];

    private void Awake() => _parameters = GameObject.FindObjectOfType<GraphPlayerBehaviour>().graph.parameters;

    public void ExecuteCalculation()
    {
        float total = 0;

        foreach (var item in values)
            total += item.Item2;

        List<float> temp = new List<float>();

        for (int i = 0; i < _pvalues.Length; i++)
        {
            _pvalues[i] = values[i].Item2 / total;
            temp.Add(_pvalues[i]);
        }

        var select = RWSelection(ref temp);
        _parameters.GetInt("Random").Value = (int)values[select].Item1;
    }

    private int RWSelection(ref List<float> lista)
    {
        var rand = Random.Range(0f, 1f);
        var acum = 0f;

        for (int i = 0; i < lista.Count; i++)
        {
            acum += lista[i];

            if (acum > rand)
                return i;
        }

        return -1;
    }
}
