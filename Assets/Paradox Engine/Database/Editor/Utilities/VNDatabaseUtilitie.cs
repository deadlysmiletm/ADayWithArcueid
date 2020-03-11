using UnityEngine;
using UnityEditor;

namespace ParadoxEngine
{
    public enum DataContainerType
    {
        Character,
        Location,
        Soundtrack,
        Setting,
        Default
    }

    public class VNDatabaseUtilitie : Editor
    {
        public static void DeleteElem(PEData chara)
        {
            GameObject.DestroyImmediate(chara, true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}