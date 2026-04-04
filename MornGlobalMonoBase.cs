using UnityEngine;

namespace MornLib
{
    public abstract class MornGlobalMonoBase<T> : MonoBehaviour, IMornGlobal where T : MornGlobalMonoBase<T>
    {
        private static T _instance;
        public static T I
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>();
                    if (_instance != null)
                    {
                        _instance.transform.SetParent(null);
                        DontDestroyOnLoad(_instance.gameObject);
                        _instance.OnInitialized();
                        Logger.Log($"{typeof(T).Name}を検出しました。");
                    }
                    else
                    {
                        var obj = new GameObject(typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                        Logger.Log($"{typeof(T).Name}を新規生成しました。");
                    }
                }

                return _instance;
            }
        }
        private static MornGlobalLogger _logger;
        public static MornGlobalLogger Logger => _logger ??= new MornGlobalLogger(I);
        string IMornGlobal.ModuleName => ModuleName;
        Color IMornGlobal.ModuleColor => ModuleColor;
        protected abstract string ModuleName { get; }
        protected virtual Color ModuleColor => Color.green;

        private void Awake()
        {
            // I経由で既に初期化済み
            if (_instance == this) return;
            if (_instance != null)
            {
                Logger.LogError($"{typeof(T).Name}が重複しています。破棄します: {gameObject.name}");
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            OnInitialized();
            Logger.Log($"{typeof(T).Name}を読み込みました。");
        }

        protected abstract void OnInitialized();
    }
}