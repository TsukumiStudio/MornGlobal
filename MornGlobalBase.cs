using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MornLib
{
    public abstract class MornGlobalBase<T> : ScriptableObject, IMornGlobal where T : MornGlobalBase<T>
    {
        private static T _instance;
        public static T I
        {
            get
            {
#if UNITY_EDITOR
                if (_instance == null)
                {
                    {
                        var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
                        _instance = preloadedAssets.OfType<T>().FirstOrDefault();
                        if (_instance != null)
                        {
                            return _instance;
                        }
                    }
                    var path = EditorUtility.SaveFilePanelInProject($"Save {typeof(T).Name}", $"{typeof(T).Name}", "asset", string.Empty);
                    if (!string.IsNullOrEmpty(path))
                    {
                        var newSettings = CreateInstance<T>();
                        AssetDatabase.CreateAsset(newSettings, path);
                        var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
                        preloadedAssets.RemoveAll(x => x is T);
                        preloadedAssets.Add(newSettings);
                        PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
                    }

                    _instance = AssetDatabase.FindAssets($"t:{typeof(T).Name}").Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>).FirstOrDefault();
                }
#endif
                return _instance;
            }
        }
        private static MornGlobalLogger _logger;
        public static MornGlobalLogger Logger => _logger ??= new MornGlobalLogger(I);
        string IMornGlobal.ModuleName => ModuleName;
        protected abstract string ModuleName { get; }

        private void OnEnable()
        {
            _instance = (T)this;
            LogInternal($"{ModuleName}/{typeof(T).Name}を読み込みました。");
#if UNITY_EDITOR
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            if (preloadedAssets.Contains(I) && preloadedAssets.Count(x => x is T) == 1)
            {
                return;
            }

            preloadedAssets.RemoveAll(x => x is T);
            preloadedAssets.Add(I);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            LogInternal($"{ModuleName}/{typeof(T).Name}をPreloadedAssetsに追加しました。");
#endif
        }

        private void OnDisable()
        {
            _instance = null;
            LogInternal($"{ModuleName}/{typeof(T).Name}をアンロードしました。");
        }

        protected void LogInternal(string message)
        {
            Logger.Log(message);
        }

        protected void LogErrorInternal(string message)
        {
            Logger.LogError(message);
        }

        protected void LogWarningInternal(string message)
        {
            Logger.LogWarning(message);
        }

        protected void SetDirtyInternal(Object target)
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(target);
#endif
        }
    }
}