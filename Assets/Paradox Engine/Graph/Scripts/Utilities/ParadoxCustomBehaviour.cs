using UnityEngine;

namespace ParadoxEngine
{
    public abstract class ParadoxCustomBehaviour : ScriptableObject
    {
        public bool IsBehaviourEnded { get; protected set; }

        public abstract void OnEnterBehaviour();
        public abstract void OnExecuteBehaviour();
        public abstract void OnExitBehaviour();

        public void SetBehaviourEnd() => IsBehaviourEnded = true;
    }
}