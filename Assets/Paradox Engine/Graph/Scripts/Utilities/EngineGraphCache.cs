using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxEngine.Utilities
{
    [CreateAssetMenu(menuName = "Paradox Engine/Cache")]
    public class EngineGraphCache : ScriptableObject
    {
        [SerializeField]
        private string _lastSessionPath;

        public string LastSessionPath
        {
            get
            {
                if (_lastSessionPath == null)
                    _lastSessionPath = "";

                return _lastSessionPath;
            }

            set
            {
                if (value[0] == '#')
                {
                    _lastSessionPath = "";
                    return;
                }

                if (!value.Contains("Assets"))
                    return;

                _lastSessionPath = value;
            }
        }
    }
}