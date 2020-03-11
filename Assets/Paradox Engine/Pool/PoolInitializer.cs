using UnityEngine;

namespace ParadoxFramework
{
    public class PoolInitializer : MonoBehaviour
    {
        public PoolObjectArrayVariable poolsRequest;
        public PoolManager poolManager;

        //Crea todos los pools y luego se destruye.
        void Start()
        {
            for (int i = 0; i < poolsRequest.poolObjects.Count; i++)
            {
                var data = poolsRequest.poolObjects[i];
                var poolComponent = data.Prefab.GetComponent<IPoolObject>();

                poolManager.CreatePool(data.Name, data.Amount, poolComponent.FactoryMethod, poolComponent.Dispose, data.IsDinamyc);
            }

            Destroy(this);
        }
    }
}