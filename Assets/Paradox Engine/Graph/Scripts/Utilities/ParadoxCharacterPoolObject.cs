using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParadoxFramework;

public class ParadoxCharacterPoolObject : MonoBehaviour, IPoolObject
{
    public void Dispose(int hashInstance) { }
    public GameObject FactoryMethod() => Instantiate(this.gameObject, DialogueDatabase.activeGraphPlayer.characterContainer);
}
