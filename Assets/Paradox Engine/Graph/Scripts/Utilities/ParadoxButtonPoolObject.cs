using UnityEngine;
using ParadoxFramework;

public class ParadoxButtonPoolObject : MonoBehaviour, IPoolObject
{
    public void Dispose(int hashInstance) { }
    public GameObject FactoryMethod() => Instantiate(this.gameObject, DialogueDatabase.activeGraphPlayer.Canvas.transform);
}