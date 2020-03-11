using UnityEngine;
using UnityEditor;
using ParadoxEngine.Utilities;

namespace ParadoxEngine.Utilities
{
    public static class EngineGraphCacheUtilities
    {
        private static string _sessionCachePatch = "Assets/Paradox Engine/Graph/Editor/Cache/SessionCache.asset";

        public static void SaveSessionCache(string path)
        {
            var cache = SearchSessionCache();
            cache.LastSessionPath = path;

            EditorUtility.SetDirty(cache);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public static EngineGraphCache SearchSessionCache() { return (EngineGraphCache)AssetDatabase.LoadAssetAtPath(_sessionCachePatch, typeof(EngineGraphCache)); }

        public static void ClearSessionCache()
        {
            EngineGraphCache cache = SearchSessionCache();
            cache.LastSessionPath = "#";

            EditorUtility.SetDirty(cache);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void LoadSessionCache()
        {
            var cache = SearchSessionCache();

            if (cache.LastSessionPath == "" || !cache.LastSessionPath.Contains("Assets"))
                return;

            EngineGraphEditorUtilities.LoadSession(cache.LastSessionPath, true);
        }
    }
}
