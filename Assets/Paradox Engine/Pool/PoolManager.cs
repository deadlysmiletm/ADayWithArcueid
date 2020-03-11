using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParadoxFramework.Exceptions;

namespace ParadoxFramework
{
    public sealed class PoolManager : MonoBehaviour, IEnumerable<IEnumerable<GameObject>>
    {
        [SerializeField]
        private Dictionary<string, PoolData> _pools;

        [Serializable]
        public struct PoolData
        {
            public List<GameObject> Pool;
            public bool IsDinamyc;

            public Func<GameObject> OnFactory;
            public Action<int> OnDispose;
        }

        /// <summary>
        /// Crea un nuevo pool.
        /// </summary>
        /// <param name="poolName">Nombre para el pool.</param>
        /// <param name="amount">Cantidad de elementos.</param>
        /// <param name="factoryMethod">Método de creación del objeto.</param>
        /// <param name="disposeMethod">Método de dispose del objeto.</param>
        /// <param name="isDinamyc">Determina si el pool es dinámico o no.</param>
        public void CreatePool(string poolName, int amount, Func<GameObject> factoryMethod, Action<int> disposeMethod, bool isDinamyc = false)
        {
            if (_pools == null)
                _pools = new Dictionary<string, PoolData>();

            PoolData data = new PoolData { Pool = new List<GameObject>(), OnFactory = factoryMethod, OnDispose = disposeMethod, IsDinamyc = isDinamyc };

            for (int i = 0; i < amount; i++)
            {
                GameObject temp = factoryMethod();
                temp.SetActive(false);
                
                data.Pool.Add(temp);
            }

            _pools.Add(poolName, data);
        }

        /// <summary>
        /// Retorna un objeto del pool pedido.
        /// </summary>
        /// <param name="poolName">Nombre del pool existente.</param>
        /// <returns></returns>
        public GameObject GetPoolObject(string poolName)
        {
            if (CheckNullException())
                return null;

            if (!_pools.ContainsKey(poolName))
            {
                string temp = "";

                foreach (var item in _pools)
                    temp += item.Key + ",";

                temp.Remove(temp.Length - 2);

                this.PrintException($"El Pool llamado {poolName} no existe dentro del Pool Manager.", LogType.Warning);
                this.PrintException($"Sólo existen los siguientes Pools: {temp}.", LogType.Warning);
                return null;
            }

            if (!_pools[poolName].Pool.Any())
            {
                if (_pools[poolName].IsDinamyc)
                {
                    GameObject newInstance = _pools[poolName].OnFactory();
                    newInstance.SetActive(true);

                    return newInstance;
                }

#if UNITY_EDITOR
                this.PrintException($"No hay más objetos en el Pool {poolName}. Si desea crear más, hagalo dinámico.", LogType.Warning);
#endif
                return null;
            }

            var instance = _pools[poolName].Pool[0];
            _pools[poolName].Pool.RemoveAt(0);
            instance.SetActive(true);

            return instance;
        }

        /// <summary>
        /// Retorna el objeto al pool.
        /// </summary>
        /// <param name="poolName">Nombre del pool al que pertenece.</param>
        /// <param name="poolObject">Objeto a devolver al pool.</param>
        public void DisposePoolObject(string poolName, GameObject poolObject)
        {
            if (CheckNullException() || CheckContainsKeyException(poolName))
                return;

            _pools[poolName].OnDispose(poolObject.GetHashCode());
            poolObject.SetActive(false);

            _pools[poolName].Pool.Add(poolObject);
        }

        /// <summary>
        /// Cambia si un pool es dinámico o no.
        /// </summary>
        /// <param name="poolName">Nombre del pool.</param>
        /// <param name="isDinamyc">Si es o no dinámico.</param>
        public void SetDinamyc(string poolName, bool isDinamyc)
        {
            if (CheckNullException() || CheckContainsKeyException(poolName))
                return;

            PoolData temp = _pools[poolName];
            temp.IsDinamyc = isDinamyc;

            _pools[poolName] = temp;
        }


        private bool CheckContainsKeyException(string poolName)
        {
            if (!_pools.ContainsKey(poolName))
            {
                string temp = "";

                foreach (var item in _pools)
                    temp += item.Key + ",";

                temp.Remove(temp.Length - 2);

                this.PrintException($"El Pool llamado {poolName} no existe dentro del Pool Manager.", LogType.Error);
                this.PrintException($"Sólo existen los siguientes Pools: {temp}.", LogType.Error);
                return true;
            }

            return false;
        }

        private bool CheckNullException()
        {
            if (_pools == null)
            {
                this.PrintException("El PoolManager no ha sido inicializado en ningún momento.", LogType.Error);
                return true;
            }

            return false;
        }


        public void Dispose() => _pools.Clear();


        public IEnumerator<IEnumerable<GameObject>> GetEnumerator()
        {
            foreach (var item in _pools)
                yield return item.Value.Pool;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return GetEnumerator();
        }
    }
}