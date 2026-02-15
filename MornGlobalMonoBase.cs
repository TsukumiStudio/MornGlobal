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
                    if (_instance == null)
                    {
                        var obj = new GameObject(typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                        _instance.OnInitialized();
                        DontDestroyOnLoad(_instance.gameObject);
                        I.Logger.Log($"{I.ModuleName}/{typeof(T).Name}を生成しました。");
                    }
                }

                return _instance;
            }
        }
        private MornGlobalLogger _logger;
        private MornGlobalLogger Logger => _logger ??= new MornGlobalLogger(this);
        string IMornGlobal.ModuleName => ModuleName;
        protected abstract string ModuleName { get; }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
                Logger.Log($"{ModuleName}/{typeof(T).Name}を読み込みました。");
                OnInitialized();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected abstract void OnInitialized();
    }
}