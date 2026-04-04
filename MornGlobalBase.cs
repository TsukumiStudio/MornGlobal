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
                    _instance = MornGlobalUtil.FindOrCreatePreloadedAsset<T>();
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
            if (_instance != null && _instance != this)
            {
                var message = $"{typeof(T).Name}が重複しています。破棄します: {name}";
                Logger.LogWarning(message);
#if UNITY_EDITOR
                var self = this;
                var assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    UnityEditor.EditorUtility.DisplayDialog($"{typeof(T).Name} 重複", message, "OK");
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        UnityEditor.AssetDatabase.DeleteAsset(assetPath);
                    }
                    else
                    {
                        DestroyImmediate(self, true);
                    }
                };
#else
                DestroyImmediate(this, true);
#endif
                return;
            }

            _instance = (T)this;
            Logger.Log($"{typeof(T).Name}を読み込みました。");
            MornGlobalUtil.EnsurePreloadedAsset(I);
        }

        private void OnDisable()
        {
            _instance = null;
            Logger.Log($"{typeof(T).Name}をアンロードしました。");
        }
    }
}