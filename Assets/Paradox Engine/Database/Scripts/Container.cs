using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParadoxEngine;

[CreateAssetMenu(menuName = "Paradox Engine/Container")]
public class Container : ScriptableObject
{
    [SerializeField]
    public List<PEData> data;
}
