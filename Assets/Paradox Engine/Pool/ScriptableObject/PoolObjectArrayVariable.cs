using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ParadoxFramework.Exceptions;

namespace ParadoxFramework
{
    [CreateAssetMenu(menuName = "Pool/PoolObject Array")]
    [System.Serializable]
    public class PoolObjectArrayVariable : ScriptableObject
    {
        [System.Serializable]
        public struct PoolObjectData
        {
            public string Name;
            public GameObject Prefab;
            public int Amount;
            public bool IsDinamyc;
        }

        [SerializeField]
        public List<PoolObjectData> poolObjects;

        public void Add(string name, GameObject prefab, int amount, bool dinamyc) => poolObjects.Add(new PoolObjectData() { Name = name, Amount = amount, Prefab = prefab, IsDinamyc = dinamyc });

        public void Remove(string name)
        {
            if (!poolObjects.Any(x => x.Name == name))
            {
                this.PrintException($"Se ha intentado remover un PoolObject llamado {name}, pero no existe en {this.name}", LogType.Error);
                return;
            }

            poolObjects.Remove(poolObjects.Where(x => x.Name == name).First());
        }

        public void Remove(GameObject prefab)
        {
            if (!poolObjects.Any(x => x.Prefab == prefab))
            {
                this.PrintException($"Se ha intentado remover un PoolObject con el prefab {prefab.name}, pero no existe en {this.name}", LogType.Error);
                return;
            }

            poolObjects.Remove(poolObjects.Where(x => x.Prefab == prefab).First());
        }

        public void RemoveAt(int index)
        {
            if (index > poolObjects.Count())
            {
                this.PrintException($"El índice es mayor al tamaño del array en {this.name}.", LogType.Error);
                return;
            }

            poolObjects.RemoveAt(index);
        }
    }
}