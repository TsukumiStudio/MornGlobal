using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornLib
{
    public static class MornGlobalUtil
    {
        public static void SetDirty(Object target)
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(target);
#endif
        }

        public static void EnsurePreloadedAsset<T>(T instance) where T : ScriptableObject
        {
#if UNITY_EDITOR
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            if (preloadedAssets.Contains(instance) && preloadedAssets.Count(x => x is T) == 1)
            {
                return;
            }

            preloadedAssets.RemoveAll(x => x is T);
            preloadedAssets.Add(instance);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
#endif
        }

        public static T FindOrCreatePreloadedAsset<T>() where T : ScriptableObject
        {
#if UNITY_EDITOR
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            var instance = preloadedAssets.OfType<T>().FirstOrDefault();
            if (instance != null)
            {
                return instance;
            }

            var path = EditorUtility.SaveFilePanelInProject($"Save {typeof(T).Name}", $"{typeof(T).Name}", "asset", string.Empty);
            if (!string.IsNullOrEmpty(path))
            {
                var newSettings = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(newSettings, path);
                preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
                preloadedAssets.RemoveAll(x => x is T);
                preloadedAssets.Add(newSettings);
                PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            }

            return AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                                .Select(AssetDatabase.GUIDToAssetPath)
                                .Select(AssetDatabase.LoadAssetAtPath<T>)
                                .FirstOrDefault();
#else
            return null;
#endif
        }
    }
}
