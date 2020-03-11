using UnityEngine;

namespace ParadoxFramework
{
    /// <summary>
    /// Interface que determina a un objeto como pooleable.
    /// </summary>
    public interface IPoolObject
    {
        GameObject FactoryMethod();
        void Dispose(int hashInstance);
    }
}